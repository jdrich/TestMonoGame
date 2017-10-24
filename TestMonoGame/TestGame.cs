using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Windows.Forms;

using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace TestMonoGame
{
    public class TestGame : Game
    {
        protected bool UseFullscreen = false;

        protected bool MouseOn = true;

        protected Vector2 screenWidthHeight;
        protected IntVector2 lastScreenWidthHeight;

		protected Debug debug;

		protected KeyUpListener keyUpListener;

        protected GraphicsDeviceManager graphics;
        
        //Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //BasicEffect for rendering
        BasicEffect basicEffect;

        //Geometric info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);

            screenWidthHeight = new Vector2(0, 0);
            lastScreenWidthHeight = new IntVector2();

            Content.RootDirectory = "Content/bin";
        }

        protected void ClientSizeChanged(object sender, EventArgs e)
        {
            int width = Window.ClientBounds.Width;
            int height = Window.ClientBounds.Height;

            if (screenWidthHeight.X != width || screenWidthHeight.Y != height)
            {
                screenWidthHeight.X = width;
                screenWidthHeight.Y = height;
                
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.ApplyChanges();
            }
        }

		protected void ToggleDebug() {
			debug.Display = !debug.Display;
		}

        protected override void Initialize()
        {
            base.Initialize();
            
            Mouse.WindowHandle = Window.Handle;

            keyUpListener = new KeyUpListener ();

			SetupFullscreenToggleListener ();
			SetupExitListener ();
            SetupDebugToggleListener();

            debug = new Debug (Content.Load<SpriteFont>("Debug"), graphics);

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(ClientSizeChanged);

            Init3d();
        }

        protected void Init3d()
        {
            //Setup Camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f),
                               GraphicsDevice.DisplayMode.AspectRatio,
                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.
                          Forward, Vector3.Up);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            // Want to see the colors of the vertices, this needs to be on
            basicEffect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a custom def
            basicEffect.LightingEnabled = false;

            //Geometry  - a simple triangle about the origin
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(
                                  0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-
                                  20, -20, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(
                                  20, -20, 0), Color.Blue);

            //Vert buffer
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(
                           VertexPositionColor), 3, BufferUsage.
                           WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
        }

		protected void SetupFullscreenToggleListener() {
			keyUpListener.Listen (Keys.F11, (key) => {
                UseFullscreen = !UseFullscreen;

                if (UseFullscreen)
                {
                    lastScreenWidthHeight.X = graphics.PreferredBackBufferWidth;
                    lastScreenWidthHeight.Y = graphics.PreferredBackBufferHeight;

                    graphics.PreferredBackBufferWidth = ScreenBounds().X;
                    graphics.PreferredBackBufferHeight = ScreenBounds().Y;

                    graphics.IsFullScreen = true;
                } else
                {
                    graphics.PreferredBackBufferWidth = lastScreenWidthHeight.X;
                    graphics.PreferredBackBufferHeight = lastScreenWidthHeight.Y;

                    graphics.IsFullScreen = false;
                }

				graphics.ApplyChanges();
			});
		}

		protected void SetupExitListener() {
			keyUpListener.Listen (Keys.Escape, (key) => {
				Exit();
			});
		}

		protected void SetupDebugToggleListener() {
			keyUpListener.Listen (Keys.F8, (key) => {
				ToggleDebug();
			});
		}

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
		{
			keyUpListener.Update();
            debug.Update(gameTime);

            Update3d(gameTime);

            base.Update(gameTime);
        }

        protected void Update3d(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X -= 1f;
                camTarget.X -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X += 1f;
                camTarget.X += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 1f;
                camTarget.Y -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y += 1f;
                camTarget.Y += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 1f;
            }
            
            Matrix rotationMatrix = Matrix.CreateRotationY(
                                    MathHelper.ToRadians(1f));
            camPosition = Vector3.Transform(camPosition,
                          rotationMatrix);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            Draw3d(gameTime);
                        
			debug.Draw(graphics.GraphicsDevice, gameTime);

            DrawMouse();

            base.Draw(gameTime);
        }

        protected void DrawMouse()
        {
            var x = Mouse.GetState().X;
            var y = Mouse.GetState().Y;
            
            SpriteBatch spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            spriteBatch.Begin();
            spriteBatch.Draw(Content.Load<Texture2D>("Mouse"), new Rectangle(x, y, 32, 32), Color.White);
            spriteBatch.End();
        }

        protected void Draw3d(GameTime gametime)
        {
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.
                                              TriangleList, 0, 3);
            }
        }

        protected IntVector2 ScreenBounds()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;

            return new IntVector2() { X = screenWidth, Y = screenHeight };
        }
    }

    public struct IntVector2
    {
        public int X;
        public int Y;
    }
}
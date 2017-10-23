using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestMonoGame
{
    public class TestGame : Game
    {
		protected Debug debug;

		protected KeyUpListener keyUpListener;

        protected GraphicsDeviceManager graphics;
        
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
                        
            Content.RootDirectory = "Content";
        }

		protected void ToggleDebug() {
			debug.Display = !debug.Display;
		}

        protected override void Initialize()
        {
            base.Initialize();

			keyUpListener = new KeyUpListener ();

			SetupFullscreenToggleListener ();
			SetupExitListener ();

			debug = new Debug ();
        }

		protected void SetupFullscreenToggleListener() {
			keyUpListener.Listen (Keys.F11, (key) => {
				graphics.ToggleFullScreen();
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
            base.Update(gameTime);

			keyUpListener.Update();
			debug.Update (gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            base.Draw(gameTime);

			debug.Draw();
        }
    }
}
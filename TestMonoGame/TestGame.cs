using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestMonoGame
{
    public class TestGame : Game
    {
		protected bool UseFullScreen = false;

		protected KeyUpListener keyUpListener;

        protected GraphicsDeviceManager graphics;
        
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
                        
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

			keyUpListener = new KeyUpListener ();

			SetupFullscreenToggleListener ();
			SetupExitListener ();

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

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
		{
            base.Update(gameTime);

			keyUpListener.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
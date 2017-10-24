using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestMonoGame
{
	public class Debug
	{
		public bool Display = false;

		protected double LastSeconds = 0;
		protected int LastFrames = 0;
		protected int Fps = 0;

        protected SpriteFont debugFont;
        protected GraphicsDeviceManager graphics;

		public Debug (SpriteFont debugFont, GraphicsDeviceManager graphics)
		{
            this.debugFont = debugFont;
            this.graphics = graphics;
        }

		public void Update(GameTime gameTime) {
			UpdateFps (gameTime);
		}

		public void UpdateFps(GameTime gameTime) {
			LastFrames++;

			LastSeconds += gameTime.ElapsedGameTime.TotalSeconds;

			if ((LastSeconds) >= 1) {
				Fps = LastFrames;

				LastFrames = 0;
				LastSeconds = 0;
			}
		}

		public void Draw(GraphicsDevice graphicsDevice, GameTime gameTime) {
			if (Display) {
				DrawFps(graphicsDevice, gameTime);
			}
		}

		protected void DrawFps(GraphicsDevice graphics, GameTime gameTime) {

            var drawColor = Color.White;

            var width = this.graphics.PreferredBackBufferWidth;
            var height = this.graphics.PreferredBackBufferHeight;

            SpriteBatch spriteBatch = new SpriteBatch(graphics);

            spriteBatch.Begin();

            var fpsString = "FPS: " + Fps;
            var resolutionString = "Resolution: " + width.ToString() + "," + height.ToString();

            var fpsWidthHeight = debugFont.MeasureString(fpsString);
            var resWidthHeight = debugFont.MeasureString(resolutionString);

            var offset = 20;

            spriteBatch.DrawString(
                debugFont, 
                fpsString, 
                new Vector2(width - offset - fpsWidthHeight.X, height - offset - fpsWidthHeight.Y),
                drawColor
            );

            spriteBatch.DrawString(
                debugFont, 
                resolutionString, 
                new Vector2(width - offset - resWidthHeight.X, height - offset - resWidthHeight.Y - fpsWidthHeight.Y), 
                drawColor
            );

            spriteBatch.End();

        }
    }
}


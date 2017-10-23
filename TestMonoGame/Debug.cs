using System;
using Microsoft.Xna.Framework;

namespace TestMonoGame
{
	public class Debug
	{
		public bool Display = false;

		protected double LastSeconds = 0;
		protected int LastFrames = 0;
		protected int Fps = 0;

		public Debug ()
		{
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

		public void Draw() {
			if (Display) {
				DrawFps ();
			}
		}

		protected void DrawFps() {
		}
	}
}


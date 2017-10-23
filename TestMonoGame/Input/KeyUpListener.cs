using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TestMonoGame
{
	public class KeyUpListener
	{
		protected Dictionary<Keys, List<Action<Keys>>> listeners;

		protected List<Keys> pressed;

		public KeyUpListener ()
		{
			listeners = new Dictionary<Keys, List<Action<Keys>>>();
			pressed = new List<Keys> ();
		}

		public void Listen(Keys key, Action<Keys> listener) {
			if (!listeners.Keys.Contains (key)) {
				listeners [key] = new List<Action<Keys>> ();
			}

			listeners[key].Add (listener);
		}

		public void Update() {
			List<Keys> currentlyPressed = Keyboard.GetState ().GetPressedKeys().ToList();

			foreach (var key in pressed) {
				if (!currentlyPressed.Contains (key)) {
					Emit (key);
				}
			}

			pressed = currentlyPressed;
		}

		protected void Emit(Keys key) {
			if (!listeners.Keys.Contains (key)) {
				return;
			}

			foreach(var listener in listeners[key]) {
				listener (key);
			}
		}
	}
}


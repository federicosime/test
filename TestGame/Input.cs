using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
	class Input
	{
		public bool Up { get => Keyboard.GetState().IsKeyDown(UpKey); }
		public bool Down { get => Keyboard.GetState().IsKeyDown(DownKey); }
		public bool Left { get => Keyboard.GetState().IsKeyDown(LeftKey); }
		public bool Right { get => Keyboard.GetState().IsKeyDown(RightKey); }
		public Keys[] PressedKeys { get => Keyboard.GetState().GetPressedKeys(); }

		private Keys UpKey { get; set; }
		private Keys DownKey { get; set; }
		private Keys LeftKey { get; set; }
		private Keys RightKey { get; set; }

		public Input()
		{
			UpKey = Keys.None;
			DownKey = Keys.None;
			LeftKey = Keys.None;
			RightKey = Keys.None;
		}

		public Input(Keys up, Keys down, Keys left, Keys right)
		{
			UpKey = up;
			DownKey = down;
			LeftKey = left;
			RightKey = right;
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame
{
	enum Direction
	{
		Forward,
		Backward,
		None,
	}

	class Sprite
	{
		private Texture2D texture;
		private float rotation;
		private Direction lastDirection = Direction.None;

		public Vector2 Position;
		public Vector2 Origin;

		public float ActualLinearVelocity = 0.0f;
		public float MaxLinearVelocity = 8f;
		public float LinearAcceleration = 0.1f;
		public float LinearDeceleration = 0.5f;

		public float ActualRotationVelocity = 0f;
		public float MaxRotationVelocity = 5f;
		public float RotationAcceleration = 0.1f;
		public float RotationDeceleration = 0.5f;

		public float Scale = 1f;

		public Sprite(Texture2D texture)
		{
			this.texture = texture;
		}

		public void Update()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Up))
			{
				StopMovement(Direction.Backward);

				ActualLinearVelocity = IncreaseValue(ActualLinearVelocity, LinearAcceleration, MaxLinearVelocity);
				ActualRotationVelocity = IncreaseValue(ActualRotationVelocity, RotationAcceleration, MaxRotationVelocity);

				Move(Direction.Forward);
				TryRotation();
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.Down))
			{
				StopMovement(Direction.Forward);

				ActualLinearVelocity = IncreaseValue(ActualLinearVelocity, LinearAcceleration, MaxLinearVelocity);
				ActualRotationVelocity = IncreaseValue(ActualRotationVelocity, RotationAcceleration, MaxRotationVelocity);

				Move(Direction.Backward);
				TryRotation();
			}
			else if (!IsUpOrDownKeyPressed())
			{
				//ResetState();
				ActualLinearVelocity = DecreaseValue(ActualLinearVelocity, LinearDeceleration, 0);
				ActualRotationVelocity = DecreaseValue(ActualRotationVelocity, RotationDeceleration, 0);
				
				Move(lastDirection);
				Debug.WriteLine(TryRotation());
			}
		}

		private void StopMovement(Direction direction)
		{
			if (lastDirection == direction)
			{
				ResetState();
			}
		}

		private void Move(Direction direction)
		{
			Vector2 directionVector = new Vector2(
				(float)Math.Sin(MathHelper.ToRadians(0) - rotation), 
				(float)Math.Cos(MathHelper.ToRadians(0) - rotation));

			lastDirection = direction;

			if (direction == Direction.Forward)
			{
				Position -= directionVector * ActualLinearVelocity;
			}
			if (direction == Direction.Backward)
			{
				Position += directionVector * ActualLinearVelocity;
			}
		}

		private void ResetState()
		{
			ActualLinearVelocity = 0;
			ActualRotationVelocity = 0;
		}

		private float IncreaseValue(float startValue, float increaseFactor, float limit)
		{
			if (startValue < limit)
			{
				startValue += increaseFactor;
			}

			return startValue;
		}

		private float DecreaseValue(float startValue, float decreaseFactor, float limit)
		{
			if (startValue > limit)
			{
				startValue -= decreaseFactor;
			}
			else
			{
				startValue = limit;
			}

			return startValue;
		}

		private bool IsUpOrDownKeyPressed()
		{
			return Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Down);
		}

		private static bool IsAnyKeyPressed()
		{
			return Keyboard.GetState().GetPressedKeys().Length > 0;
		}

		private bool TryRotation()
		{
			bool rotated = false;

			if (Keyboard.GetState().IsKeyDown(Keys.Left))
			{
				rotation -= MathHelper.ToRadians(ActualRotationVelocity);
				rotated = true;
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Right))
			{
				rotation += MathHelper.ToRadians(ActualRotationVelocity);
				rotated = true;
			}

			return rotated;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, Position, null, Color.White, rotation, Origin, Scale, SpriteEffects.None, 0);	
		}
	}
}

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
		private float Rotation { get; set; }
		private Direction LastDirection { get; set; }

		public Texture2D Texture { get; private set; }
		public Input Input { get; set; }

		public Vector2 Position { get; set; }
		public Vector2 Origin { get; set; }

		public float ActualLinearVelocity { get; set; }
		public float MaxLinearVelocity { get; set; }
		public float LinearAcceleration { get; set; }
		public float LinearDeceleration { get; set; } 

		public float ActualRotationVelocity { get; set; }
		public float MaxRotationVelocity { get; set; }
		public float RotationAcceleration { get; set; }
		public float RotationDeceleration { get; set; }

		public float Scale { get; set; }

		private Sprite()
		{
			LastDirection = Direction.None;

			Input = new Input(Keys.Up, Keys.Down, Keys.Left, Keys.Right);

			Position = new Vector2(0, 0);
			Origin = new Vector2(0, 0);

			ActualLinearVelocity = 0.0f;
			MaxLinearVelocity = 8.0f;
			LinearAcceleration = 0.1f;
			LinearDeceleration = 0.5f;

			ActualRotationVelocity = 0.0f;
			MaxRotationVelocity = 5.0f;
			RotationAcceleration = 0.1f;
			RotationDeceleration = 0.5f;

			Scale = 1.0f;
		}

		public Sprite(Texture2D texture) : this()
		{
			this.Texture = texture;
		}

		public void Update()
		{
			if (Input.Up)
			{
				StopMovement(Direction.Backward);

				ActualLinearVelocity = IncreaseValue(ActualLinearVelocity, LinearAcceleration, MaxLinearVelocity);
				ActualRotationVelocity = IncreaseValue(ActualRotationVelocity, RotationAcceleration, MaxRotationVelocity);

				Move(Direction.Forward);
				TryRotation();
			}
			else if (Input.Down)
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
				
				Move(LastDirection);
				Debug.WriteLine(TryRotation());
			}
		}

		private void StopMovement(Direction direction)
		{
			if (LastDirection == direction)
			{
				ResetState();
			}
		}

		private void Move(Direction direction)
		{
			Vector2 directionVector = new Vector2(
				(float)Math.Sin(MathHelper.ToRadians(0) - Rotation), 
				(float)Math.Cos(MathHelper.ToRadians(0) - Rotation));

			LastDirection = direction;

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
			return Input.Up || Input.Down;
		}

		private bool IsAnyKeyPressed()
		{
			return Input.PressedKeys.Length > 0;
		}

		private bool TryRotation()
		{
			bool rotated = false;

			if (Input.Left)
			{
				Rotation -= MathHelper.ToRadians(ActualRotationVelocity);
				rotated = true;
			}
			if (Input.Right)
			{
				Rotation += MathHelper.ToRadians(ActualRotationVelocity);
				rotated = true;
			}

			return rotated;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);	
		}
	}
}

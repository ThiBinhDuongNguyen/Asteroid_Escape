using Microsoft.Xna.Framework.Input;
using System;
using System.Numerics;

namespace Assignment3SpaceGame
{
	public class Ship
	{
		public Vector2 Position { get; private set; } = new Vector2(100, 100);
		private int Speed { get; } = 2;
		private int BoostSpeed { get; } = 10; // Boost speed
		private int _radius;

		public Ship()
		{
			_radius = 30; // Example radius, modify based on the size of your ship sprite
		}

		public void SetRadius(int radius)
		{
			_radius = radius;
		}

		public int GetRadius()
		{
			return _radius;
		}

		public void Update()
		{
			KeyboardState state = Keyboard.GetState();

			int speed = Speed;
			if (state.IsKeyDown(Keys.Enter) && state.IsKeyDown(Keys.Right)) // Boost speed
			{
				speed = BoostSpeed;
			}

			if (state.IsKeyDown(Keys.Left))
				Position = new Vector2(Position.X - speed, Position.Y);

			if (state.IsKeyDown(Keys.Right))
				Position = new Vector2(Position.X + speed, Position.Y);

			if (state.IsKeyDown(Keys.Up))
				Position = new Vector2(Position.X, Position.Y - speed);

			if (state.IsKeyDown(Keys.Down))
				Position = new Vector2(Position.X, Position.Y + speed);
		}
	}

}

using System;
using Microsoft.Xna.Framework;


namespace Assignment3SpaceGame
{
	public class Asteroid
	{
		public Vector2 Position { get; set; }
		public int Speed { get; set; }
		public static int Radius { get; set; } = 20; // Example value
		public bool HasPassed { get; set; } = false; // Flag to track if asteroid has passed the ship

		public Asteroid(int speed, Vector2 position)
		{
			Speed = speed;
			Position = position;
		}

		public void Update()
		{
			// Move the asteroid to the left
			Position = new Vector2(Position.X - Speed, Position.Y);
		}
	}


}

using Assignment3SpaceGame;
using Microsoft.Xna.Framework;
using System;

namespace Assignment3SpaceGame
{
	public class Controller
	{
		private TimeSpan _elapsedTime;
		private int _secondsElapsed;

		public Controller()
		{
			_elapsedTime = TimeSpan.Zero;
			_secondsElapsed = 0;
		}

		public int UpdateTime(GameTime gameTime)
		{
			_elapsedTime += gameTime.ElapsedGameTime;
			if (_elapsedTime.TotalSeconds >= 1)
			{
				_secondsElapsed++;
				_elapsedTime = TimeSpan.Zero;
			}
			return _secondsElapsed;
		}

		public bool CheckCollision(Ship player, Asteroid asteroid)
		{
			int playerRadius = player.GetRadius();
			int asteroidRadius = Asteroid.Radius;
			float distance = Vector2.Distance(player.Position, asteroid.Position);

			// Return true if the distance between the centers is less than the sum of the radii
			return distance < (playerRadius + asteroidRadius);
		}

	}
}


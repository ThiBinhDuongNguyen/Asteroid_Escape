using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Assignment3SpaceGame
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Texture2D shipSprite;
		private Texture2D asteroidSprite;
		private Texture2D spaceSprite;
		private SpriteFont gameFont;
		private SpriteFont timerFont;

		private Ship player = new Ship();
		private List<Asteroid> asteroids = new List<Asteroid>();
		private Random rand = new Random();

		private TimeSpan elapsedTime;
		private int secondsElapsed;
		private int score;
		private bool inGame = true;
		private bool gameWon = false;

		private Controller controller = new Controller();

		private SoundEffect backgroundMusic;
		private SoundEffectInstance backgroundMusicInstance;
		private SoundEffect collisionSound;
		private SoundEffect gameOverSound;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_graphics.PreferredBackBufferWidth = 1200;
			_graphics.PreferredBackBufferHeight = 900;
			_graphics.ApplyChanges();

			elapsedTime = TimeSpan.Zero;
			secondsElapsed = 0;
			score = 0;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			shipSprite = Content.Load<Texture2D>("ship");
			asteroidSprite = Content.Load<Texture2D>("asteroid");
			spaceSprite = Content.Load<Texture2D>("space");
			gameFont = Content.Load<SpriteFont>("spaceFont");
			timerFont = Content.Load<SpriteFont>("timerFont");

			// Load sound effects
			backgroundMusic = Content.Load<SoundEffect>("backgroundMusic");
			backgroundMusicInstance = backgroundMusic.CreateInstance();
			collisionSound = Content.Load<SoundEffect>("collisionSound");
			gameOverSound = Content.Load<SoundEffect>("gameOverSound");

			// Play background music
			backgroundMusicInstance.IsLooped = true;
			backgroundMusicInstance.Play();
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (!inGame) return;

			player.Update();
			UpdateAsteroids();
			secondsElapsed = (int)gameTime.TotalGameTime.TotalSeconds;

			// Spawn asteroids at regular intervals
			if (secondsElapsed % 2 == 0 && elapsedTime.Seconds != secondsElapsed)
			{
				SpawnAsteroid();
				elapsedTime = TimeSpan.FromSeconds(secondsElapsed);
			}

			// Check for collisions and update score accordingly
			for (int i = asteroids.Count - 1; i >= 0; i--)
			{
				// Check for collisions between the ship and asteroids
				if (controller.CheckCollision(player, asteroids[i]))
				{
					collisionSound.Play();

					// Decrease the score by 3 when a collision occurs (ensure score doesn't go below 0)
					score = Math.Max(0, score - 3);

					asteroids.RemoveAt(i);

					// If score reaches 0, end the game
					if (score == 0)
					{
						Console.WriteLine("Game Over! Score is 0.");  // Debugging line
						inGame = false;
						gameWon = false;
						gameOverSound.Play();
						break;
					}
				}
				// If no collision, check if the asteroid passes the spaceship
				else if (asteroids[i].Position.X < 0) // Asteroid has passed off the left side of the screen
				{
					score += 1; // Increase score for passing asteroid
					asteroids.RemoveAt(i);
				}
				// Check if the asteroid has passed the spaceship horizontally (without colliding)
				else if (asteroids[i].Position.X > player.Position.X + shipSprite.Width / 2 && !asteroids[i].HasPassed)
				{
					score += 1; // Increase score for passing asteroid
					asteroids[i].HasPassed = true; // Mark the asteroid as passed
				}
			}

			// Stop background music if game is over
			if (!inGame)
			{
				backgroundMusicInstance.Stop();
			}

			// Check for game win condition
			if (secondsElapsed >= 15 && inGame)
			{
				inGame = false;
				gameWon = true;
				backgroundMusicInstance.Stop();
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();
			_spriteBatch.Draw(spaceSprite, new Vector2(0, 0), Color.White);

			_spriteBatch.Draw(shipSprite, new Vector2(player.Position.X - shipSprite.Width / 2, player.Position.Y - shipSprite.Height / 2), Color.White);

			foreach (var asteroid in asteroids)
			{
				_spriteBatch.Draw(asteroidSprite, new Vector2(asteroid.Position.X - Asteroid.Radius, asteroid.Position.Y - Asteroid.Radius), Color.White);
			}

			_spriteBatch.DrawString(timerFont, $"Time: {Math.Min(secondsElapsed, 15)}/15", new Vector2(10, 10), Color.White);
			_spriteBatch.DrawString(gameFont, $"Score: {score}", new Vector2(10, 40), Color.White);

			if (!inGame)
			{
				string message = gameWon ? $"Game Won! You surpassed {score} asteroids" : "Game Lost! Spaceship hit by Asteroid";
				_spriteBatch.DrawString(gameFont, message, new Vector2(300, 400), Color.White);
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private void SpawnAsteroid()
		{
			int speed = rand.Next(1, 5);
			int yPosition = rand.Next(0, _graphics.PreferredBackBufferHeight - Asteroid.Radius * 2);
			asteroids.Add(new Asteroid(speed, new Vector2(_graphics.PreferredBackBufferWidth, yPosition)));
		}

		private void UpdateAsteroids()
		{
			foreach (var asteroid in asteroids)
			{
				asteroid.Update();
			}
		}
	}
}

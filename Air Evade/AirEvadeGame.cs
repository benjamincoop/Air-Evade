using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Air_Evade
{
    public class AirEvadeGame : Game
    {
        #region Local vars
        /// <summary>
        /// The graphics device manager instance
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The SpriteBatch instance
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The two sprites that make up the infinite scrolling background
        /// </summary>
        private Background[] background;

        /// <summary>
        /// The player object instance
        /// </summary>
        private Player player;

        /// <summary>
        /// The collection of missiles in the game
        /// </summary>
        private List<Missile> missiles;

        /// <summary>
        /// The minimum number of missiles that must be present
        /// </summary>
        private int difficulty = 1;

        /// <summary>
        /// Instance of the Random class for randomly generated randomness
        /// </summary>
        private Random random;

        /// <summary>
        /// InputManager instance for managing input
        /// </summary>
        private InputManager inputManager;

        /// <summary>
        /// The music and music instance
        /// </summary>
        private SoundEffect gameMusic;
        private SoundEffectInstance gameMusicInstance;

        /// <summary>
        /// The font for displaying the player score
        /// </summary>
        private SpriteFont gameFont;

        /// <summary>
        /// Indicates that the game is fully over and awaiting restart
        /// </summary>
        private bool isGameOver = false;

        /// <summary>
        /// The player's current score
        /// </summary>
        private int score = 0;

        /// <summary>
        /// The highest score a player has gotten on any run
        /// </summary>
        private int highScore = 0;
        #endregion

        public AirEvadeGame()
        {
            // Set up graphics device and render window
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 750
            };
            Window.Title = "Air Evade";

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // Create the scrolling background
            background = new Background[] {
                new Background(this, new Vector2(0,0)),
                new Background(this, new Vector2(graphics.GraphicsDevice.Viewport.Width,0))
            };

            // Create player
            player = new Player(this, new Vector2(25, graphics.GraphicsDevice.Viewport.Height / 2));

            // Create random instance
            random = new Random();

            // Initialize missiles collection
            missiles = new List<Missile>();

            // Initialize input manager
            inputManager = new InputManager();

            base.Initialize();

            gameMusicInstance.IsLooped = true;
            gameMusicInstance.Volume = 1;
            gameMusicInstance.Play();
        }

        /// <summary>
        /// Load in static sprite assets and music
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameFont = Content.Load<SpriteFont>("VT323");
            gameMusic = Content.Load<SoundEffect>("music");
            gameMusicInstance = gameMusic.CreateInstance();

            foreach (Background bg in background) bg.LoadContent();
            player.LoadContent();
        }

        /// <summary>
        /// Triggers a soft reset of the game
        /// </summary>
        private void Restart()
        {
            isGameOver = false;
            difficulty = 1;
            score = 0;
            Initialize();
        }

        /// <summary>
        /// Main game logic loop, called once per frame
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {

            // Get latest input from manager
            inputManager.Update(gameTime);

            if (inputManager.Exit)
            {
                Exit();
            } else
            {
                // Update player position
                player.Move(inputManager.Direction);

                // Iterating through list of active missiles
                for (int i = 0; i < missiles.Count; i++)
                {
                    // If missile is active, do stuff with it
                    if (missiles[i].Active)
                    {
                        // Update missile position
                        missiles[i].Move();

                        // If a missile flies offscreen, disable it and increase score
                        if(missiles[i].Position.X < 0)
                        {
                            missiles[i].Active = false;
                            IncrementScore();
                        } else
                        {
                            // Check for collisions and handle Game Over events
                            if (missiles[i].CollisionBox.CollidesWith(player.CollisionBox))
                            {
                                missiles[i].Detonate();
                                GameOver();
                            }
                        }  
                    }
                    // If missile is inactive, remove it from the list
                    else
                    {
                        missiles.RemoveAt(i);
                    }
                }
                // If there aren't enough missiles on screen, spawn another each frame until there are
                if (missiles.Count < difficulty) SpawnMissile();

                base.Update(gameTime);

                // Handle fade out of game music
                if(player.State == Player.PlayerState.DEAD)
                {
                    if (gameMusicInstance.Volume > 0.009f)
                    {
                        gameMusicInstance.Volume -= 0.009f;
                    } else
                    {
                        gameMusicInstance.Stop();
                        isGameOver = true;
                    }
                }

                // Restart game upon input from player
                if(isGameOver && inputManager.Restarting) Restart();
            }
        }

        /// <summary>
        /// Increments player score by one and increases difficulty if score is at a certain threshold
        /// </summary>
        private void IncrementScore()
        {
            if(player.State != Player.PlayerState.DEAD)
            {
                score++;
                switch (score)
                {
                    case 10:
                        difficulty++;
                        break;
                    case 30:
                        difficulty++;
                        break;
                    case 60:
                        difficulty++;
                        break;
                    case 100:
                        difficulty++;
                        break;
                    case 150:
                        difficulty++;
                        break;
                    case 210:
                        difficulty++;
                        break;
                    case 280:
                        difficulty++;
                        break;
                    case 360:
                        difficulty++;
                        break;
                    case 450:
                        difficulty++;
                        break;
                }
            
            }
        }

        /// <summary>
        /// Handles the transition to a game over state
        /// </summary>
        private void GameOver()
        {
            player.State = Player.PlayerState.DEAD;
            difficulty = 0;
            if (score > highScore) highScore = score;
            foreach (Background bg in background) bg.Speed = 0;
        }

        /// <summary>
        /// Main game drawing loop, called once per frame
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (Background bg in background) bg.Draw(spriteBatch);

            player.Draw(spriteBatch);

            foreach (Missile missile in missiles) missile.Draw(spriteBatch);

            spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(gameFont, "High Score: " + highScore.ToString(), new Vector2(
                (graphics.GraphicsDevice.Viewport.Width) - (gameFont.MeasureString("High Score: " + highScore.ToString()).X + 5),
                5), Color.Black);

            // Show restart prompt in the center of the screen upon defeat
            if (isGameOver)
            {
                string promptStr;
                if(inputManager.PreferGamePad)
                {
                    promptStr = "Press 'start' to play again.";
                } else
                {
                    promptStr = "Press 'enter' to play again.";
                }
                Vector2 strSize = gameFont.MeasureString(promptStr);
                spriteBatch.DrawString(gameFont, promptStr, new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - (strSize.X / 2),
                    (graphics.GraphicsDevice.Viewport.Height / 2) - (strSize.Y / 2)), Color.DarkRed);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Spawns a missile with a random velocity and Y-axis position
        /// </summary>
        private void SpawnMissile()
        {
            missiles.Add(new Missile(this, new Vector2(
                    graphics.GraphicsDevice.Viewport.Width, random.Next(0, graphics.GraphicsDevice.Viewport.Height)), random.Next(20, 30), 0.06f));
        }
    }
}

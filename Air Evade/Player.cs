using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Air_Evade
{
    class Player : Sprite
    {
        #region Local vars

        /// <summary>
        /// The textures to use for the player when idle/flying
        /// </summary>
        Texture2D[] flyTexture;

        /// <summary>
        /// The textures to use for the player when firing weapon
        /// </summary>
        Texture2D[] shootTexture;

        /// <summary>
        /// An index into the Texture2D arrays that gives the current sprite texture to draw 
        /// </summary>
        int animIndex = 0;

        /// <summary>
        /// The number of game frames between each player animation frame
        /// </summary>
        readonly int animDelay = 3;

        /// <summary>
        /// The number of game frames drawn since last player animation frame
        /// </summary>
        int animTimer = 0;

        /// <summary>
        /// The speed of the player's movement in pixels-per-second
        /// </summary>
        readonly int speed = 250;
        #endregion

        #region Public properties
        /// <summary>
        /// The current state of the player instance
        /// </summary>
        public enum PlayerState { IDLE, SHOOTING, DEAD }
        public PlayerState State { get; set; } = PlayerState.IDLE;
        #endregion

        /// <summary>
        /// Constructs a new class instance
        /// </summary>
        /// <param name="game">The game this ball belongs in</param>
        /// <param name="color">A color to distinguish this ball</param>
        public Player(Game game, Vector2 position)
        {
            BaseGame = game;
            Position = position;
            ScaleFactor = 0.25f;
        }

        /// <summary>
        /// Loads the player textures and bounding box
        /// </summary>
        public void LoadContent()
        {
            flyTexture = new Texture2D[]
            {
                BaseGame.Content.Load<Texture2D>("fly1"),
                BaseGame.Content.Load<Texture2D>("fly2")
            };
            shootTexture = new Texture2D[]
            {
                BaseGame.Content.Load<Texture2D>("shoot1"),
                BaseGame.Content.Load<Texture2D>("shoot2"),
                BaseGame.Content.Load<Texture2D>("shoot3"),
                BaseGame.Content.Load<Texture2D>("shoot4"),
                BaseGame.Content.Load<Texture2D>("shoot5")
            };
            BaseTexture = BaseGame.Content.Load<Texture2D>("dead");

            Size = new Vector2(BaseTexture.Width * ScaleFactor, BaseTexture.Height * ScaleFactor);
            CollisionBox = new CollisionHelper.BoundingRectangle(Position, Size * 0.5f);
        }

        /// <summary>
        /// Controls the motion of the player sprite based on recived inputs
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector2 direction)
        {
            // Override player controls if plane is destroyed
            if (State == PlayerState.DEAD)
            {
                Rotation = 0.5f;
                Position += new Vector2(0, (float)BaseGame.TargetElapsedTime.TotalSeconds * speed);
            } else
            // Update position and rotation based on input
            {
                if (direction.Y > 0) // player going downwards
                {
                    Rotation = 0.1f;
                }
                else
                {
                    if (direction.Y < 0) // player going upwards
                    {
                        Rotation = -0.1f;
                    }
                    else
                    {
                        Rotation = 0f;
                    }
                }
                Position += direction * speed;
            }

            // Enforce screen boundries
            if (Position.X < 0) Position = new Vector2(1, Position.Y);
            if (Position.X + Size.X > BaseGame.GraphicsDevice.Viewport.Width) Position = new Vector2(BaseGame.GraphicsDevice.Viewport.Width - (Size.X + 1), Position.Y);
            if (Position.Y < 0) Position = new Vector2(Position.X, 1);
            if (Position.Y + Size.Y > BaseGame.GraphicsDevice.Viewport.Height) Position = new Vector2(Position.X, BaseGame.GraphicsDevice.Viewport.Height - (Size.Y + 1));

            // Update location of CollisionBox to the center of sprite
            CollisionBox.Position = new Vector2(Position.X + (Size.X - CollisionBox.Size.X) / 2, Position.Y + (Size.Y - CollisionBox.Size.Y) / 2);
        }

        /// <summary>
        /// Returns the correct Texture2D to be drawn this frame, based on the PlayerState and animation timings
        /// </summary>
        /// <returns></returns>
        private Texture2D Animate()
        {
            if(animTimer >= animDelay)
            {
                animIndex++;
                animTimer = 0;
            } else
            {
                animTimer++;
            }

            switch (State)
            {
                case PlayerState.IDLE:
                    if (animIndex > 1)
                    {
                        animIndex = 0;
                    }
                    return flyTexture[animIndex];
                case PlayerState.SHOOTING:
                    if (animIndex > 4) animIndex = 0;
                    return shootTexture[animIndex];
                case PlayerState.DEAD:
                    return BaseTexture;
                default:
                    throw new Exception("Invalid PlayerState.");
            }
        }

        /// <summary>
        /// Draws the player sprite at its current position
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Animate();

            if (texture is null)
            {
                throw new InvalidOperationException("Player texture unloaded.");
            }
            else
            {
                spriteBatch.Draw(texture, Position, null, ShadingColor, Rotation, Vector2.Zero, ScaleFactor, SpriteEffects.None, 0f);
            }

        }
    }
}

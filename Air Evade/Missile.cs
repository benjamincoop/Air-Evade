using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Air_Evade
{
    class Missile : Sprite
    {
        #region Local vars
        /// <summary>
        /// The Y-axis postion that the missile's trajectory is centered on
        /// </summary>
        readonly int origin;

        /// <summary>
        /// The speed at which the missile sways vertically
        /// </summary>
        readonly int frequency;

        /// <summary>
        /// The amount of vertical sway in the missile's trajectory
        /// </summary>
        readonly int amplitude = 10;

        /// <summary>
        /// An index into the explosionTexture Texture2D array 
        /// </summary>
        int explosionAnimIndex = 0;

        /// <summary>
        /// The number of game frames between each animation frame
        /// </summary>
        readonly int explosionAnimDelay = 5;

        /// <summary>
        /// The number of game frames drawn since last animation frame
        /// </summary>
        int explosionAnimTimer = 0;

        /// <summary>
        /// If true, indicates that the missile is in the process of exploding
        /// </summary>
        bool detonating = false;

        /// <summary>
        /// The sound that plays when this missiles detonates
        /// </summary>
        SoundEffect explosionSound;

        /// <summary>
        /// The textures for an exploded missile
        /// </summary>
        Texture2D[] explosionTexture;
        #endregion

        #region Public properties

        /// <summary>
        /// Indicates if the missile is in active use. Inactive missiles should be ignored and garbage collected
        /// </summary>
        public bool Active { get; private set; } = true;

        public int Speed { get; set; }
        #endregion

        public Missile(Game game, Vector2 position, int speed, float rotation)
        {
            BaseGame = game;
            Position = position;
            ScaleFactor = 0.25f;
            origin = (int)position.Y;
            Speed = speed;
            frequency = speed / 8;
            Rotation = rotation;
            LoadContent();
        }

        /// <summary>
        /// Loads the missile textures, sounds, and bounding box
        /// </summary>
        private void LoadContent()
        {
            BaseTexture = BaseGame.Content.Load<Texture2D>("missile");
            explosionTexture = new Texture2D[]
            {
                BaseGame.Content.Load<Texture2D>("explosion0"),
                BaseGame.Content.Load<Texture2D>("explosion1"),
                BaseGame.Content.Load<Texture2D>("explosion2"),
                BaseGame.Content.Load<Texture2D>("explosion3"),
                BaseGame.Content.Load<Texture2D>("explosion4"),
                BaseGame.Content.Load<Texture2D>("explosion5"),
                BaseGame.Content.Load<Texture2D>("explosion6"),
                BaseGame.Content.Load<Texture2D>("explosion7"),
                BaseGame.Content.Load<Texture2D>("explosion8")
            };

            explosionSound = BaseGame.Content.Load<SoundEffect>("explode");
            
            Size = new Vector2(BaseTexture.Width * ScaleFactor, BaseTexture.Height * ScaleFactor);
            CollisionBox = new CollisionHelper.BoundingRectangle(Position, Size * 0.75f);
        }

        /// <summary>
        /// Controls the motion and explosion animation of the missile
        /// </summary>
        public void Move()
        {
            if(detonating)
            {
                // When animation finishes, deactivate and hide missile
                if(explosionAnimIndex >= 8)
                {
                    Active = false;
                    BaseTexture = new Texture2D(BaseGame.GraphicsDevice, 1, 1);
                } else
                {
                    // Increment frame counter and animation frame index
                    if(explosionAnimTimer == explosionAnimDelay)
                    {
                        explosionAnimIndex++;
                        explosionAnimTimer = 0;
                        BaseTexture = explosionTexture[explosionAnimIndex];
                    }
                    explosionAnimTimer++;
                }
            } else
            {
                // Moves the missile vertically in an oscillating wave pattern
                if (Rotation > 0f) // missile pointing upwards
                {
                    if (Position.Y < origin - amplitude)
                    {
                        Rotation = -0.06f;
                        Position = new Vector2(Position.X - Speed, Position.Y + frequency);
                    }
                    else
                    {
                        Position = new Vector2(Position.X - Speed, Position.Y - frequency);
                    }
                }
                else // missile pointing downwards
                {
                    if (Position.Y > origin + amplitude)
                    {
                        Rotation = 0.06f;
                        Position = new Vector2(Position.X - Speed, Position.Y - frequency);
                    }
                    else
                    {
                        Position = new Vector2(Position.X - Speed, Position.Y + frequency);
                    }
                }

                // Deactivate missile if it flies offscreen, otherwise update attached collision box
                if (Position.X < -1)
                {
                    Active = false;
                    ((AirEvadeGame)BaseGame).UpdateScore(1);
                }
                else
                {
                    // Update location of CollisionBox to the center of sprite
                    CollisionBox.Position = new Vector2(Position.X + (Size.X - CollisionBox.Size.X) / 2, Position.Y + (Size.Y - CollisionBox.Size.Y) / 2);
                }
            }
        }

        /// <summary>
        /// Triggers the detonation of the missile
        /// </summary>
        public void Detonate()
        {
            if(detonating == false) explosionSound.Play();
            detonating = true;
            Speed = 0;
            Rotation = 0f;
            BaseTexture = explosionTexture[explosionAnimIndex];
        }

        /// <summary>
        /// Draws the missile sprite at its current position
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (BaseTexture is null)
            {
                throw new InvalidOperationException("Missile texture unloaded.");
            }
            else
            {
                spriteBatch.Draw(BaseTexture, Position, null, ShadingColor, Rotation, Vector2.Zero, ScaleFactor, SpriteEffects.None, 0f);
            }

        }
    }
}

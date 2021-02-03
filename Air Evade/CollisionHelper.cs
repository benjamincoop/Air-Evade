using System;
using Microsoft.Xna.Framework;

namespace Air_Evade
{
    /// <summary>
    /// Provides a framework for detecting collisions between sprites
    /// </summary>
    class CollisionHelper
    {
        /// <summary>
        /// Defines a rectangular-type collision box
        /// </summary>
        public class BoundingRectangle
        {
            /// <summary>
            /// The sprite that this bounding box is attached to
            /// </summary>
            private readonly Sprite sprite;

            /// <summary>
            /// The width and height of the bounding box, after scaling
            /// </summary>
            public Vector2 Size { get; private set; }

            /// <summary>
            /// The position of the bounding box
            /// </summary>
            public Vector2 Position { get; private set; } = new Vector2();

            /// <summary>
            /// Creates a BoundingRectangle around the given sprite
            /// </summary>
            /// <param name="sprite"></param>
            public BoundingRectangle(Sprite sprite)
            {
                this.sprite = sprite;
                Size = new Vector2(sprite.BaseTexture.Width * sprite.ScaleFactor, sprite.BaseTexture.Height * sprite.ScaleFactor);
                UpdatePosition();
            }

            /// <summary>
            /// Creates a BoundingRectangle with a scaling factor around the given sprite
            /// </summary>
            /// <param name="sprite"></param>
            /// <param name="offsets"></param>
            public BoundingRectangle(Sprite sprite, float rectScaleFactor)
            {
                this.sprite = sprite;
                Size = new Vector2((sprite.BaseTexture.Width * sprite.ScaleFactor) * rectScaleFactor, (sprite.BaseTexture.Height * sprite.ScaleFactor) * rectScaleFactor);
                UpdatePosition();
            }

            /// <summary>
            /// Sets the position of the collision box to the center of the sprite
            /// </summary>
            public void UpdatePosition()
            {
                Position = new Vector2(
                    sprite.Position.X + ((sprite.Size.X - Size.X) / 2),
                    sprite.Position.Y + ((sprite.Size.Y - Size.Y) / 2)
                );
            }

            /// <summary>
            /// Returns true if this BoundingRectangle is intersecting that of the given sprite
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            public bool CollidesWith(Sprite target)
            {
                if(
                    Position.X + Size.X < target.CollisionBox.Position.X
                    || Position.X > target.CollisionBox.Position.X + target.CollisionBox.Size.X
                    || Position.Y + Size.Y < target.CollisionBox.Position.Y
                    || Position.Y > target.CollisionBox.Position.Y + target.CollisionBox.Size.Y
                )
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }
    }
}

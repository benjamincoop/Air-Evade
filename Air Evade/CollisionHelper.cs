using System;
using Microsoft.Xna.Framework;

namespace Air_Evade
{
    /// <summary>
    /// Provides a framework for detecting collisions between sprites
    /// </summary>
    public class CollisionHelper
    {
        /// <summary>
        /// Defines a rectangular-type collision box
        /// </summary>
        public class BoundingRectangle
        {
            /// <summary>
            /// The width and height of the bounding box, after scaling
            /// </summary>
            public Vector2 Size { get; set; }

            /// <summary>
            /// The position of the bounding box
            /// </summary>
            public Vector2 Position { get; set; }

            /// <summary>
            /// Creates a BoundingRectangle around the given sprite
            /// </summary>
            /// <param name="sprite"></param>
            public BoundingRectangle(Vector2 position, Vector2 size)
            {
                Position = position;
                Size = size;
            }

            /// <summary>
            /// Detects if this BoundingRectangle is colliding with another
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            public bool CollidesWith(BoundingRectangle other)
            {
                return !(Position.X + Size.X < other.Position.X
                    || Position.X > other.Position.X + other.Size.X
                    || Position.Y + Size.Y < other.Position.Y
                    || Position.Y > other.Position.Y + other.Size.Y);
            }
        }
    }
}

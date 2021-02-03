using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Air_Evade
{
    /// <summary>
    /// Defines a base class that all sprites inherit
    /// </summary>
    class Sprite
    {
        /// <summary>
        /// The game this sprite is a part of
        /// </summary>
        public Game BaseGame { get; set; }

        /// <summary>
        /// The default bitmap texture for this sprite
        /// </summary>
        public Texture2D BaseTexture { get; set; }

        /// <summary>
        /// The scaling factor by which to increase or reduce sprite size
        /// </summary>
        public float ScaleFactor { get; set; }

        /// <summary>
        /// The degree of rotation of the sprite
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The color used to shade the sprite, usually white
        /// </summary>
        public Color ShadingColor { get; set; } = Color.White;

        /// <summary>
        /// The size of the sprite, after scaling
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The BoundingRectangle for collision detection
        /// </summary>
        public CollisionHelper.BoundingRectangle CollisionBox { get; set; }
    }
}

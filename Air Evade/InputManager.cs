using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Air_Evade
{
    class InputManager
    {
        #region Local vars
        // Holds input device states as of the previous frame
        KeyboardState priorKeyboardState;
        GamePadState priorGamePadState;

        // Holds current input device states as of this frame
        KeyboardState currentKeyboardState;
        GamePadState currentGamePadState;
        #endregion

        #region Public properties
        /// <summary>
        /// Vector giving current input direction
        /// </summary>
        public Vector2 Direction { get; private set; }

        /// <summary>
        /// If true, show gamepad controls, otherwise, show controls for keyboard
        /// </summary>
        public bool PreferGamePad { get; private set; }

        /// <summary>
        /// Triggers an Exit() call for the program when true
        /// </summary>
        public bool Exit { get; private set; } = false;

        /// <summary>
        /// Indicates that the player is firing their weapon when true
        /// </summary>
        public bool Shooting { get; private set; } = false;

        /// <summary>
        /// Indicates that the user is restarting the game
        /// </summary>
        public bool Restarting { get; private set; } = false;
        #endregion

        public void Update(GameTime gameTime)
        {
            #region Set states
            priorKeyboardState = currentKeyboardState;
            priorGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            #endregion

            #region Gamepad input
            Direction = new Vector2(currentGamePadState.ThumbSticks.Left.X, currentGamePadState.ThumbSticks.Left.Y * -1)
                * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check if player is using gamepad
            if(Direction != Vector2.Zero)
            {
                PreferGamePad = true;
            }
            #endregion

            #region Keyboard input
            // Check if player is using keyboard
            if(currentKeyboardState.GetPressedKeyCount() > 0)
            {
                PreferGamePad = false;
            }

            // Check input on WASD and arrow keys
            if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W))
            {
                Direction += new Vector2(0, -1 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A))
            {
                Direction += new Vector2(-1 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S))
            {
                Direction += new Vector2(0, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D))
            {
                Direction += new Vector2((float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            #endregion

            #region Exit detection
            if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit = true;
            }
            #endregion

            #region Restart detection
            if(currentKeyboardState.IsKeyDown(Keys.Enter) || currentGamePadState.IsButtonDown(Buttons.Start))
            {
                Restarting = true;
            } else
            {
                Restarting = false;
            }
            #endregion
        }
    }
}

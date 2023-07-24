using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Input
{
    public class KeyboardReader : IInputReader
    {
        public string AnimationState { get; set; }
        public bool IsJumping { get; set; }

        public float ReadInput()
        {
            KeyboardState state = Keyboard.GetState();

            float direction = 0f;


            if (state.IsKeyDown(Keys.Left))
            {
                direction = -1.0f;
                AnimationState = "Walk";
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                direction = 1.0f;
                AnimationState = "Walk";
            }
            else
            {
                AnimationState = "Idle";
            }

            if ((state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.NumPad0)) && !IsJumping)
            {
                IsJumping = true;
                AnimationState = "Jump";
            }

            return direction;
        }
    }
}

using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Input
{
    public class KeyboardReader : IInputReader
    {
        public string AnimationState { get; set; }
        public bool IsJumping { get; set; }
        public bool IsDoubleJumping { get; set; }

        private KeyboardState previousState;

        public KeyboardReader()
        {
            previousState = Keyboard.GetState();
        }
        public float ReadInput()
        {
            KeyboardState state = Keyboard.GetState();

            float direction = 0f;

            //Movement handling
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

            //Jump handling

            if (state.IsKeyDown(Keys.Space) && !previousState.IsKeyDown(Keys.Space))
            {
                Debug.WriteLine("Space bar: " + state.IsKeyDown(Keys.Space));
                if (!IsJumping && !IsDoubleJumping)
                {
                    IsJumping = true;
                    AnimationState = "Jump";
                }
                else if (IsJumping && !IsDoubleJumping)
                {
                    IsJumping = false;
                    IsDoubleJumping = true;
                    AnimationState = "High_Jump";
                }
            }
            else
            {
                IsJumping = false;
                IsDoubleJumping = false;
            }

            if(MouseReader.ReadInput() == "left") { AnimationState = "Attack"; }

            return direction;
        }
    }
}

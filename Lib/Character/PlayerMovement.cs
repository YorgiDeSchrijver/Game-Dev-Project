using GameDevProject.Lib.Input;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Character
{
    public class PlayerMovement
    {
        private readonly MovementManager movementManager;

        public PlayerMovement()
        {
            movementManager = new();
        }

        public void Move(IMoveAble moveAble, GameTime gameTime)
        {
            movementManager.Move(moveAble, gameTime);
        }
    }
}

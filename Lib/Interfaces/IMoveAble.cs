using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Interfaces
{
    public interface IMoveAble
    {
        public Vector2 Position { get; set; }
        public IInputReader InputReader { get; set; }

        public Rectangle BoundingRectangle { get; }

        public bool IsOnGround { get; set; }
    }
}

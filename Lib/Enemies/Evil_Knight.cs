using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Enemies
{
    public class Evil_Knight: Enemy
    {
        public Evil_Knight(Vector2 position, ContentLoader contentLoader, List<Rectangle> collisionObjects, IMoveAble player) : base(position, contentLoader, collisionObjects, player, "Evil_Knight", 256, 20)
        {
        }
    }
}

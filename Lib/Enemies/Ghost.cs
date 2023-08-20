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
    public class Ghost : Enemy
    {
        public Ghost(Vector2 position, ContentLoader contentLoader, List<Rectangle> collisionObjects, IMoveAble player) : base(position, contentLoader, collisionObjects, player, "Ghost", 128, 20)
        {
        }
    }
}

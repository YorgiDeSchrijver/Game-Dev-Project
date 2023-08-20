using GameDevProject.Lib.Extensions;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Enemies
{
    public class EnemyCollisions
    {
        public void HandleCollisions(IEnemy enemy, List<Rectangle> collisionObjects)
        {
            Rectangle bounds = enemy.BoundingRectangle;

            foreach (Rectangle obj in collisionObjects)
            {
                if (bounds.Intersects(obj))
                {
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, obj);
                    if (depth != Vector2.Zero)
                    {

                        float absDepthX = Math.Abs(depth.X);
                        float absDepthY = Math.Abs(depth.Y);


                        if (absDepthY < absDepthX)
                        {
                            enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + depth.Y);
                        }
                        else
                        {
                            enemy.Position = new Vector2(enemy.Position.X + depth.X, enemy.Position.Y);
                        }
                    }
                }
            }
        }
    }
}

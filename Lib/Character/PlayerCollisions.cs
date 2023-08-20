using GameDevProject.Lib.Extensions;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Character
{
    public class PlayerCollisions
    {
        private float previousBottom;
        public void HandleCollisions(IMoveAble moveable, List<Rectangle> collisionObjects)
        {
            bool isOnGround = false;
            Rectangle bounds = moveable.BoundingRectangle;

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
                            moveable.Position = new Vector2(moveable.Position.X, moveable.Position.Y + depth.Y);
                            isOnGround = true;
                        }
                        else
                        {
                            moveable.Position = new Vector2(moveable.Position.X + depth.X, moveable.Position.Y);
                        }
                    }
                }
            }
            moveable.IsOnGround = isOnGround;
            previousBottom = bounds.Bottom;
        }
    }
}

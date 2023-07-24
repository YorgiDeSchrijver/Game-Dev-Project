using GameDevProject.Lib.Extensions;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Collisions
{
    public class PlayerCollisions
    {
        private float previousBottom;
        public void HandleCollisions(IMoveAble moveable, List<Rectangle> collisionObjects, Rectangle BoundingRectangle)
        {
            Rectangle bounds = BoundingRectangle;

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
                            if (previousBottom <= obj.Top)
                                moveable.IsOnGround = true;

                            moveable.Position = new Vector2(moveable.Position.X, moveable.Position.Y + depth.Y);

                            bounds = BoundingRectangle;
                        }
                        else
                        {
                            moveable.Position = new Vector2(moveable.Position.X + depth.X, moveable.Position.Y);

                            bounds = BoundingRectangle;
                        }
                    }
                }
            }

            previousBottom = bounds.Bottom;
        }
    }
}

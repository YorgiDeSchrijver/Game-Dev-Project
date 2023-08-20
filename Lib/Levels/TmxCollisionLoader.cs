using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class TmxCollisionLoader : ICollisionLoader
    {
        public List<Rectangle> LoadCollisionRectangles(TmxMap map)
        {
            List<Rectangle> collisionRectangles = new List<Rectangle>();
            foreach (TmxObjectGroup objectGroup in map.ObjectGroups)
            {
                if (objectGroup.Name == "Collisions")
                {
                    foreach (TmxObject obj in objectGroup.Objects)
                    {
                        Rectangle collisionRect = new Rectangle(
                            (int)obj.X,
                            (int)obj.Y,
                            (int)obj.Width,
                            (int)obj.Height);

                        collisionRectangles.Add(collisionRect);
                    }
                    break;
                }
            }
            return collisionRectangles;
        }
    }
}

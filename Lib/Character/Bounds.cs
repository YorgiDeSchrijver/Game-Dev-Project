using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Character
{
    public static class Bounds
    {
        public static Rectangle Getbounds(Texture2D texture, Rectangle imageBounds)
        {
            Rectangle bounds = new(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int y = imageBounds.Y; y < imageBounds.Y + imageBounds.Height; y++)
            {
                for (int x = imageBounds.X; x < imageBounds.X + imageBounds.Width; x++)
                {
                    int index = (y - imageBounds.Y) * texture.Width + (x - imageBounds.X);
                    Color pixel = data[index];
                    if (pixel.A == 255)
                    {
                        bounds.X = Math.Min(bounds.X, x);
                        bounds.Y = Math.Min(bounds.Y, y);
                        bounds.Width = Math.Max(bounds.Width, x);
                        bounds.Height = Math.Max(bounds.Height, y);
                    }
                }
            }

            bounds.Width -= bounds.X;
            bounds.Height -= bounds.Y;
            bounds.X -= imageBounds.X;
            bounds.Y -= imageBounds.Y;

            //check for even numbers
            bounds.Width = bounds.Width % 2 == 0 ? bounds.Width : bounds.Width--;
            bounds.Height = bounds.Height % 2 == 0 ? bounds.Height : bounds.Height--;
            bounds.X = bounds.X % 2 == 0 ? bounds.X : bounds.X--;
            bounds.Y = bounds.Y % 2 == 0 ? bounds.Y : bounds.Y--;


            return bounds;
        }
    }
}

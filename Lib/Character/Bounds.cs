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
        public static Rectangle GetBounds(Texture2D texture, Rectangle imageBounds)
        {
            int chunkSize = 2;
            Rectangle bounds = new(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            int startX = Math.Max(imageBounds.X, 0);
            int startY = Math.Max(imageBounds.Y, 0);
            int endX = Math.Min(imageBounds.X + imageBounds.Width, texture.Width);
            int endY = Math.Min(imageBounds.Y + imageBounds.Height, texture.Height);

            for (int y = startY; y < endY; y += chunkSize)
            {
                for (int x = startX; x < endX; x += chunkSize)
                {
                    int chunkEndX = Math.Min(x + chunkSize, endX);
                    int chunkEndY = Math.Min(y + chunkSize, endY);

                    for (int subY = y; subY < chunkEndY; subY++)
                    {
                        for (int subX = x; subX < chunkEndX; subX++)
                        {
                            int index = subY * texture.Width + subX;
                            Color pixel = data[index];
                            if (pixel.A == 255)
                            {
                                bounds.X = Math.Min(bounds.X, subX);
                                bounds.Y = Math.Min(bounds.Y, subY);
                                bounds.Width = Math.Max(bounds.Width, subX);
                                bounds.Height = Math.Max(bounds.Height, subY);
                            }
                        }
                    }
                }
            }

            // Clamp the values to even numbers
            bounds.Width = MathHelper.Clamp(bounds.Width - bounds.X + 1, 0, imageBounds.Width);
            bounds.Height = MathHelper.Clamp(bounds.Height - bounds.Y + 1, 0, imageBounds.Height);
            bounds.X = MathHelper.Clamp(bounds.X - imageBounds.X, 0, imageBounds.Width - bounds.Width);
            bounds.Y = MathHelper.Clamp(bounds.Y - imageBounds.Y, 0, imageBounds.Height - bounds.Height);

            return bounds;
        }

    }
}

using GameDevProject.Lib.ContentManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject.Lib.Tiles
{
    public  class Tile
    {
        public Texture2D Texture;

        public static int Width = 64;
        public static int Height = 64;

        private Rectangle sourceRectangle;

        public static readonly Vector2 size = new(Width, Height);
        private Color color;

        public Tile(Texture2D texture,Rectangle rectangle, Color color)
        {
            Texture = texture;
            sourceRectangle = rectangle;
            Width = rectangle.Width;
            Height = rectangle.Height;
            this.color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, position, sourceRectangle, color, 0f, new Vector2(0,sourceRectangle.Height), 1f, SpriteEffects.None, 0f);

            }
        }
    }
}

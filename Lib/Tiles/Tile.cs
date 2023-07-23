using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject.Lib.Tiles
{
    public  class Tile
    {
        public Texture2D Texture;

        public static int Width = 64;
        public static int Height = 64;

        public static readonly Vector2 size = new(Width, Height);

        public Tile(Texture2D texture)
        {
            Texture = texture;
        }

        public Tile(Texture2D texture, int width, int height)
        {
            Texture = texture;
            Width = width;
            Height = height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, position, Texture.Bounds, Color.White, 0f, new Vector2(0, Texture.Height), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}

using GameDevProject.Lib.ContentManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.UI
{
    public class Button
    {
        private Rectangle Bounds { get; set; }
        private Texture2D NormalTexture { get; set; }
        private Texture2D HoverTexture { get; set; }

        public bool Ishovered { get; private set; }

        public Button(Rectangle bounds, ContentLoader contentLoader)
        {
            Bounds = bounds;
            NormalTexture = contentLoader.LoadTexture("UI/Button/buttons1_yellow");
            HoverTexture = contentLoader.LoadTexture("UI/Button/buttons1");
        }

        public Button(Rectangle bounds, Texture2D differtTexture)
        {
            Bounds = bounds;
            NormalTexture = differtTexture;
        }

        public void Update(Vector2 mousePosition)
        {
            Ishovered = Bounds.Contains(mousePosition);
            if (Ishovered){ Mouse.SetCursor(MouseCursor.Hand); } else { Mouse.SetCursor(MouseCursor.Arrow); }
        }

        public Texture2D GetCurrentTexture()
        {
            if (HoverTexture != null) return Ishovered ? HoverTexture : NormalTexture;
            else return NormalTexture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenOffset, float scale)
        {
            spriteBatch.Draw(GetCurrentTexture(), new Vector2(Bounds.X, Bounds.Y) + screenOffset, null, Color.White,0f, new Vector2(0,0), scale, SpriteEffects.None,0f);
        }

    }
}

using GameDevProject.Lib.Animations;
using GameDevProject.Lib.Character;
using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Objects
{
    public abstract class Object : IGameObject
    {
        private readonly Animation animation;
        private Texture2D texture;

        private readonly ContentLoader contentLoader;
        public Vector2 Position { get; private set; }
        public Vector2 Origin { get; private set; }

        private Rectangle bounds;
        public Rectangle BoundingRectangle 
        { 
            get
            {
                if(animation.CurrentFrame != null)
                {
                    if (bounds.IsEmpty)
                    {
                        bounds = Bounds.GetBounds(texture, animation.CurrentFrame.SourceRectangle);
                    }
                    int left = (int)Math.Round(Position.X - Origin.X) + bounds.X;
                    int top = (int)Math.Round(Position.Y - Origin.Y) + bounds.Y;
                    return new Rectangle(left, top, bounds.Width, bounds.Height);
                }
                return new Rectangle(0, 0, 0, 0);
            } 
        }

        public Object(Vector2 position, ContentLoader contentLoader, string objectType, int imageSize, int numberOfSprites)
        {
            Position = position;
            this.contentLoader = contentLoader;
            animation = new();
            Origin = new Vector2(-(64-imageSize)/2, -(64-imageSize)/2);
            LoadContent(objectType, imageSize, numberOfSprites);

        }

        private void LoadContent(string objectType, int imageSize, int numberOfSprites)
        {
            texture = contentLoader.LoadTexture("Objects/" + objectType + "/" + objectType);

            animation.SetTextureProperties(texture.Width, texture.Height, imageSize, numberOfSprites);
            animation.SetAnimationCycles("Content/Objects/" + objectType + "/Animation.Json");
            //An object has only 1 animation state
            animation.Play("Idle");
        }
        
        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}

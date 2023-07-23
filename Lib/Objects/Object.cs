using GameDevProject.Lib.Animations;
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

        private readonly ContentManager content;
        public Vector2 Position { get; private set; }


        public Object(Vector2 position, ContentManager content, string objectType, int imageSize, int numberOfSprites)
        {
            Position = position;
            this.content = content;
            animation = new();

            LoadContent(objectType, imageSize, numberOfSprites);

        }

        private void LoadContent(string objectType, int imageSize, int numberOfSprites)
        {
            texture = content.Load<Texture2D>("Objects/" + objectType + "/SpriteSheet");

            animation.SetTextureProperties(texture.Width, texture.Height, imageSize, numberOfSprites);
            animation.SetAnimationCycles("Content/Objects/" + objectType + "/Animation.Json");
            //An object has only 1 animation state
            animation.Play("Idle");
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, animation.CurrentFrame.SourceRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }
    }
}

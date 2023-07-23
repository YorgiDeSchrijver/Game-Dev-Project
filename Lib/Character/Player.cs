using GameDevProject.Lib.Animations;
using GameDevProject.Lib.WindowCamera;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameDevProject.Lib.Character
{
    public class Player: IGameObject, IMoveAble
    {
        private readonly Animation animation;
        private readonly IInputReader inputReader;
        private readonly ContentManager content;
        private Texture2D texture;
        private string animationState;

        public Vector2 Position { get; set; }
        public Rectangle BoundingRectangle { get; set; }

        public Player(Vector2 position, IInputReader input, ContentManager content, string characterType)
        {
            this.Position = position;
            this.content = content;
            inputReader = input;
            animation = new();

            LoadContent(characterType);
        }

        private void LoadContent(string characterType)
        {
            texture = content.Load<Texture2D>("Character/"+characterType+"/SpriteSheet");

            animation.SetTextureProperties(texture.Width, texture.Height, 128, 90);
            animation.SetAnimationCycles("Content/Character/"+characterType+"/Animation.Json");
            animation.Play(animationState);
        }

        public void Update(GameTime gameTime)
        {
            GetState(this);
            Camera.Follow(this);

            animation.Play(animationState);
            animation.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, animation.Origin, 1f, SpriteEffects.None, 0f);
        }

        public void GetState(IMoveAble moveAble)
        {
            animationState = moveAble.InputReader.AnimationState;
        }
    }
}

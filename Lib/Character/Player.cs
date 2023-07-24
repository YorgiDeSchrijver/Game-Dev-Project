using GameDevProject.Lib.Animations;
using GameDevProject.Lib.WindowCamera;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameDevProject.Lib.Input;
using GameDevProject.Lib.Collisions;
using System.Collections.Generic;
using System;

namespace GameDevProject.Lib.Character
{
    public class Player: IGameObject, IMoveAble
    {
        private readonly Animation animation;
        private readonly ContentManager content;
        private Texture2D texture;
        private string animationState;
        private MovementManager movementManager;
        private Rectangle bounds;
        private List<Rectangle> collisionObjects;

        public Vector2 Position { get; set; }

        public Rectangle BoundingRectangle
        {
            get
            {
                if (animation.CurrentFrame != null)
                {
                    if (bounds.IsEmpty)
                    {
                        bounds = Bounds.Getbounds(texture, animation.CurrentFrame.SourceRectangle);
                    }
                    int left = (int)Math.Round(Position.X - animation.Origin.X) + bounds.X;
                    int top = (int)Math.Round(Position.Y - animation.Origin.Y) + bounds.Y;
                    return new Rectangle(left, top, bounds.Width, bounds.Height);
                }
                return new Rectangle(0, 0, 0, 0);
            }
        }
        public IInputReader InputReader { get; set; }
        public bool IsOnGround { get; set; }
        public PlayerCollisions collisions { get; set; } //Get this out of player class

        public Player(Vector2 position, IInputReader input, ContentManager content, string characterType, List<Rectangle> collisionObjects)
        {
            this.Position = position;
            this.content = content;
            this.collisionObjects = collisionObjects;
            InputReader = input;
            animation = new();
            movementManager = new();
            collisions = new();

            animationState = "Idle";
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
            Move(gameTime);
            GetState(this);
            Camera.Follow(this);

            animation.Play(animationState);
            animation.Update(gameTime);
            if (IsOnGround) { InputReader.IsJumping = false; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, animation.Origin, 1f, SpriteEffects.None, 0f);
        }

        private void Move(GameTime gameTime)
        {
            movementManager.Move(this, gameTime);
        }

        public void GetState(IMoveAble moveAble)
        {
            animationState = moveAble.InputReader.AnimationState;
        }

        public void HandleCollisions()
        {
            collisions.HandleCollisions(this, collisionObjects, BoundingRectangle);
        }
    }
}

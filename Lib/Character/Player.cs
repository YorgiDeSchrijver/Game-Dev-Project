using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using GameDevProject.Lib.WindowCamera;
using GameDevProject.Lib.Animations;
using System.Diagnostics;

namespace GameDevProject.Lib.Character
{
    public class Player : IGameObject, IMoveAble
    {
        private readonly PlayerAnimation playerAnimation;
        private readonly PlayerMovement playerMovement;
        private readonly PlayerCollisions playerCollisions;

        private readonly Texture2D texture;
        private readonly List<Rectangle> collisionObjects;
        private string animationState;
        private SpriteEffects spriteEffects;

        private AnimationFrame lastFrame;
        private Rectangle lastFrameBounds;
        public Rectangle BoundingRectangle 
        { 
            get 
            { 
                if(playerAnimation.currentAnimation.CurrentFrame != null)
                {
                    if(playerAnimation.currentAnimation.CurrentFrame != lastFrame)
                    {
                        lastFrameBounds = Bounds.GetBounds(texture, playerAnimation.currentAnimation.CurrentFrame.SourceRectangle);
                        lastFrame = playerAnimation.currentAnimation.CurrentFrame;
                    }
                    int left = (int)Math.Round(Position.X - playerAnimation.currentAnimation.Origin.X) + lastFrameBounds.X;
                    int top = (int)Math.Round(Position.Y - playerAnimation.currentAnimation.Origin.Y) + lastFrameBounds.Y;
                    return new Rectangle(left, top, lastFrameBounds.Width, lastFrameBounds.Height);
                }
                return new(0, 0, 0, 0);
            } 
        }

        public Vector2 Position { get; set; }
        public IInputReader InputReader { get; }
        
        public bool IsOnGround { get; set; }

        public Player(Texture2D texture, Vector2 position, IInputReader inputReader, List<Rectangle> collisionObjects)
        {
            this.texture = texture;
            this.collisionObjects = collisionObjects;

            Position = position;
            InputReader = inputReader;

            playerAnimation = new(texture, "Content/Character/Knight/Animation.json");
            playerMovement = new();
            playerCollisions = new();
        }


        public void Update(GameTime gameTime)
        {
            Camera.Follow(this);
            //Movement
            playerMovement.Move(this, gameTime);
            //Animation
            animationState = InputReader.AnimationState;
            playerAnimation.Play(animationState);
            playerAnimation.Update(gameTime);
            //Collision
            playerCollisions.HandleCollisions(this, collisionObjects);

            spriteEffects = InputReader.ReadInput() < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, Position, playerAnimation.currentAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, new Vector2(playerAnimation.currentAnimation.CurrentFrame.SourceRectangle.Width / 2, playerAnimation.currentAnimation.CurrentFrame.SourceRectangle.Height) /*playerAnimation.currentAnimation.Origin*/, 1f, spriteEffects, 0f);
            // Draw the bounding rectangle around the player character
            Rectangle boundingRect = BoundingRectangle;
            spriteBatch.DrawRectangle(boundingRect, Color.Red * 0.5f);
        }
    }
}

public static class SpriteBatchExtensions
{
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        Texture2D pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { color });

        spriteBatch.Draw(pixel, rectangle, color);
    }
}

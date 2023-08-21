using GameDevProject.Lib.Animations;
using GameDevProject.Lib.Character;
using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Enemies
{
    public abstract class Enemy : IEnemy
    {
        public enum EnemyState { Idle, Walk, Death }
        public EnemyState CurrentState { get; set; }
        public string currentAnimationState {get;set;}
        private enum Direction { Left, Right}
        private Direction currentDirection;

        private readonly EnemyMovement enemyMovement;
        private readonly EnemyCollisions enemyCollisions;

        private readonly Animation animation;
        private readonly Texture2D texture;
        private readonly List<Rectangle> collisionRectangles;
        private Vector2 lastPosition;
        private readonly IMoveAble player;

        //Durations in milliseconds
        protected const int idleDuration = 1500;
        protected const int attackCooldown = 1500;
        protected const int maxdistanceXToPlayer = 75;
        protected const int maxWaittimeAttack = 500;
        protected int lives = 3;

        protected int timeSinceIdleStart;
        protected int timeSinceLastAttack;
        protected int timeSinceWaittimeAttack;

        private Rectangle lastFrameBounds;
        public Rectangle BoundingRectangle
        {
            get 
            {
                if (animation.CurrentFrame != null)
                {
                    if (lastFrameBounds.IsEmpty)
                    {
                        lastFrameBounds = Bounds.GetBounds(texture, animation.CurrentFrame.SourceRectangle);
                    }

                    int left = (int)Math.Round(Position.X - animation.Origin.X) + lastFrameBounds.X;
                    int top = (int)Math.Round(Position.Y - animation.Origin.Y) + lastFrameBounds.Y;
                    return new Rectangle(left, top, lastFrameBounds.Width, lastFrameBounds.Height);
                }
                return new(0, 0, 0, 0);
            }
        }

        public Vector2 Position { get; set; }

        public Enemy(Vector2 position, ContentLoader contentLoader, List<Rectangle> collisionRectangles, IMoveAble player, string enemyType, int imageSize, int numberOfSprites) 
        { 
            this.Position = position;
            this.collisionRectangles = collisionRectangles;
            this.player = player;

            CurrentState = EnemyState.Walk;
            currentDirection = Direction.Left;

            enemyMovement = new();
            enemyCollisions = new();

            texture = contentLoader.LoadTexture("Enemies/" + enemyType + "/" + enemyType);
            animation = new();
            animation.SetTextureProperties(texture.Width, texture.Height, imageSize, numberOfSprites);
            animation.SetAnimationCycles("Content/Enemies/" + enemyType + "/Animation.json");
            animation.Play("Walk");
        }

        protected void ChangeState(EnemyState newState)
        {
            if(CurrentState != newState)
            {
                CurrentState = newState;
                currentAnimationState = newState.ToString();
                animation.Play(currentAnimationState);
                
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (CurrentState)
            {
                case EnemyState.Idle:
                    timeSinceIdleStart += gameTime.ElapsedGameTime.Milliseconds;
                    if(timeSinceIdleStart >= idleDuration)
                    {
                        ChangeState(EnemyState.Walk);
                        timeSinceIdleStart = 0;
                        currentDirection = currentDirection == Direction.Left? Direction.Right : Direction.Left;
                    }
                    break;
                case EnemyState.Walk:
                    int direction = currentDirection == Direction.Left ? -1 : 1;
                    enemyMovement.Move(this, gameTime, direction);
                    enemyCollisions.HandleCollisions(this, collisionRectangles);
                    if (lastPosition == Position) { ChangeState(EnemyState.Idle); } else { lastPosition = Position; }
                    break;
                case EnemyState.Death:
                    break;
            }
            if(player.InputReader.AnimationState == "Attack" && player.BoundingRectangle.Intersects(BoundingRectangle)) 
            { 
                ChangeState(EnemyState.Death); 
            }
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, animation.CurrentFrame.SourceRectangle, Color.White, 0f, new Vector2(animation.CurrentFrame.SourceRectangle.Width / 2, animation.CurrentFrame.SourceRectangle.Height), 1f, currentDirection == Direction.Left? SpriteEffects.FlipHorizontally
                : SpriteEffects.None, 0f);
        }

    }
}

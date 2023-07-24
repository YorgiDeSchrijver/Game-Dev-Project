using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Input
{
    public class MovementManager
    {
        // Constants for controlling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        public static Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        static Vector2 velocity;


        private bool wasJumping;
        private float jumpTime;
        public void Move(IMoveAble moveable, GameTime gameTime)
        {
            var direction = moveable.InputReader.ReadInput();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = moveable.Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += direction * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime, moveable);

            // Apply pseudo-drag horizontally.
            if (moveable.IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            moveable.Position += velocity * elapsed;
            moveable.Position = new Vector2((float)Math.Round(moveable.Position.X), (float)Math.Round(moveable.Position.Y));

            // Handle Collisions
            moveable.HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (moveable.Position.X == previousPosition.X)
                velocity.X = 0;
            if (moveable.Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        public float DoJump(float velocityY, GameTime gameTime, IMoveAble moveable)
        {
            // If the player wants to jump
            if (moveable.InputReader.IsJumping)
            {

                // Begin or continue a jump
                if ((!wasJumping && moveable.IsOnGround) || jumpTime > 0.0f)
                {
                    moveable.IsOnGround = false;
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = moveable.InputReader.IsJumping;

            return velocityY;
        }
    }
}

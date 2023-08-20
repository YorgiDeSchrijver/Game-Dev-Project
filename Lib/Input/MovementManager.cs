using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const float JumpLaunchVelocity = -5000.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;
        private const int MaxJumps = 4;

        private Vector2 velocity;
        private bool wasJumping;
        private float jumpTime;
        private int numJumps;

        public void Move(IMoveAble moveable, GameTime gameTime)
        {
            var direction = moveable.InputReader.ReadInput();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = moveable.Position;

            velocity.X += direction * MoveAcceleration * elapsed;
            ApplyGravity(moveable, elapsed);
            ApplyJump(moveable, gameTime);

            if (moveable.IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            moveable.Position += velocity * elapsed;
            moveable.Position = new Vector2((float)Math.Round(moveable.Position.X), (float)Math.Round(moveable.Position.Y));

            // If the collision stopped us from moving, reset the velocity to zero.
            if (moveable.Position.X == previousPosition.X)
                velocity.X = 0;
            if (moveable.Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        private void ApplyGravity(IMoveAble moveable, float elapsed)
        {
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
        }

        private void ApplyJump(IMoveAble moveable, GameTime gameTime)
        {
            velocity.Y = CalculateJumpVelocity(velocity.Y, gameTime, moveable);
        }

        private float CalculateJumpVelocity(float velocityY, GameTime gameTime, IMoveAble moveable)
        {
            if (moveable.InputReader.IsJumping || moveable.InputReader.IsDoubleJumping)
            {
                Debug.WriteLine( $"{jumpTime}, {numJumps}, {moveable.InputReader.IsJumping},  {moveable.InputReader.IsDoubleJumping}");
                if ((!wasJumping && moveable.IsOnGround) || jumpTime > 0.0f || numJumps < MaxJumps)
                {
                    if (!wasJumping && moveable.IsOnGround)
                        numJumps = 0;

                     /*if (numJumps < MaxJumps)
                    {
                         numJumps++;
                    }*/   

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (0.0f < jumpTime && jumpTime <= MaxJumpTime * MaxJumps)
                {
                    return JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    jumpTime = 0.0f;
                }
            }
            else
            {
                jumpTime = 0.0f;
            }

            

            wasJumping = moveable.InputReader.IsJumping;
            return velocityY;
        }
    }
}

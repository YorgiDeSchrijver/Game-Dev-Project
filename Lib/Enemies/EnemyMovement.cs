using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Enemies
{
    public class EnemyMovement
    {
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;

        private Vector2 velocity;
        public void Move(IEnemy enemy, GameTime gameTime, int direction)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = enemy.Position;

            velocity.X += direction * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.X *= GroundDragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity.
            enemy.Position += velocity * elapsed;
            enemy.Position = new Vector2((float)Math.Round(enemy.Position.X), (float)Math.Round(enemy.Position.Y));

            // If the collision stopped us from moving, reset the velocity to zero.
            if (enemy.Position.X == previousPosition.X)
                velocity.X = 0;
            if (enemy.Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }
    }
}

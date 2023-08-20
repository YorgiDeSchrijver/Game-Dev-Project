using GameDevProject.Lib.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Character
{
    public class PlayerAnimation
    {
        private readonly Animation animation;
        public Animation currentAnimation => animation; //?
        public PlayerAnimation(Texture2D texture, string animationJsonPath)
        {
            animation = new();
            animation.SetTextureProperties(texture.Width, texture.Height, 128, 90);
            animation.SetAnimationCycles(animationJsonPath);
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Play(string animationState)
        {
            animation.Play(animationState);
        }


    }
}

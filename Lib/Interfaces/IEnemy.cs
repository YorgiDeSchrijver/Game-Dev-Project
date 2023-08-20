﻿using GameDevProject.Lib.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Interfaces
{
    public interface IEnemy
    {
        Vector2 Position { get; set; }
        Rectangle BoundingRectangle { get; }
        string currentAnimationState { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}

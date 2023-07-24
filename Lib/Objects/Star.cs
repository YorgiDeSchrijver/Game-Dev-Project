﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Objects
{
    public class Star : Object
    {
        public Star(Vector2 position, ContentManager content) : base(position, content, "Star", 32, 12)
        {
        }
    }
}
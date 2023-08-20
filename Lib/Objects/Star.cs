using GameDevProject.Lib.ContentManagement;
using Microsoft.Xna.Framework;
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
        public Star(Vector2 position, ContentLoader contentLoader) : base(position, contentLoader, "Star", 32, 12)
        {
        }
    }
}

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.ContentManagement
{
    public  class ContentLoader
    {
        private ContentManager content;
        public ContentManager Content => content;
        public ContentLoader(ContentManager content)
        {
            this.content = content;
        }

        public Texture2D LoadTexture(string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        public SpriteFont LoadFont(string assetName)
        {
            return content.Load<SpriteFont>(assetName);
        }
    }
}

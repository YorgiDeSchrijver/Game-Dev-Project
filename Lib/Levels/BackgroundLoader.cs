using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Levels
{
    public class BackgroundLoader: IBackgroundLoader
    {
        public List<Texture2D> LoadBackgrounds(ContentLoader contentLoader)
        {
            List<Texture2D> backgrounds = new();
            for (int i = 1; i <= 6; i++)
            {
                backgrounds.Add(contentLoader.LoadTexture("World/Forest/Background/" + i));
            }
            return backgrounds;
        }
    }
}

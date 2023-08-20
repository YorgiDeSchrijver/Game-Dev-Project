using GameDevProject.Lib.ContentManagement;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Interfaces
{
    public interface IBackgroundLoader
    {
        List<Texture2D> LoadBackgrounds(ContentLoader contentLoader);
    }
}

using GameDevProject.Lib.ContentManagement;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TiledSharp;

namespace GameDevProject.Lib.Interfaces
{
    public interface ITilesetLoader
    {
        Dictionary<int, Texture2D> LoadTilesets(TmxMap map, ContentLoader contentLoader);
    }
}

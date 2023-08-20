using GameDevProject.Lib.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TiledSharp;

namespace GameDevProject.Lib.Interfaces
{
    public interface ILayerLoader
    {
        Tile[,,] LoadLayers(TmxMap map, Dictionary<int, Texture2D> tilesetTextures);
        Dictionary<int, Vector2> LoadLayerOffsets(TmxMap map);
    }
}

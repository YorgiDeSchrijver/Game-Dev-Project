using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class TmxTilesetLoader : ITilesetLoader
    {
        public Dictionary<int, Texture2D> LoadTilesets(TmxMap map, ContentLoader contentLoader)
        {
            Dictionary<int, Texture2D> tilesetTextures = new Dictionary<int, Texture2D>();

            foreach (TmxTileset tileset in map.Tilesets)
            {
                string tilesetImagePath = tileset.Image.Source;
                string tilesetTexturePath = Path.GetRelativePath(contentLoader.Content.RootDirectory, tilesetImagePath);
                Texture2D tilesetTexture = contentLoader.LoadTexture(tilesetTexturePath);
                tilesetTextures.Add(tileset.FirstGid, tilesetTexture);
            }

            return tilesetTextures;
        }
    }
}

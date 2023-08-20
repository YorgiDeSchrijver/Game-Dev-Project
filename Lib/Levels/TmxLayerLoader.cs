using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using GameDevProject.Lib.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class TmxLayerLoader: ILayerLoader
    {
        private GraphicsDevice graphicsDevice;
        public TmxLayerLoader(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }
        public Tile[,,] LoadLayers(TmxMap map, Dictionary<int, Texture2D> tilesetTextures)
        {
            Tile[,,] tiles = new Tile[map.Width, map.Height, map.Layers.Count];

            int layerIndex = 0;
            foreach (TmxLayer layer in map.Layers)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        int tileGid = layer.Tiles[x + y * map.Width].Gid;
                        Texture2D tileTexture = GetTilesetTextureFromGid(tileGid, tilesetTextures, map);
                        Rectangle sourceRect = GetSourceRectangle(tileGid, tilesetTextures, map);
                        Color color = (layer.Name == "BackgroundTiles") ? Color.Black : Color.White;
                        tiles[x, y, layerIndex] = new Tile(tileTexture, sourceRect, color);
                    }
                }
                layerIndex++;
            }

            return tiles;
        }

        public Dictionary<int, Vector2> LoadLayerOffsets(TmxMap map)
        {
            Dictionary<int, Vector2> layerOffsets = new Dictionary<int, Vector2>();
            int layerIndex = 0;
            foreach (TmxLayer layer in map.Layers)
            {
                layerOffsets.Add(layerIndex, new((float)layer.OffsetX, (float)layer.OffsetY));
                layerIndex++;
            }
            return layerOffsets;
        }

        private Texture2D GetTilesetTextureFromGid(int gid, Dictionary<int, Texture2D> tilesetTextures, TmxMap map)
        {
            for (int i = tilesetTextures.Count - 1; i >= 0; i--)
            {
                TmxTileset tileset = map.Tilesets[i];
                if (gid >= tileset.FirstGid)
                {
                    return tilesetTextures[tileset.FirstGid];
                }
            }

            return null;
        }

        private Rectangle GetSourceRectangle(int gid, Dictionary<int, Texture2D> tilesetTextures, TmxMap map)
        {
            for (int i = tilesetTextures.Count - 1; i >= 0; i--)
            {
                TmxTileset tileset = map.Tilesets[i];
                if (gid >= tileset.FirstGid)
                {
                    int tileIndex = gid - tileset.FirstGid;

                    int tilesetWidth = tileset.TileWidth;
                    int tilesetHeight = tileset.TileHeight;

                    int tilesPerRow = (int)(tileset.Image.Width / tilesetWidth);

                    int tilesetX = (tileIndex % tilesPerRow) * tilesetWidth;
                    int tilesetY = (tileIndex / tilesPerRow) * tilesetHeight;

                    Rectangle sourceRect = new(tilesetX, tilesetY, tilesetWidth, tilesetHeight);

                    return sourceRect;
                }
            }
            return new(0,0,64,64);
        }
    }
}

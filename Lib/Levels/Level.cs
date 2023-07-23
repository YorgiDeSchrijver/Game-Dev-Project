using GameDevProject.Lib.Character;
using GameDevProject.Lib.Interfaces;
using GameDevProject.Lib.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class Level
    {
        private readonly ContentManager content;
        private readonly GraphicsDevice graphicsDevice;
        private readonly IInputReader inputReader;

        private TmxMap map;
        private readonly Dictionary<int, Texture2D> tilesetTextures;
        private readonly Dictionary<int, Vector2> tileLayerOffsets;
        private readonly Tile[,,] tiles;
        private readonly List<Rectangle> collisionObjects;
        private readonly List<IGameObject> levelObjects;


        public Level(IServiceProvider serviceProvider, Stream fileStream, IInputReader input, GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            inputReader = input;
            content = new ContentManager(serviceProvider, "Content");

            tilesetTextures = new();
            tileLayerOffsets = new();
            collisionObjects = new();

            LoadMap(fileStream);
        }

        private void LoadMap(Stream fileStream)
        {
            map = new TmxMap(fileStream);

            LoadTilesets();
            LoadMapLayers();
            LoadObjects();
        }

        private void LoadTilesets()
        {
            foreach (TmxTileset tileset in map.Tilesets)
            {
                string tilesetImagePath = tileset.Image.Source;
                string tilesetTexturePath = Path.GetRelativePath(content.RootDirectory, tilesetImagePath);
                Texture2D tilesetTexture = content.Load<Texture2D>(tilesetTexturePath);
                tilesetTextures.Add(tileset.FirstGid, tilesetTexture);
            }
        }

        private void LoadMapLayers()
        {
            int index = 0;
            foreach (TmxLayer layer in map.Layers)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        int tileGid = layer.Tiles[x + y * map.Width].Gid;
                        Texture2D tileTexture = GetTileTextureFromGid(tileGid);
                        tiles[x, y, index] = new Tile(tileTexture);
                    }
                }
                index++;
            }
        }

        private void LoadObjects()
        {
            foreach (TmxObjectGroup objectGroup in map.ObjectGroups)
            {
                switch (objectGroup.Name)
                {
                    case "Collisions":
                        LoadCollisionRectangles(objectGroup);
                        break;
                    case "Start":
                        LoadPlayerObject(objectGroup);
                        break;
                    default:
                        break;
                }
            }
        }

        private void LoadCollisionRectangles(TmxObjectGroup objectGroup)
        {
            foreach (TmxObject tmxObject in objectGroup.Objects)
            {
                Rectangle collisionRect = new((int)tmxObject.X, (int)tmxObject.Y - map.TileHeight, (int)tmxObject.Width, (int)tmxObject.Height);
                collisionObjects.Add(collisionRect);
            }
        }

        private void LoadPlayerObject(TmxObjectGroup objectGroup)
        {
            if (objectGroup.Objects.Count > 1)
            {
                throw new ArgumentException("Can't create more than 1 start!");
            }
            else if (objectGroup.Objects.Count == 0)
            {
                throw new ArgumentException("You need a starting point!");
            }
            else
            {
                TmxObject obj = objectGroup.Objects[0];
                Vector2 position = new((int)obj.X, (int)obj.Y);
                levelObjects.Add(new Player(position, inputReader, content, "Knight"));
            }
        }

        private Texture2D GetTileTextureFromGid(int gid)
        {
            List<int> tilesetKeys = new(tilesetTextures.Keys);
            int low = 0;
            int high = tilesetKeys.Count - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                int tilesetKey = tilesetKeys[mid];
                TmxTileset tileset = map.Tilesets[tilesetKey];

                if (gid >= tileset.FirstGid)
                {
                    int tileIndex = gid - tileset.FirstGid;
                    int tilesetWidth = tileset.TileWidth;
                    int tilesetHeight = tileset.TileHeight;
                    int tilesPerRow = (int)(tileset.Image.Width / tilesetWidth);
                    int tilesetX = (tileIndex % tilesPerRow) * tilesetWidth;
                    int tilesetY = (tileIndex / tilesPerRow) * tilesetHeight;
                    Rectangle sourceRect = new(tilesetX, tilesetY, tilesetWidth, tilesetHeight);

                    return CreateTileTexture(tilesetTextures[tilesetKey], sourceRect);
                }
                else if (gid < tileset.FirstGid)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }
            return null;
        }
      

        private Texture2D CreateTileTexture(Texture2D spritesheet, Rectangle sourceRect)
        {
            Texture2D tileTexture = new(graphicsDevice, sourceRect.Width, sourceRect.Height);
            Color[] data = new Color[sourceRect.Width * sourceRect.Height];
            spritesheet.GetData(0, sourceRect, data, 0, data.Length);
            tileTexture.SetData(data);
            return tileTexture;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var levelObject in levelObjects)
            {
                levelObject.Update(gameTime);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int z = 0; z < tiles.GetLength(2); z++)
            {
                for (int x = 0; x < tiles.GetLength(0); ++x)
                {
                    for (int y = 0; y < tiles.GetLength(1); y++)
                    {
                        tiles[x, y, z].Draw(spriteBatch, new Vector2(x, y) * Tile.size + tileLayerOffsets[z]);
                    }
                }
            }
            foreach (IGameObject levelObject in levelObjects)
            {
                levelObject.Draw(spriteBatch);
            }
        }
    }
}

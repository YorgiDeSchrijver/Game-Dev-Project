using GameDevProject.Lib.Character;
using GameDevProject.Lib.Interfaces;
using GameDevProject.Lib.Objects;
using GameDevProject.Lib.Tiles;
using GameDevProject.Lib.WindowCamera;
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
        private Tile[,,] tiles;
        private readonly List<Rectangle> collisionObjects;
        private readonly List<IGameObject> levelObjects;
        private readonly List<Texture2D> backgrounds;
        private readonly float[] parallaxFactors = { 0.6f, 0.4f, 0.4f, 0.3f, 0.2f, 0.1f};

        public Level(IServiceProvider serviceProvider, Stream fileStream, string world, IInputReader input, GraphicsDevice graphics)
        {
            graphicsDevice = graphics;
            inputReader = input;
            content = new ContentManager(serviceProvider, "Content");

            tilesetTextures = new();
            tileLayerOffsets = new();
            collisionObjects = new();
            levelObjects = new();
            backgrounds = new();

            LoadMap(fileStream, world);
        }

        private void LoadMap(Stream fileStream, string world)
        {
            map = new TmxMap(fileStream);

            LoadBackground(world);
            LoadTilesets();
            LoadMapLayers();
            LoadObjects();
        }

        private void LoadBackground(string world)
        {
            for (int i = 1; i <= 6; i++)
            {
                backgrounds.Add(content.Load<Texture2D>("World/" + world + "/Background/" + i));
            }
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
            tiles = new Tile[map.Width, map.Height, map.Layers.Count];
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
                tileLayerOffsets.Add(index, new((float)layer.OffsetX, (float)layer.OffsetY));
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
                        LoadPlayer(objectGroup);
                        break;
                    case "Coins":
                        LoadCoins(objectGroup);
                        break;
                    case "Crystals":
                        LoadCrystals(objectGroup);
                        break;
                    case "Hearts":
                        LoadHearts(objectGroup);
                        break;
                    case "Stars":
                        LoadStars(objectGroup);
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

        private void LoadPlayer(TmxObjectGroup objectGroup)
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
                levelObjects.Add(new Player(position, inputReader, content, "Knight", collisionObjects));
            }
        }

        private void LoadCoins(TmxObjectGroup objectGroup)
        {
            foreach (TmxObject obj in objectGroup.Objects)
            {
                Vector2 position = new((int)obj.X, (int)obj.Y);
                levelObjects.Add(new Coin(position, content));
            }
        }


        private void LoadCrystals(TmxObjectGroup objectGroup)
        {
            foreach (TmxObject obj in objectGroup.Objects)
            {
                Vector2 position = new((int)obj.X, (int)obj.Y);
                levelObjects.Add(new Crystal(position, content));
            }
        }

        private void LoadHearts(TmxObjectGroup objectGroup)
        {
            foreach (TmxObject obj in objectGroup.Objects)
            {
                Vector2 position = new((int)obj.X, (int)obj.Y);
                levelObjects.Add(new Heart(position, content));
            }
        }

        private void LoadStars(TmxObjectGroup objectGroup)
        {
            foreach (TmxObject obj in objectGroup.Objects)
            {
                Vector2 position = new((int)obj.X, (int)obj.Y);
                levelObjects.Add(new Star(position, content));
            }
        }

        private Texture2D GetTileTextureFromGid(int gid)
        {
            /*List<int> tilesetKeys = new(tilesetTextures.Keys);
            int low = 0;
            int high = tilesetKeys.Count - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                TmxTileset tileset = map.Tilesets[mid];

                if (gid >= tileset.FirstGid)
                {
                    int tileIndex = gid - tileset.FirstGid;
                    int tilesetWidth = tileset.TileWidth;
                    int tilesetHeight = tileset.TileHeight;
                    int tilesPerRow = (int)(tileset.Image.Width / tilesetWidth);
                    int tilesetX = (tileIndex % tilesPerRow) * tilesetWidth;
                    int tilesetY = (tileIndex / tilesPerRow) * tilesetHeight;
                    Rectangle sourceRect = new(tilesetX, tilesetY, tilesetWidth, tilesetHeight);

                    return CreateTileTexture(tilesetTextures[tileset.FirstGid], sourceRect);
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
            return null;*/

            for(int i = tilesetTextures.Count - 1; i >= 0; i--)
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

                    Rectangle sourceRect = new Rectangle(tilesetX, tilesetY, tilesetWidth, tilesetHeight);

                    return CreateTileTexture(tilesetTextures[tileset.FirstGid], sourceRect);
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
            float mapWidth = map.Width * Tile.size.X; // Calculate the total width of the map

            for (int i = 0; i < backgrounds.Count; i++)
            {
                float layerDepth = 1.0f - (float)i / backgrounds.Count;
                float parallaxFactor = parallaxFactors[i];

                // Calculate the parallax offset based on camera position and parallax factor
                float parallaxOffsetX = -(Camera.Transform.Translation.X * parallaxFactor) % backgrounds[i].Width;
                float parallaxOffsetY = -(Camera.Transform.Translation.Y) - backgrounds[i].Height/2;

                Vector2 backgroundPosition = new Vector2(parallaxOffsetX, parallaxOffsetY);

                for (double j = 0; j < mapWidth; j += backgrounds[i].Width * 1.6)
                {
                    spriteBatch.Draw(backgrounds[i], backgroundPosition + new Vector2((float)j, 0f), null, Color.White, 0f, Vector2.Zero, 1.6f, SpriteEffects.None, layerDepth);
                }
            }

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

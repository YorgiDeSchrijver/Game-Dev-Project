using GameDevProject.Lib.Character;
using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using GameDevProject.Lib.Objects;
using GameDevProject.Lib.Tiles;
using GameDevProject.Lib.WindowCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class Level: IDisposable
    {
        private readonly IMapLoader mapLoader;
        private readonly ITilesetLoader tilesetLoader;
        private readonly ILayerLoader layerLoader;
        private readonly ICollisionLoader collisionLoader;
        private readonly IObjectLoader objectLoader;
        private readonly IPlayerLoader playerLoader;
        private readonly IEnemyLoader enemyLoader;
        private readonly IBackgroundLoader backgroundLoader;
        private ContentLoader contentLoader;

        private readonly TmxMap map;
        private readonly Dictionary<int, Texture2D> tilesetTextures;
        private readonly Tile[,,] tiles;
        private readonly Dictionary<int, Vector2> layerOffsets;
        private readonly List<Rectangle> collisionObjects;
        private readonly List<IGameObject> gameObjects;
        private readonly List<IEnemy> enemies;
        private readonly List<Texture2D> backgrounds;
        private readonly Rectangle exit;
        private readonly Player player;

        private bool isPlayerDefeated = false;
        public bool IsPlayerDefeated => isPlayerDefeated;
        private bool isLevelCompleted = false;
        public bool IsLevelCompleted => isLevelCompleted;
        private int levelNumber;
        public int LevelNumber => levelNumber;

        private int lives = 3;
        public int Lives => lives;
        private int coins = 0;
        public int Coins => coins;
        private int stars = 0;
        public int Stars => stars;


        public Level(IMapLoader mapLoader, ITilesetLoader tilesetLoader, ILayerLoader layerLoader, ICollisionLoader collisionLoader, IObjectLoader objectLoader, IPlayerLoader playerLoader, IEnemyLoader enemyLoader, IBackgroundLoader backgroundLoader, IInputReader inputReader, ContentLoader contentLoader, int levelNumber)
        {
            this.mapLoader = mapLoader;
            this.tilesetLoader = tilesetLoader;
            this.layerLoader = layerLoader;
            this.collisionLoader = collisionLoader;
            this.objectLoader = objectLoader;
            this.playerLoader = playerLoader;
            this.enemyLoader = enemyLoader;
            this.backgroundLoader = backgroundLoader;
            this.levelNumber = levelNumber;
            this.contentLoader = contentLoader;

            using Stream fileStream = File.OpenRead("Content/Levels/Level" + levelNumber + ".tmx");
            //Loading the map
            map = LoadMap(fileStream);
            //Loading the tilesets
            tilesetTextures = LoadTilesets(map);
            //Loading the layers
            tiles = LoadLayers(map, tilesetTextures);
            //Loading the layer offsets
            layerOffsets = LoadLayerOffsets(map);
            //Loading the collision rectangles
            collisionObjects = LoadCollisionRectangles(map);
            //Loading the game objects
            gameObjects = LoadObjects(map);
            //Loading the player
            player = LoadPlayer(map, inputReader, collisionObjects);
            enemies = LoadEnemies(map, collisionObjects, player);
            backgrounds = LoadBackgrounds();
            exit = LoadExit(map);
            
        }

        private TmxMap LoadMap(Stream fileStream)
        {
            return mapLoader.LoadMap(fileStream);
        }

        private Dictionary<int, Texture2D> LoadTilesets(TmxMap map)
        {
            return tilesetLoader.LoadTilesets(map, contentLoader);
        }

        private Tile[,,] LoadLayers(TmxMap map, Dictionary<int, Texture2D> tilesetTextures)
        {
            return layerLoader.LoadLayers(map, tilesetTextures);
        }

        private Dictionary<int, Vector2>LoadLayerOffsets(TmxMap map)
        {
            return layerLoader.LoadLayerOffsets(map);
        }

        private List<Rectangle> LoadCollisionRectangles(TmxMap map)
        {
            return collisionLoader.LoadCollisionRectangles(map);
        }

        private List<IGameObject> LoadObjects(TmxMap map)
        {
            return objectLoader.LoadObjects(map, contentLoader);
        }

        private Player LoadPlayer(TmxMap map, IInputReader inputReader, List<Rectangle> collisionObjects)
        {
            return playerLoader.LoadPlayer(map, inputReader, collisionObjects, contentLoader);
        }

        private Rectangle LoadExit(TmxMap map)
        {
            TmxObject exitObject = map.ObjectGroups["End"].Objects.First();
            return new Rectangle((int)exitObject.X, (int)exitObject.Y, (int)exitObject.Width, (int)exitObject.Height);
        }

        private List<IEnemy> LoadEnemies(TmxMap map, List<Rectangle> collisionObjects, IMoveAble player)
        {
            return enemyLoader.LoadEnemies(map, collisionObjects, player, contentLoader); ;
        }

        private List<Texture2D> LoadBackgrounds()
        {
            return backgroundLoader.LoadBackgrounds(contentLoader);
        }

        public void HandleObjectCollisions()
        {
            List<IGameObject> objectsToRemove = new();

            foreach (IGameObject gameObject in gameObjects)
            {
                if (player.BoundingRectangle.Intersects(gameObject.BoundingRectangle))
                {
                    // Check the type of the object (Coin or Star)
                    if (gameObject is Coin)
                    {
                        coins++; // Increase coins counter
                    }
                    else if (gameObject is Star)
                    {
                        stars++; // Increase stars counter
                    }
                    else if(gameObject is Heart)
                    {
                        if(lives < 3)
                        {
                            lives++;
                        }
                    }

                    // Mark the object for removal
                    objectsToRemove.Add(gameObject);
                }
            }

            // Remove the collided objects
            foreach (IGameObject objectToRemove in objectsToRemove)
            {
                gameObjects.Remove(objectToRemove);
            }
        }

        public void HandleEnemyCollisions()
        {
            List<IEnemy> enemiesToRemove = new();
            foreach(IEnemy enemy in enemies)
            {
                if(player.BoundingRectangle.Intersects(enemy.BoundingRectangle) && player.InputReader.AnimationState != "Attack")
                {
                    lives--;
                }
                if(enemy.currentAnimationState == "Death")
                {
                    enemiesToRemove.Add(enemy);
                }
            }
            foreach(IEnemy enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
            }
        }

        public void Update(GameTime gameTime)
        {
            HandleObjectCollisions();
            HandleEnemyCollisions();
            foreach (IGameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }
            foreach (IEnemy enemy in enemies)
            {
                enemy.Update(gameTime);
                
            }
            player.Update(gameTime);

            if (player.BoundingRectangle.Intersects(exit))
            {
                isLevelCompleted = true;
            }
            if(lives == 0)
            {
                isPlayerDefeated = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            for (int i = 0; i < backgrounds.Count; i++)
            {
                spriteBatch.Draw(backgrounds[i], new Vector2(0,0) + screenOffset, null, Color.White, 0f, new(0, 0), 1.1f, SpriteEffects.None, 0f);
            }

            for (int z = 0; z < tiles.GetLength(2); z++)
            {
                for (int x = 0; x < tiles.GetLength(0); ++x)
                {
                    for (int y = 0; y < tiles.GetLength(1); y++)
                    {
                        tiles[x, y, z].Draw(spriteBatch, new Vector2(x, y) * Tile.size + layerOffsets[z] + new Vector2(0,map.TileHeight));

                    }
                }

            }

            foreach (IGameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

            foreach(IEnemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
            //objects
            player.Draw(spriteBatch);
        }

        public void Dispose()
        {
            gameObjects.Clear();
            enemies.Clear();
        }
    }
}

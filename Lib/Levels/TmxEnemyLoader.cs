using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Enemies;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class TmxEnemyLoader: IEnemyLoader
    {
        private readonly List<IEnemy> enemies = new();
        public List<IEnemy> LoadEnemies(TmxMap map, List<Rectangle> collisionObjects, IMoveAble player, ContentLoader contentLoader)
        {
            foreach (TmxObject obj in map.ObjectGroups["Enemies"].Objects)
            {
                switch (obj.Name)
                {
                    case "Ghost":
                        LoadEnemiesOfType<Ghost>(obj, position => new Ghost(position, contentLoader, collisionObjects, player));
                        break;
                    case "Skeleton":
                        LoadEnemiesOfType<Skeleton>(obj, position => new Skeleton(position, contentLoader, collisionObjects, player));
                        break;
                    case "Evil_Knight":
                        LoadEnemiesOfType<Evil_Knight>(obj, position => new Evil_Knight(position, contentLoader, collisionObjects, player));
                        break;
                    default:
                        break;
                }
            }
            return enemies;
        }

        public void LoadEnemiesOfType<T>(TmxObject obj, Func<Vector2, IEnemy> objectFactory)
        {
            Vector2 position = new((int)obj.X, (int)obj.Y);
            enemies.Add(objectFactory(position));
        }
    }
}

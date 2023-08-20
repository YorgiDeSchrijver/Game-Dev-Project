using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using GameDevProject.Lib.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class TmxObjectLoader : IObjectLoader
    {
        private readonly List<IGameObject> objects = new();
        private void LoadObjectsOfType<T>(TmxObjectGroup objectGroup, Func<Vector2, IGameObject> objectFactory)
        {
            foreach (TmxObject obj in objectGroup.Objects)
            {
                Vector2 position = new((int)obj.X, (int)obj.Y);
                objects.Add(objectFactory(position));
            }
        }

        public List<IGameObject> LoadObjects(TmxMap map, ContentLoader contentLoader)
        {
            foreach(TmxObjectGroup obj in map.ObjectGroups)
            {
                switch (obj.Name)
                {
                    case "Coins":
                        LoadObjectsOfType<Coin>(obj, position => new Coin(position, contentLoader));
                        break;
                    case "Crystals":
                        LoadObjectsOfType<Crystal>(obj, position => new Crystal(position, contentLoader));
                        break;
                    case "Hearts":
                        LoadObjectsOfType<Heart>(obj, position => new Heart(position, contentLoader));
                        break;
                    case "Stars":
                        LoadObjectsOfType<Star>(obj, position => new Star(position, contentLoader));
                        break;
                    default:
                        break;
                }
            }
            return objects;
             
        }
    }
}

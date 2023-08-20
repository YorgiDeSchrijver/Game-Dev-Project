using GameDevProject.Lib.ContentManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Interfaces
{
    public interface IEnemyLoader
    {
        List<IEnemy> LoadEnemies(TmxMap map, List<Rectangle> collisionObjects, IMoveAble player, ContentLoader contentLoader);
    }
}

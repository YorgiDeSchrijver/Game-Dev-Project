using GameDevProject.Lib.Character;
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
    public interface IPlayerLoader
    {
        Player LoadPlayer(TmxMap map, IInputReader inputReader,List<Rectangle> collisionObject, ContentLoader contentLoader);
    }
}

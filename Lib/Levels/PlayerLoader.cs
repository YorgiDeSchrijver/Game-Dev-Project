using GameDevProject.Lib.Character;
using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    internal class PlayerLoader : IPlayerLoader
    {
        public Player LoadPlayer(TmxMap map, IInputReader inputReader, List<Rectangle> collisionObjects, ContentLoader contentLoader)
        {
            TmxObjectGroup objectGroup = map.ObjectGroups["Start"];
            TmxObject startObject = objectGroup.Objects.First();

            Vector2 startPosition = new((int)startObject.X, (int)startObject.Y);
            Texture2D playerTexture = contentLoader.LoadTexture("Character/Knight/SpriteSheet");
            return new Player(playerTexture, startPosition, inputReader, collisionObjects);
        }
    }
}

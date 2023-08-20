using GameDevProject.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Levels
{
    public class TmxMapLoader:IMapLoader
    {
        public TmxMap LoadMap(Stream fileStream)
        {
            return new TmxMap(fileStream);
        }
    }
}

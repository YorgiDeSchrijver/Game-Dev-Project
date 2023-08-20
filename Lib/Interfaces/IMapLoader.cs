using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Interfaces
{
    public interface IMapLoader
    {
        TmxMap LoadMap(Stream fileStream);
    }
}

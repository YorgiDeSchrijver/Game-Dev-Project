using GameDevProject.Lib.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameDevProject.Lib.Interfaces
{
    public interface IObjectLoader
    {
        List<IGameObject> LoadObjects(TmxMap map, ContentLoader contentLoader);
    }
}

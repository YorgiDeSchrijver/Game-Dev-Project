using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.Interfaces
{
    public interface IInputReader
    {
        public float ReadInput();
        public string AnimationState { get; set; }
        public bool IsJumping { get; set; }
    }
}

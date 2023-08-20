using Microsoft.Xna.Framework;

namespace GameDevProject.Lib.Interfaces
{
    public interface IMoveAble
    {
        Vector2 Position { get; set; }
        IInputReader InputReader { get; }
        Rectangle BoundingRectangle { get; }
        bool IsOnGround { get; set; }

    }
}

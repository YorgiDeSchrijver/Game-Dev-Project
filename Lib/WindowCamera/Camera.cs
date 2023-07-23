using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;

namespace GameDevProject.Lib.WindowCamera
{
    public static class Camera
    {
        public static Matrix Transform { get; private set; }

        public static void Follow(IGameObject target)
        {
            Matrix position;
            float translateX = -target.Position.X - (target.BoundingRectangle.Width / 2) + (Game1.ScreenWidth / 2);
            float translateY = -target.Position.Y - (target.BoundingRectangle.Height / 2) + (Game1.ScreenHeight / 2);
            position = Matrix.CreateTranslation(translateX, translateY, 0);
            Transform = position;
        }
    }
}

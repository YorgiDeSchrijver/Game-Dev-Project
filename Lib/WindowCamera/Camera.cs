using GameDevProject.Lib.Interfaces;
using Microsoft.Xna.Framework;

namespace GameDevProject.Lib.WindowCamera
{
    public static class Camera
    {
        public static Matrix Transform { get; private set; }

        public static void Follow(IMoveAble target)
        {
            Matrix position;
            float translateX = -target.Position.X + (Game1.ScreenWidth / 2);
            float translateY = -target.Position.Y + (Game1.ScreenHeight / 1.5f);
            position = Matrix.CreateTranslation(translateX, translateY, 0);
            Transform = position;
        }

        public static void SetInitialPosition()
        {
            Transform = Matrix.CreateTranslation(0, 0, 0);
        }  
    }
}

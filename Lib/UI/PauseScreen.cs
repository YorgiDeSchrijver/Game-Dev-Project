using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Input;
using GameDevProject.Lib.WindowCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.UI
{
    public class PauseScreen
    {
        //Texture
        private readonly Texture2D pause;

        //Font
        private readonly SpriteFont textFont;

        //Buttons
        private readonly Button continueButton;
        private readonly Button restartButton;
        private readonly Button exitButton;

        //Text
        private readonly string _continue = "Continue";
        private readonly string _restart = "Restart";
        private readonly string _exit = "Exit";

        //Variables
        private bool continueButtonClicked = false;
        public bool ContinueButtonClicked => continueButtonClicked;
        private bool restartButtonClicked = false;
        public bool RestartButtonClicked => restartButtonClicked;
        private bool exitButtonClicked = false;
        public bool ExitButtonClicked => exitButtonClicked;

        public PauseScreen(ContentLoader contentLoader)
        {
            //Textures
            pause = contentLoader.LoadTexture("UI/Pause/Pause");

            //Fonts
            textFont = contentLoader.LoadFont("Fonts/LouisGeorgeCafeBold");

            //Buttons

            continueButton = new(new(397, 257, 167, 36), contentLoader);
            restartButton = new(new(397, 314, 167, 36), contentLoader);
            exitButton = new(new(397, 371, 167, 36), contentLoader);
        }

        public void ResetButtonStates()
        {
            continueButtonClicked = false;
            restartButtonClicked = false;
            exitButtonClicked = false;
        }


        public void Update(Vector2 mousePosition)
        {
            continueButton.Update(mousePosition);
            restartButton.Update(mousePosition);
            exitButton.Update(mousePosition);

            if(MouseReader.ReadInput() == "left")
            {
                if (continueButton.Ishovered)
                {
                    continueButtonClicked = true;
                }

                if (restartButton.Ishovered)
                {
                    restartButtonClicked = true;
                }

                if (exitButton.Ishovered)
                {
                    exitButtonClicked = true;

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            spriteBatch.Draw(pause, new Vector2(363, 124) + screenOffset,null, Color.White, 0f, new(0,0), new Vector2(1.5f,1.5f), SpriteEffects.None, 0f);
            continueButton.Draw(spriteBatch, screenOffset, 1f);
            restartButton.Draw(spriteBatch, screenOffset, 1f);
            exitButton.Draw(spriteBatch, screenOffset, 1f);

            //Continue
            Vector2 middlePointContinue = textFont.MeasureString(_continue) / 2;
            spriteBatch.DrawString(textFont, _continue, new Vector2(480, 275) + screenOffset, Color.Black, 0, middlePointContinue, 0.5f, SpriteEffects.None, 0.5f);
            //Restart
            Vector2 middlePointRestart = textFont.MeasureString(_restart) / 2;
            spriteBatch.DrawString(textFont, _restart, new Vector2(480, 332) + screenOffset, Color.Black, 0, middlePointRestart, 0.5f, SpriteEffects.None, 0.5f);
            //Exit
            Vector2 middlePointExit = textFont.MeasureString(_exit) / 2;
            spriteBatch.DrawString(textFont, _exit, new Vector2(480, 389) + screenOffset, Color.Black, 0, middlePointExit, 0.5f, SpriteEffects.None, 0.5f);
        }
    }
}

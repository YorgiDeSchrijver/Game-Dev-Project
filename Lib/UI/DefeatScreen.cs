using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.UI
{
    public class DefeatScreen
    {
        //Textures
        private readonly Texture2D knight_loose;
        private readonly Texture2D border;
        private readonly Texture2D clock;
        private readonly Texture2D coin;
        private readonly Texture2D star;

        //Fonts
        private readonly SpriteFont titleFont1;
        private readonly SpriteFont titleFont2;
        private readonly SpriteFont titleFont3;
        private readonly SpriteFont textFont;

        //Text
        private readonly string _defeat = "Defeat...";
        private readonly string _restart = "Restart";
        private readonly string _exit = "Exit";

        //Buttons
        private readonly Button restartButton;
        private readonly Button exitButton;

        //Variables
        private bool restartButtonClicked = false;
        public bool RestartButtonClicked => restartButtonClicked;
        private bool exitButtonClicked = false;
        public bool ExitButtonClicked => exitButtonClicked;

        public DefeatScreen(ContentLoader contentLoader)
        {
            knight_loose = contentLoader.LoadTexture("UI/Defeat/knight_loose");
            border = contentLoader.LoadTexture("UI/Defeat/begie_border2");
            clock = contentLoader.LoadTexture("UI/Defeat/clock_yellow");
            coin = contentLoader.LoadTexture("UI/Defeat/coin");
            star = contentLoader.LoadTexture("UI/Defeat/star_small");

            //Fonts
            titleFont1 = contentLoader.LoadFont("Fonts/SmackyFormula");
            titleFont2 = contentLoader.LoadFont("Fonts/SmackyFormulaOut");
            titleFont3 = contentLoader.LoadFont("Fonts/SmackyFormulaSha");
            textFont = contentLoader.LoadFont("Fonts/LouisGeorgeCafeBold");

            //Buttons
            restartButton = new(new(298,380,167,36), contentLoader);
            exitButton = new(new(496,380,167,36), contentLoader);
        }

        public void ResetButtonStates()
        {
            restartButtonClicked = false;
            exitButtonClicked = false;
        }

        public void Update(Vector2 mousePosition)
        {
            restartButton.Update(mousePosition);
            exitButton.Update(mousePosition);

            if (MouseReader.ReadInput() == "left")
            {
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
            spriteBatch.Draw(knight_loose, new Vector2(428,79) + screenOffset, Color.White);
            spriteBatch.Draw(border, new Vector2(377,260) + screenOffset, Color.White);
            spriteBatch.Draw(clock, new Vector2(395,273) + screenOffset, Color.White);
            spriteBatch.Draw(coin, new Vector2(466,275) + screenOffset, Color.White);
            spriteBatch.Draw(star, new Vector2(532,273) + screenOffset, Color.White);
            restartButton.Draw(spriteBatch, screenOffset, 1f);
            exitButton.Draw(spriteBatch, screenOffset, 1f);

            //Victory
            Vector2 middlePoint1 = titleFont1.MeasureString(_defeat) / 2;
            Vector2 middlePoint2 = titleFont2.MeasureString(_defeat) / 2;
            Vector2 middlePoint3 = titleFont3.MeasureString(_defeat) / 2;
            spriteBatch.DrawString(titleFont1, _defeat, new Vector2(480, 220) + screenOffset, Color.DarkOrange, 0, middlePoint1, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont2, _defeat, new Vector2(476, 222) + screenOffset, Color.Black, 0, middlePoint2, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont3, _defeat, new Vector2(480, 222) + screenOffset, Color.Black, 0, middlePoint3, 0.5f, SpriteEffects.None, 0.5f);

            //Continue
            Vector2 middlePointContinue = textFont.MeasureString(_restart) / 2;
            spriteBatch.DrawString(textFont, _restart, new Vector2(382, 398) + screenOffset, Color.Black, 0, middlePointContinue, 0.5f, SpriteEffects.None, 0.5f);

            //Restart
            Vector2 middlePointRestart = textFont.MeasureString(_exit) / 2;
            spriteBatch.DrawString(textFont, _exit, new Vector2(580, 398) + screenOffset, Color.Black, 0, middlePointRestart, 0.5f, SpriteEffects.None, 0.5f);
        }
    }
}

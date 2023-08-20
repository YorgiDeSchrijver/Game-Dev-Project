using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Input;
using GameDevProject.Lib.WindowCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.UI
{
    public class VictoryScreen
    {
        //Textures
        private readonly Texture2D knightAndTape;
        private readonly Texture2D clock;
        private readonly Texture2D coin;
        private readonly Texture2D starSmall;
        private readonly Texture2D border;
        private readonly Texture2D starBig;
        private readonly Texture2D starBigYellow;

        //Fonts
        private readonly SpriteFont titleFont1;
        private readonly SpriteFont titleFont2;
        private readonly SpriteFont titleFont3;
        private readonly SpriteFont textFont;

        //Buttons
        private readonly Button continueButton;
        private readonly Button restartButton;

        //String
        private readonly string _victory = "Victory!";
        private readonly string _continue = "Continue";
        private readonly string _restart = "Restart";

        //Variables
        private bool continueButtonClicked = false;
        public bool ContinueButtonClicked => continueButtonClicked;
        private bool restartButtonClicked = false;
        public bool RestartButtonClicked => restartButtonClicked;

        public VictoryScreen(ContentLoader contentLoader)
        {
            //Textures
            knightAndTape = contentLoader.LoadTexture("UI/Victory/knight+tape");
            clock = contentLoader.LoadTexture("UI/Victory/clock_yellow");
            coin = contentLoader.LoadTexture("UI/Victory/coin");
            starSmall = contentLoader.LoadTexture("UI/Victory/star_small");
            border = contentLoader.LoadTexture("UI/Victory/beige_border2");
            starBig = contentLoader.LoadTexture("UI/Victory/big_star");
            starBigYellow = contentLoader.LoadTexture("UI/Victory/big_star_yellow");

            //Fonts
            titleFont1 = contentLoader.LoadFont("Fonts/SmackyFormula");
            titleFont2 = contentLoader.LoadFont("Fonts/SmackyFormulaOut");
            titleFont3 = contentLoader.LoadFont("Fonts/SmackyFormulaSha");
            textFont = contentLoader.LoadFont("Fonts/LouisGeorgeCafeBold");

            //Buttons
            continueButton = new(new(298, 410, 167, 36), contentLoader);
            restartButton = new(new(496, 410, 167, 36), contentLoader);
        }

        public void ResetButtonStates()
        {
            restartButtonClicked = false;
            continueButtonClicked = false;
        }

        public void Update(Vector2 mousePosition)
        {
            continueButton.Update(mousePosition);
            restartButton.Update(mousePosition);

            if (MouseReader.ReadInput() == "left")
            {
                if (continueButton.Ishovered)
                {
                    continueButtonClicked = true;
                }

                if (restartButton.Ishovered)
                {
                    restartButtonClicked = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            spriteBatch.Draw(knightAndTape, new Vector2(228, 19) + screenOffset, Color.White);
            spriteBatch.Draw(border, new Vector2(377, 292) + screenOffset, Color.White);
            spriteBatch.Draw(clock, new Vector2(465, 306) + screenOffset, Color.White);
            spriteBatch.Draw(coin, new Vector2(532, 308) + screenOffset, Color.White);
            spriteBatch.Draw(starSmall, new Vector2(397, 307) + screenOffset, Color.White);
            continueButton.Draw(spriteBatch, screenOffset, 1f);
            restartButton.Draw(spriteBatch, screenOffset, 1f);
            

            //Victory
            Vector2 middlePoint1 = titleFont1.MeasureString(_victory) / 2;
            Vector2 middlePoint2 = titleFont2.MeasureString(_victory) / 2;
            Vector2 middlePoint3 = titleFont3.MeasureString(_victory) / 2;
            spriteBatch.DrawString(titleFont1, _victory, new Vector2(478, 177) + screenOffset, Color.DarkOrange, 0, middlePoint1, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont2, _victory, new Vector2(474, 179) + screenOffset, Color.Black, 0, middlePoint2, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont3, _victory, new Vector2(478, 179) + screenOffset, Color.Black, 0, middlePoint3, 0.5f, SpriteEffects.None, 0.5f);

            //Continue
            Vector2 middlePointContinue = textFont.MeasureString(_continue) / 2;
            spriteBatch.DrawString(textFont, _continue, new Vector2(381, 428) + screenOffset, Color.Black, 0, middlePointContinue, 0.5f, SpriteEffects.None, 0.5f);

            //Restart
            Vector2 middlePointRestart = textFont.MeasureString(_restart) / 2;
            spriteBatch.DrawString(textFont, _restart, new Vector2(579, 428) + screenOffset, Color.Black, 0, middlePointRestart, 0.5f, SpriteEffects.None, 0.5f);
        }

    }
}

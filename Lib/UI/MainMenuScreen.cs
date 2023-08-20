using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using GameDevProject.Lib.Input;
using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.WindowCamera;

namespace GameDevProject.Lib.UI
{
    public class MainMenuScreen
    {
        //Textures
        private readonly Texture2D background;
        private readonly Texture2D characters;
        private readonly Texture2D platform;

        //Buttons
        private readonly Button startButton;
        private readonly Button exitButton;

        //Fonts
        private readonly SpriteFont titleFont1;
        private readonly SpriteFont titleFont2;
        private readonly SpriteFont titleFont3;
        private readonly SpriteFont textFont;

        //Strings
        private readonly string Title = "Fantasy Platformer";
        private readonly string start = "Play";
        private readonly string exit = "Exit";

        //Variables
        private bool exitButtonClicked = false;
        public bool ExitButtonClicked => exitButtonClicked;
        private bool startButtonClicked = false;
        public bool StartButtonClicked => startButtonClicked;


        public MainMenuScreen(ContentLoader contentLoader)
        {

            //Textures
            background = contentLoader.LoadTexture("UI/Main/bg");
            characters = contentLoader.LoadTexture("UI/Main/characters");
            platform = contentLoader.LoadTexture("UI/Main/platform");

            //Fonts
            titleFont1 = contentLoader.LoadFont("Fonts/SmackyFormula");
            titleFont2 = contentLoader.LoadFont("Fonts/SmackyFormulaOut");
            titleFont3 = contentLoader.LoadFont("Fonts/SmackyFormulaSha");
            textFont = contentLoader.LoadFont("Fonts/LouisGeorgeCafeBold");

            //Buttons
            startButton = new(new(344, 325, 274, 59),contentLoader);
            exitButton = new(new(344, 425, 274, 59), contentLoader);
        }

        public void ResetButtonStates()
        {
            startButtonClicked = false;
            exitButtonClicked = false;
        }

        public void Update(Vector2 mousePosition)
        {
            startButton.Update(mousePosition);
            exitButton.Update(mousePosition);

            if (MouseReader.ReadInput() == "left")
            {
                if (startButton.Ishovered)
                {
                    startButtonClicked = true;
                }
                if (exitButton.Ishovered)
                {
                    exitButtonClicked = true;

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {

            spriteBatch.Draw(background, Vector2.Zero + screenOffset, null, Color.White, 0f, new Vector2(0, 0) , new Vector2(1.07f, 1.07f), SpriteEffects.None, 0f);
            spriteBatch.Draw(characters, new Vector2(276, 21) + screenOffset, Color.White);
            spriteBatch.Draw(platform, new Vector2(322, 229) + screenOffset, Color.White);
            startButton.Draw(spriteBatch, screenOffset, 1.6f);
            exitButton.Draw(spriteBatch, screenOffset, 1.6f);

            //Title
            Vector2 middlePoint1 = titleFont1.MeasureString(Title) / 2;
            Vector2 middlePoint2 = titleFont2.MeasureString(Title) / 2;
            Vector2 middlePoint3 = titleFont3.MeasureString(Title) / 2;
            spriteBatch.DrawString(titleFont1, Title, new Vector2(479, 240) + screenOffset, Color.DarkOrange, 0, middlePoint1, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont2, Title, new Vector2(470, 245) + screenOffset, Color.Black, 0, middlePoint2, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(titleFont3, Title, new Vector2(479, 245) + screenOffset, Color.Black, 0, middlePoint3, 1.0f, SpriteEffects.None, 0.5f);

            //Start
            Vector2 middlePointStart = textFont.MeasureString(start) / 2;
            spriteBatch.DrawString(textFont, start, new Vector2(480, 355) + screenOffset, Color.Black, 0, middlePointStart, 1.0f, SpriteEffects.None, 0.5f);

            //Exit
            Vector2 middlePointExit = textFont.MeasureString(exit) / 2;
            spriteBatch.DrawString(textFont, exit, new Vector2(480, 455) + screenOffset, Color.Black, 0, middlePointExit, 1.0f, SpriteEffects.None, 0.5f);
        }
    }

}

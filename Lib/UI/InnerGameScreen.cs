using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib.UI
{
    public class InnerGameScreen
    {
        //Textures
        private readonly Texture2D heart;
        private readonly Texture2D fullHeart;
        private readonly Texture2D coin;
        private readonly Texture2D door;
        private readonly Texture2D star;

        //Fonts
        private readonly SpriteFont textFont;
        
        //Buttons
        private readonly Button pauseButton;

        


        private bool pauseButtonClicked = false;
        public bool PauseButtonClicked => pauseButtonClicked;

        public InnerGameScreen(ContentLoader contentLoader)
        {
            //Textures
            heart = contentLoader.LoadTexture("UI/Inner/heart");
            fullHeart = contentLoader.LoadTexture("UI/Inner/heart_yellow");
            coin = contentLoader.LoadTexture("UI/Inner/money_yellow");
            door = contentLoader.LoadTexture("UI/Inner/door_yellow");
            star = contentLoader.LoadTexture("UI/Inner/big_star_yellow");

            //Fonts
            textFont = contentLoader.LoadFont("Fonts/LouisGeorgeCafeBold");

            //Buttons
            pauseButton = new(new(901, 20, 39, 51), door);
        }

        public void ResetButtonStates()
        {
            pauseButtonClicked = false;
        }

        public void Update(Vector2 mousePosition)
        {
            pauseButton.Update(mousePosition);

            if (MouseReader.ReadInput() == "left" && pauseButton.Ishovered)
            {
                pauseButtonClicked = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 screenOffset, int hearts, int coins, int stars)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 position = new Vector2(20 + 64 * i, 20);
                if (i < hearts)
                {
                    spriteBatch.Draw(fullHeart,position + screenOffset, null, Color.Red, 0f, new(0,0), new Vector2(1.5f, 1.5f), SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(heart, position + screenOffset, null, Color.White, 0f, new(0, 0), new Vector2(1.5f, 1.5f), SpriteEffects.None, 0f);
                }
            }
            spriteBatch.Draw(coin, new Vector2(20, 80) + screenOffset, null, Color.White, 0f, new(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f) ;
            spriteBatch.Draw(star, new Vector2(20, 140) + screenOffset, null, Color.White, 0f, new(0, 0), new Vector2(0.6f, 0.6f), SpriteEffects.None, 0f );
            pauseButton.Draw(spriteBatch, screenOffset, 1.5f);

            spriteBatch.DrawString(textFont, coins.ToString(), new Vector2(85, 73) + screenOffset, Color.Black);
            spriteBatch.DrawString(textFont, stars.ToString(), new Vector2(85, 133) + screenOffset, Color.Black);
        }
    }
}

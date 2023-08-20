using GameDevProject.Lib.ContentManagement;
using GameDevProject.Lib.Input;
using GameDevProject.Lib.Interfaces;
using GameDevProject.Lib.Levels;
using GameDevProject.Lib.UI;
using GameDevProject.Lib.WindowCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameDevProject.Lib
{
    public class GameManager
    {
        private enum GameState
        {
            MainMenu,
            Playing,
            Defeat,
            Victory,
            Pause,
        }

        private GameState currentState;
        private Level currentLevel;

        // UI
        private readonly MainMenuScreen mainMenuScreen;
        private readonly DefeatScreen defeatScreen;
        private readonly VictoryScreen victoryScreen;
        private readonly PauseScreen pauseScreen;
        private readonly InnerGameScreen innerGameScreen;

        //Level Loaders
        private readonly IMapLoader mapLoader;
        private readonly ITilesetLoader tilesetLoader;
        private readonly ILayerLoader layerLoader;
        private readonly ICollisionLoader collisionLoader;
        private readonly IObjectLoader objectLoader;
        private readonly IPlayerLoader playerLoader;
        private readonly IEnemyLoader enemyLoader;
        private readonly IBackgroundLoader backgroundLoader;

        private readonly ContentLoader contentLoader;
        private readonly ContentManager content;
        private readonly IInputReader inputReader;

        private bool exitButtonClicked = false;
        public bool ExitButtonClicked => exitButtonClicked;

        public GameManager(IServiceProvider services, GraphicsDevice graphics)
        {
            content = new(services, "Content");
            inputReader = new KeyboardReader();
            currentState = GameState.MainMenu;
            contentLoader = new(content);

            mainMenuScreen = new(contentLoader);
            defeatScreen = new(contentLoader);
            victoryScreen = new(contentLoader);
            pauseScreen = new(contentLoader);
            innerGameScreen = new(contentLoader);

            mapLoader = new TmxMapLoader();
            tilesetLoader = new TmxTilesetLoader();
            layerLoader = new TmxLayerLoader(graphics);
            collisionLoader = new TmxCollisionLoader();
            objectLoader = new TmxObjectLoader();
            playerLoader = new PlayerLoader();
            enemyLoader = new TmxEnemyLoader();
            backgroundLoader = new BackgroundLoader();

            Camera.SetInitialPosition();
        }

        public void Update(GameTime gameTime)
        {
            Vector2 mousePosition = new(Mouse.GetState().X, Mouse.GetState().Y);
            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenuScreen.Update(mousePosition);
                    if (mainMenuScreen.StartButtonClicked)
                    {
                        StartLevel(1);
                    }
                    else if (mainMenuScreen.ExitButtonClicked)
                    {
                        exitButtonClicked = true;
                    }
                    
                    break;
                case GameState.Playing:
                    currentLevel.Update(gameTime);
                    innerGameScreen.Update(mousePosition);
                    if (currentLevel.IsPlayerDefeated)
                    {
                        currentState = GameState.Defeat;
                    }
                    else if (currentLevel.IsLevelCompleted)
                    {
                        currentState = GameState.Victory;
                    }
                    if (innerGameScreen.PauseButtonClicked)
                    {
                        currentState = GameState.Pause;
                    }
                    //If pause is clicked =>>>
                    break;
                case GameState.Defeat:
                    defeatScreen.Update(mousePosition);
                    if (defeatScreen.RestartButtonClicked)
                    {
                        StartLevel(currentLevel.LevelNumber);
                    }
                    else if (defeatScreen.ExitButtonClicked)
                    {
                        currentState = GameState.MainMenu;
                    }
                    break;
                case GameState.Victory:
                    victoryScreen.Update(mousePosition);
                    if (victoryScreen.RestartButtonClicked)
                    {
                        StartLevel(currentLevel.LevelNumber);
                    }
                    else if (victoryScreen.ContinueButtonClicked && currentLevel.LevelNumber +1 <= 3)
                    {
                        StartLevel(currentLevel.LevelNumber + 1);
                    }
                    else if (victoryScreen.ContinueButtonClicked && currentLevel.LevelNumber + 1 > 3)
                    {
                        currentState = GameState.MainMenu;
                    }
                    break;
                case GameState.Pause:
                    pauseScreen.Update(mousePosition);
                    if (pauseScreen.RestartButtonClicked)
                    {
                        StartLevel(currentLevel.LevelNumber);
                        
                    }
                    else if (pauseScreen.ContinueButtonClicked)
                    {
                        currentState = GameState.Playing;
                    }
                    else if (pauseScreen.ExitButtonClicked)
                    {
                        currentState = GameState.MainMenu;
                    }
                    break;
            };

            defeatScreen.ResetButtonStates();
            victoryScreen.ResetButtonStates();
            pauseScreen.ResetButtonStates();
            innerGameScreen.ResetButtonStates();
            mainMenuScreen.ResetButtonStates();
        }

        private void StartLevel(int levelNumber)
        {
            if (currentLevel != null) currentLevel.Dispose();
            currentLevel = new Level(mapLoader, tilesetLoader, layerLoader, collisionLoader, objectLoader, playerLoader, enemyLoader, backgroundLoader, inputReader, contentLoader, levelNumber);
            currentState = GameState.Playing;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 screenOffset = Vector2.Transform(Vector2.Zero, Matrix.Invert(Camera.Transform));
            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenuScreen.Draw(spriteBatch, screenOffset);
                    break;
                case GameState.Playing:
                    currentLevel.Draw(spriteBatch, screenOffset);
                    innerGameScreen.Draw(spriteBatch, screenOffset, currentLevel.Lives, currentLevel.Coins, currentLevel.Stars); //Game UI
                    break;
                case GameState.Defeat:
                    currentLevel.Draw(spriteBatch, screenOffset);
                    defeatScreen.Draw(spriteBatch, screenOffset); 
                    break;
                case GameState.Victory:
                    currentLevel.Draw(spriteBatch, screenOffset);
                    victoryScreen.Draw(spriteBatch, screenOffset);
                    break;
                case GameState.Pause:
                    currentLevel.Draw(spriteBatch, screenOffset);
                    pauseScreen.Draw(spriteBatch, screenOffset);
                    break;
            } ;
        }

    }
}

using System;
using SadConsole;
using Console = SadConsole.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GeradorDeMapaConceito
{
    public class GameLoop
    {
        public const int GameWidth = 120;
        public const int GameHeight = 30;

        public static UIManager UIManager;

        public static void Main()
        {
            // Setup the engine and creat the main window.
            SadConsole.Game.Create(GameWidth, GameHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            //Start the game.
            SadConsole.Game.Instance.Run();

            // Code here will not run until the game window closes.
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            // Any startup code for your game. We will use an example console for now
            UIManager = new UIManager();

            // Initiate the map

            UIManager.Init();
        }
    }
}
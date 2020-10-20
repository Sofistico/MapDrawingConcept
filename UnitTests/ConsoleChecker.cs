using System;
using Xunit;
using GeradorDeMapaConceito;
using System.Linq;
using SadConsole.Input;
using SadConsole;

namespace UnitTests
{
    public class ConsoleChecker
    {
        private const int width = GameLoop.GameWidth;
        private const int height = GameLoop.GameHeight;
        public MouseConsoleState State { get; set; }

        [Fact]
        public void CheckWhyReturnsNull()
        {
            NewConsole();

            /*SadConsole.Controls.ControlBase goRogueControl = GameLoop.UIManager.MainMenu.Controls.Last();
            goRogueControl.ProcessMouse(State);*/
        }

        private void NewConsole()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create(80, 25);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            var console = new ScrollingConsole(width, height);
            SadConsole.Global.CurrentScreen = console;
        }
    }
}
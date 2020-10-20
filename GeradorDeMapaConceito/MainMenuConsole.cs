using SadConsole;
using System;
using Microsoft.Xna.Framework;
using SadConsole.Controls;
using SadConsole.Themes;

namespace GeradorDeMapaConceito
{
    public class MainMenuConsole : Window
    {
        //private Console menu;

        public MainMenuConsole(int width, int height, string title = "Main Menu") : base(width, height)
        {
            Title = title.Align(HorizontalAlignment.Center, Width);

            Button startGameSadConsole = new Button(23, 1)
            {
                Position = new Point(width / 2, height / 2),
                Text = "Create SadConsole Map",
                Name = "Sad Map",
                ThemeColors = Colors.CreateAnsi()
            };
            startGameSadConsole.Click += StartGameSadConsole_Click;
            Add(startGameSadConsole);

            Button startGameGoRogue = new Button(20, 1)
            {
                Position = new Point(width / 3, height / 3),
                Text = "Create GoRogue Map",
                Name = "GoRogue Map",
                ThemeColors = Colors.CreateAnsi()
            };

            startGameGoRogue.Click += StartGameGoRogue_Click;

            Add(startGameGoRogue);
        }

        private void StartGameGoRogue_Click(object sender, EventArgs e)
        {
            GameLoop.UIManager.CreateMapGoRogue(GameLoop.GameWidth, GameLoop.GameHeight);
        }

        private void StartGameSadConsole_Click(object sender, EventArgs e)
        {
            GameLoop.UIManager.CreateMapSadConsole(GameLoop.GameWidth, GameLoop.GameHeight);
        }
    }
}
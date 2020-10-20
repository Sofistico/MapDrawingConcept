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

            Button startGame = new Button(12, 1);
            startGame.Position = new Point(width / 2, height / 2);
            startGame.Text = "Start Game";
            startGame.ThemeColors = Colors.CreateAnsi();
            startGame.Click += StartGame_Click;
            Add(startGame);
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            GameLoop.UIManager.CreateMap(GameLoop.GameWidth, GameLoop.GameHeight);
        }
    }
}
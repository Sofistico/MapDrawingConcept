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
            Button startGame = new Button(12, 1);
            startGame.Position = new Point(width / 2, height / 2);
            //startGame.Name = "Start Game";
            startGame.Text = "Start Game";
            startGame.TextAlignment = HorizontalAlignment.Center;
            startGame.ThemeColors = Colors.CreateAnsi();
            startGame.Click += StartGame_Click;
            //startGame.DoClick();
            Add(startGame);
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            GameLoop.UIManager.CreateMap(GameLoop.GameWidth, GameLoop.GameHeight);
        }
    }
}
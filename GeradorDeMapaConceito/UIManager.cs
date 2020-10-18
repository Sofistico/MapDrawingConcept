using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SadConsole;
using Microsoft.Xna.Framework.Input;
using SadConsole.Entities;
using SadConsole.Input;
using SadConsole.Components;
using GoRogue;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace GeradorDeMapaConceito
{
    public class UIManager : ContainerConsole
    {
        public ScrollingConsole MapConsole;
        public MainMenuConsole MainMenu;
        private TileBase[] tiles;
        private Player player;
        private const int maxRooms = 6;
        private const int maxSize = 10;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;
            //UseMouse = true;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = Global.CurrentScreen;
        }

        public void Init()
        {
            MainMenu = new MainMenuConsole(GameLoop.GameWidth, GameLoop.GameHeight);
            Children.Add(MainMenu);
            MainMenu.Show();

            MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight);
            /*MapConsole = new ScrollingConsole(GameLoop.GameWidth, GameLoop.GameHeight);
            MapConsole.IsVisible = false;*/

            //CreateMap(GameLoop.GameWidth, GameLoop.GameHeight);
        }

        private void MapConsole_MouseMove(object sender, MouseEventArgs e)
        {
            var console = (ScrollingConsole)sender;
            if (e.MouseState.Mouse.LeftButtonDown)
            {
                // The proper way of doing it is by clearing the cell that the mouse is over.
                console.Clear(e.MouseState.CellPosition.X, e.MouseState.CellPosition.Y);
                int mouseLocation = Helpers.GetIndexFromPoint(e.MouseState.CellPosition.X,
                    e.MouseState.CellPosition.Y, MapConsole.Width);
                e.MouseState.Cell.CopyAppearanceFrom(tiles[mouseLocation] = new TileWall());
            }
            if (e.MouseState.Mouse.RightButtonDown && !Global.KeyboardState.IsKeyDown(Keys.LeftShift))
            {
                int mouseLocation = Helpers.GetIndexFromPoint(e.MouseState.CellPosition.X,
                    e.MouseState.CellPosition.Y, MapConsole.Width);
                if (!tiles[mouseLocation].isTileWalkable)
                    AddBasicMob(mouseLocation);
            }
            if (Global.KeyboardState.IsKeyDown(Keys.LeftShift) && e.MouseState.Mouse.RightButtonDown)
            {
                int mouseLocation = Helpers.GetIndexFromPoint(e.MouseState.CellPosition.X,
                   e.MouseState.CellPosition.Y, MapConsole.Width);
                if (!tiles[mouseLocation].isTileWalkable)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        AddBasicMob(mouseLocation);
                    }
                }
            }
        }

        private void AddPlayer()
        {
            player = new Player();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (!tiles[i].isTileWalkable)
                {
                    player.Position = Helpers.GetPointFromIndex(i, MapConsole.Width);
                }
            }
            MapConsole.Children.Add(player);
            player.IsVisible = true;
        }

        private void AddBasicMob(int indexOfTile)
        {
            BasicMob mob = new BasicMob();
            mob.Position = Helpers.GetPointFromIndex(indexOfTile, MapConsole.Width);
            //MapConsole.Children.Add(mob);
            MapConsole.Children.Insert(MapConsole.Children.Count, mob);
            mob.IsVisible = true;
        }

        private bool MoveBy(Point positionChange, Player player)
        {
            if (player != null)
            {
                if (IsTileWalkable(player.Position + positionChange))
                {
                    player.Position += positionChange;
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        private bool IsTileWalkable(Point location)
        {
            // first make sure that actor isn't trying to move
            // off the limits of the map
            if (location.X < 0 || location.Y < 0 || location.X >= MapConsole.Width || location.Y >= MapConsole.Height)
                return false;
            return !tiles[location.Y * MapConsole.Width + location.X].isTileWalkable;
        }

        public void CreateMap(int width, int height)
        {
            MainMenu.Hide();
            // Creates a empty map console to not get any errors, to them populate
            MapConsole = new ScrollingConsole(width, height);

            FloodFloors();
            MakeWalls();

            MapConsole = new ScrollingConsole(width, height, Global.FontDefault
                , new Rectangle(0, 0, width, height), tiles);
            UseMouse = true;
            Children.Add(MapConsole);
            MapConsole.MouseMove += MapConsole_MouseMove;
        }

        private void FloodFloors()
        {
            tiles = new TileBase[GameLoop.GameWidth * GameLoop.GameHeight];

            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new TileFloor();
            }
        }

        private void MakeWalls()
        {
            /*int firstPointOfPerimeter = 0;
            int secondPointOfPerimeter = 119;
            int thirdPointOfPerimeter = 3599 - 119;
            int fourthPointOfPerimeter = 3599;*/

            // formula x*m + y, calculo de row major
            for (int leftY = 0; leftY < MapConsole.Height; leftY++)
            {
                tiles[leftY * MapConsole.Width + 0] = new TileWall();
                for (int rightY = 0; rightY < MapConsole.Height; rightY++)
                {
                    tiles[rightY * MapConsole.Width + 119] = new TileWall();
                    for (int topX = 0; topX < MapConsole.Width; topX++)
                    {
                        tiles[0 * MapConsole.Width + topX] = new TileWall();
                        for (int bottomX = 0; bottomX < MapConsole.Width; bottomX++)
                        {
                            tiles[0 * MapConsole.Width + bottomX + 3480] = new TileWall();
                        }
                    }
                }
            }
        }

        public override void Update(TimeSpan timeElapsed)
        {
            ProcessKeyboard();

            base.Update(timeElapsed);
        }

        private void ProcessKeyboard()
        {
            if (Global.KeyboardState.IsKeyPressed(Keys.Escape))
                SadConsole.Game.Instance.Exit();
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad8))
                MoveBy(new Point(0, -1), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad2))
                MoveBy(new Point(0, 1), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad4))
                MoveBy(new Point(-1, 0), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad6))
                MoveBy(new Point(1, 0), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad7))
                MoveBy(new Point(-1, -1), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad9))
                MoveBy(new Point(1, -1), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad1))
                MoveBy(new Point(-1, 1), player);
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad3))
                MoveBy(new Point(1, 1), player);
            if (Global.KeyboardState.IsKeyDown(Keys.LeftShift) && Global.KeyboardState.IsKeyPressed(Keys.P))
                AddPlayer();
        }
    }

    public abstract class TileBase : Cell
    {
        public bool isTileWalkable;
        public bool isBlockingVision;

        public TileBase(Color foreground, Color background, int glyph, bool blocksMove = false, bool blocksVision = false)
        : base(foreground, background, glyph)
        {
            isTileWalkable = blocksMove;
            isBlockingVision = blocksVision;
        }
    }

    public class TileFloor : TileBase
    {
        public TileFloor(bool blocksMove = false, bool blocksVision = false) : base(Color.DarkGray, Color.Transparent, '.', blocksMove, blocksVision)
        {
        }
    }

    public class TileWall : TileBase
    {
        public TileWall(bool blocksMove = true, bool blocksVision = true) : base(Color.LightGray, Color.Transparent, '#', blocksMove, blocksVision)
        {
        }
    }

    public class Player : Actor
    {
        public Player() : base(Color.White, Color.Transparent, '@')
        {
        }
    }

    public class BasicMob : Actor
    {
        public BasicMob() : base(Color.Green, Color.Transparent, 'g')
        {
        }
    }

    public abstract class Actor : Entity
    {
        public Actor(Color foreground, Color background, int glyph, int height = 1, int width = 1) :
            base(foreground, background, glyph, height, width)
        {
        }
    }

    public abstract class Entity : SadConsole.Entities.Entity
    {
        public Entity(Color foreground, Color background, int glyph, int height, int width) : base(height, width)
        {
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;

            //Components.Add(new EntityViewSyncComponent());
        }
    }
}
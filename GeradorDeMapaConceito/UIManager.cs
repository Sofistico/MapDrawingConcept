﻿using System;
using Microsoft.Xna.Framework;
using SadConsole;
using Microsoft.Xna.Framework.Input;
using SadConsole.Input;
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
                if (!tiles[mouseLocation].IsTileWalkable)
                    AddBasicMob(mouseLocation);
            }
            if (Global.KeyboardState.IsKeyDown(Keys.LeftShift) && e.MouseState.Mouse.RightButtonDown)
            {
                // Made to stress test how many entities it holds
                int mouseLocation = Helpers.GetIndexFromPoint(e.MouseState.CellPosition.X,
                   e.MouseState.CellPosition.Y, MapConsole.Width);
                if (!tiles[mouseLocation].IsTileWalkable)
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
                if (!tiles[i].IsTileWalkable)
                {
                    player.Position = Helpers.GetPointFromIndex(i, MapConsole.Width);
                }
            }
            MapConsole.Children.Add(player);
            player.IsVisible = true;
        }

        private void AddBasicMob(int indexOfTile)
        {
            BasicMob mob = new BasicMob
            {
                Position = Helpers.GetPointFromIndex(indexOfTile, MapConsole.Width)
            };
            //MapConsole.Children.Add(mob);
            MapConsole.Children.Insert(MapConsole.Children.Count, mob);
            //mob.IsVisible = true;
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
            return !tiles[location.Y * MapConsole.Width + location.X].IsTileWalkable;
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
            // formula x*m + y, calculo de row major
            for (int y = 0; y < MapConsole.Height; y++)
            {
                for (int x = 0; x < MapConsole.Width; x++)
                {
                    if (x == 0 || y == 0 || x == MapConsole.Width - 1 || y == MapConsole.Height - 1)
                    {
                        tiles[y * MapConsole.Width + x] = new TileWall();
                    }
                }
            }
        }

        public override bool ProcessMouse(MouseConsoleState state)
        {
            return base.ProcessMouse(state);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
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

            return base.ProcessKeyboard(info);
        }
    }

    internal enum MapLayer
    {
        TERRAIN,
        ENTITY
    }

    public abstract class TileBase : Cell
    {
        public bool IsTileWalkable;
        public bool IsBlockingVision;
        public int Layer;

        public TileBase(Color foreground, Color background, int glyph, int layer, bool blocksMove = false, bool blocksVision = false)
        : base(foreground, background, glyph)
        {
            IsTileWalkable = blocksMove;
            IsBlockingVision = blocksVision;
            Layer = layer;
        }
    }

    public class TileFloor : TileBase
    {
        public TileFloor(bool blocksMove = false, bool blocksVision = false) : base(Color.DarkGray, Color.Transparent, '.', (int)MapLayer.TERRAIN, blocksMove, blocksVision)
        {
        }
    }

    public class TileWall : TileBase
    {
        public TileWall(bool blocksMove = true, bool blocksVision = true) : base(Color.LightGray, Color.Transparent, '#', (int)MapLayer.TERRAIN, blocksMove, blocksVision)
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

    public abstract class Actor : SadConsole.Entities.Entity
    {
        public Actor(Color foreground, Color background, int glyph, int layer = (int)MapLayer.ENTITY, int height = 1, int width = 1) :
            base(height, width)
        {
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
        }
    }

    /*public abstract class Entity : SadConsole.Entities.Entity
    {
        public Entity(Color foreground, Color background, int glyph, int height, int width, int layer) : base(height, width)
        {
            //Components.Add(new EntityViewSyncComponent());
        }
    }*/
}
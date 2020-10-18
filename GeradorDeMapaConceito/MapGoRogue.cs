using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;

namespace GeradorDeMapaConceito
{
    public class MapGoRogue : UIManager
    {
        public ScrollingConsole MapRender;
        public MapGeneratorGoRogue MapGen;

        public MapGoRogue(int width, int height)
        {
            MapGen = GenerateMap(width, height);

            Children.Add(MapRender);
        }

        private MapGeneratorGoRogue GenerateMap(int mapWidth, int mapHeight)
        {
            // Generate map via GoRogue, and update the real map with appropriate terrain.
            MapGeneratorGoRogue map = new MapGeneratorGoRogue(mapWidth, mapHeight);

            ArrayMap<bool> tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRectangleMap(tempMap);

            return map;
        }

        public void GenerateFloor()
        {
        }
    }
}
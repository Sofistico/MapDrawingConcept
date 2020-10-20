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
    // For now its not used.
    public class MapGoRogue : UIManager
    {
        public MapGeneratorGoRogue MapGen;
        private ArrayMap<bool> tempMap;

        public MapGoRogue(int width, int height)
        {
            MapGen = GenerateMap(width, height);

            Children.Add(GameLoop.UIManager.MapConsole);
        }

        private IGameObject TempMapValueToTile(Coord pos, bool val) => val ? new TileFloor() : new TileWall();

        private MapGeneratorGoRogue GenerateMap(int mapWidth, int mapHeight)
        {
            // Generate map via GoRogue, and update the real map with appropriate terrain.
            MapGeneratorGoRogue map = new MapGeneratorGoRogue(mapWidth, mapHeight);

            tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateRectangleMap(tempMap);

            // Update real map with tiles (using a GoRogue helper method)
            map.ApplyTerrainOverlay(tempMap, TempMapValueToTile);

            return map;
        }

        public void GenerateFloor()
        {
        }
    }
}
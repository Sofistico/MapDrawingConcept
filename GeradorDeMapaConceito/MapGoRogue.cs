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
    // This uses the core feature of GoRogue
    public class MapGoRogue : UIManager
    {
        private ArrayMap<bool> tempMap;

        public MapGoRogue(int width, int height)
        {
            GenerateMap(width, height);
        }

        private TileBase TempMapValueToTile(Coord pos, bool val) => val ? new TileFloor() : new TileWall();

        private void GenerateMap(int mapWidth, int mapHeight)
        {
            // Create "real" map, or if this has already been created by this point there's nothing to do here
            //GameLoop.UIManager.tileBase = new ArrayMap<TileBase>(mapWidth, mapHeight);

            // Generate temp map via GoRogue
            tempMap = new ArrayMap<bool>(mapWidth, mapHeight);
            QuickGenerators.GenerateRectangleMap(tempMap);

            // Update real map with tiles (using a GoRogue helper method)
            GameLoop.UIManager.tileBase.ApplyOverlay(new LambdaTranslationMap<bool, TileBase>(tempMap, TempMapValueToTile));
        }

        public void GenerateFloor()
        {
        }
    }
}
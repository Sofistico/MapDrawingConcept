using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using SadConsole;
using SadConsole.Input;

namespace GeradorDeMapaConceito
{
    // This uses the core feature of GoRogue
    public class MapGoRogue
    {
        public ArrayMap<bool> tempMap;

        // Constructor to create the map when it is called by the map gen aspect of uimanager
        public MapGoRogue(int width, int height)
        {
            GenerateMap(width, height);
        }

        // Function that translates the values from tempMap to the real map
        private TileBase TempMapValueToTile(Coord pos, bool val) => val ? (TileBase)new TileFloor() : new TileWall();

        private void GenerateMap(int mapWidth, int mapHeight)
        {
            // Create "real" map, or if this has already been created by this point there's nothing to do here
            GameLoop.UIManager.tileBase = new ArrayMap<TileBase>(mapWidth, mapHeight);

            // Generate temp map via GoRogue
            tempMap = new ArrayMap<bool>(mapWidth, mapHeight);
            QuickGenerators.GenerateRectangleMap(tempMap);

            // Update real map with tiles (using a GoRogue helper method)
            GameLoop.UIManager.tileBase.ApplyOverlay(new LambdaTranslationMap<bool, TileBase>(tempMap, TempMapValueToTile));
        }
    }
}
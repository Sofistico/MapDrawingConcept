using System;
using System.Collections.Generic;
using System.Text;
using GoRogue;
using GoRogue.GameFramework;
using SadConsole;

namespace GeradorDeMapaConceito
{
    // For now its not used.
    public class MapGeneratorGoRogue : Map
    {
        public enum MapLayer
        {
            TERRAIN,
            FURNITURE,
            ITEMS,
            ACTORS,
            PLAYER
        }

        private readonly Random rnd = new Random();

        public MapGeneratorGoRogue(int widht, int height)
            : base(widht, height, Enum.GetNames(typeof(MapLayer)).Length - 1,
                  Distance.CHEBYSHEV,
                  entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS))
        {
        }
    }
}
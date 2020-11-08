using GoRogue;
using GoRogue.Pathing;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;

namespace GeradorDeMapaConceito
{
    public class Pathfinder
    {
        private readonly IMapView<bool> nodes; // IMap view that contains all nodes, way better than storing it in an array

        /*// F is just g + h, the cost to move to a tile
         private int f;
         // Distance from the starting point
         private int g;
         // Estimated distance from the desired spot
         private int h;*/

        public FastAStar Path { get; }

        // The constructor inserts all current tiles in the World to the list.
        public Pathfinder(IMapView<bool> walkabilityMap)
        {
            nodes = walkabilityMap;
            Path = new FastAStar(nodes, Distance.CHEBYSHEV);
        }

        public void AStarPathfindingTest(Coord target, Actor mob)
        {
            if (mob != null)
            {
                Path pathToItem = Path.ShortestPath(mob.Position, target);

                FollowPath(pathToItem, mob);
            }
        }

        private bool FollowPath(Path path, Actor actor)
        {
            if (path != null)
            {
                foreach (Coord coord in path.Steps)
                {
                    if (GameLoop.UIManager.IsTileWalkable(coord))
                    {
                        // Sort of works, but it doesn't take in account the terrain
                        actor.Position = new Coord(coord.X, coord.Y);
                        //GameLoop.UIManager.MoveBy(coord, actor);
                        return true;
                    }
                    else
                        return false;
                }
            }

            return false;
        }

        public void FollowPlayer(Coord target, Actor mob)
        {
            if (mob != null)
            {
                Path path = Path.ShortestPath(mob.Position, target);
                FollowPath(path, mob);
            }
        }
    }
}
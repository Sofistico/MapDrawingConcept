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
        private ArrayMap<bool> nodes; // thought for the future, contains all tiles that are walkable.

        /*// F is just g + h, the cost to move to a tile
         private int f;
         // Distance from the starting point
         private int g;
         // Estimated distance from the desired spot
         private int h;*/

        public AStar Path { get; }

        // The constructor inserts all current tiles in the World to the list.
        public Pathfinder(ArrayMap<bool> walkabilityMap)
        {
            nodes = walkabilityMap;
            Path = new AStar(nodes, Distance.CHEBYSHEV);
        }

        // Helper method to calculate the h score
        /*private int CalculateH(Point x, Point y, Point targetX, Point targetY)
        {
            return Math.Abs(targetX.X - x.X) + Math.Abs(targetY.Y - y.Y);
        }*/

        public void AStarPathfindingTest(Coord target, Actor mob)
        {
            // I cant seem to make it work, fuck this shit.
            //var target = GameLoop.UIManager.GetEntityAt<Item>(item.Position);
            //var actor = GameLoop.UIManager.GetEntityAt<Actor>(mob.Position);
            if (mob != null)
            {
                Path pathToItem = Path.ShortestPath(mob.Position, target);

                FollowPath(pathToItem, mob);
            }
            //MoveTowards(pathToItem, actor);
        }

        public bool FollowPath(Path path, Actor actor)
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
            return false;
        }

        public void MoveActor(Actor actor, Coord coord)
        {
        }

        public bool MoveTo(Coord newPos, Actor actor)
        {
            actor.Position = newPos;
            return true;
        }
    }

    /*// Node representing a grid position in AStar's priority queue
    internal class AStarNode : FastPriorityQueueNode
    {
        public readonly Coord Position;

        // Whether or not the node has been closed
        public float F;

        // (Partly estimated) distance to end point going thru this node
        public float G;

        public AStarNode Parent; // (Known) distance from start to this node, by shortest known path

        public AStarNode(Coord position, AStarNode parent = null)
        {
            Parent = parent;
            Position = position;
            F = G = float.MaxValue;
        }
    }

    // Encapsulates a path as returned by pathfinding algorithms like AStar.
    // Provides various functions to iterate through/access steps of the path, as well as
    // constant-time reversing functionality.
    public class Path
    {
        private IReadOnlyList<Coord> stepsTaken;
        private bool inOriginalOrder;

        //Creates a copy of the path, optionally reversing the path as it does so.
        public Path(Path pathToCopy, bool reverse = false)
        {
            stepsTaken = pathToCopy.stepsTaken;
            inOriginalOrder = (reverse ? !pathToCopy.inOriginalOrder : pathToCopy.inOriginalOrder);
        }

        // Create based on internal list
        internal Path(IReadOnlyList<Coord> steps)
        {
            stepsTaken = steps;
            inOriginalOrder = true;
        }

        // Ending point of path
        public Coord End
        {
            get
            {
                if (inOriginalOrder)
                    return stepsTaken[0];
                return stepsTaken[stepsTaken.Count - 1];
            }
        }

        // The length of the path, NOT including the starting point.
        public int Length { get => stepsTaken.Count - 1; }

        // The length of the path, INCLUDING the starting point.
        public int LengthWithStart { get => stepsTaken.Count; }

        // Starting point of path
        public Coord Start
        {
            get
            {
                if (inOriginalOrder)
                    return stepsTaken[stepsTaken.Count - 1];
                return stepsTaken[0];
            }
        }

        // The coordinates that constitute the path (in order), NOT including the starting point.
        // These are the coordinates something might walk along to follow a path.
        public IEnumerable<Coord> Steps
        {
            get
            {
                if (inOriginalOrder)
                {
                    for (int i = stepsTaken.Count - 2; i >= 0; i--)
                        yield return stepsTaken[i];
                }
                else
                {
                    for (int i = 1; i < stepsTaken.Count; i++)
                    {
                        yield return stepsTaken[i];
                    }
                }
            }
        }

        /// <summary>
		/// The coordinates that constitute the path (in order), INCLUDING the starting point.
		/// </summary>
		public IEnumerable<Coord> StepsWithStart
        {
            get
            {
                if (inOriginalOrder)
                {
                    for (int i = stepsTaken.Count - 1; i >= 0; i--)
                        yield return stepsTaken[i];
                }
                else
                {
                    for (int i = 0; i < stepsTaken.Count; i++)
                        yield return stepsTaken[i];
                }
            }
        }

        /// <summary>
        /// Gets the nth step along the path, where 0 is the step AFTER the starting point.
        /// </summary>
        /// <param name="stepNum">The (array-like index) of the step to get.</param>
        /// <returns>The coordinate consituting the step specified.</returns>
        public Coord GetStep(int stepNum)
        {
            if (inOriginalOrder)
                return stepsTaken[(stepsTaken.Count - 2) - stepNum];

            return stepsTaken[stepNum + 1];
        }

        /// <summary>
        /// Gets the nth step along the path, where 0 IS the starting point.
        /// </summary>
        /// <param name="stepNum">The (array-like index) of the step to get.</param>
        /// <returns>The coordinate consituting the step specified.</returns>
        public Coord GetStepWithStart(int stepNum)
        {
            if (inOriginalOrder)
                return stepsTaken[(stepsTaken.Count - 1) - stepNum];

            return stepsTaken[stepNum];
        }

        /// <summary>
        /// Reverses the path, in constant time.
        /// </summary>
        public void Reverse() => inOriginalOrder = !inOriginalOrder;

        /// <summary>
        /// Returns a string representation of all the steps in the path, including the start point,
        /// eg. [(1, 2), (3, 4), (5, 6)].
        /// </summary>
        /// <returns>A string representation of all steps in the path, including the start.</returns>
        public override string ToString() => StepsWithStart.ExtendToString();
    }*/
}
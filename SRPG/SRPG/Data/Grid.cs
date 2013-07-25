using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SRPG.Data
{
    public struct Grid
    {
        /// <summary>
        /// A length and width value indicating the size of the grid being processed.
        /// </summary>
        public Rectangle Size;
        /// <summary>
        /// A two dimensional array, whose size corresponds to Size.Width and Size.Height, applying a weight value to each
        /// square of the grid. This weight is arbitrary and can be used for movement grids, splash damage weighting, or
        /// anything else that the caller desires.
        /// </summary>
        public byte[,] Weight;

        public Grid(int x, int y, byte defaultValue = 0)
        {
            Size = new Rectangle(0, 0, x, y);
            Weight = new byte[x, y];

            for(var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    Weight[i, j] = defaultValue;
                }
            }
        }

        public static Grid FromBitmap(string bitmapName)
        {
            var texture = Torch.Game.GetInstance().Content.Load<Texture2D>(bitmapName);
            var grid = new Grid(texture.Width, texture.Height);

            for (var i = 0; i < grid.Size.Width; i++)
            {
                for (var j = 0; j < grid.Size.Height; j++)
                {
                    var c = new Color[1];
                    texture.GetData(0, new Rectangle(i, j, 1, 1), c, 0, 1);
                    grid.Weight[i, j] = (byte)((c[0].R + c[0].G + c[0].B)/3);
                }
            }

            return grid;
        }

        /// <summary>
        /// A* algorithm for calculating a path between two points on a grid.
        /// </summary>
        /// <param name="start">The start point to begin calculation.</param>
        /// <param name="end">The destination to seek out.</param>
        /// <returns>A list of points to move the character through to reach the destination in the shortest possible distance.</returns>
        public List<Point> Pathfind(Point start, Point end)
        {
            var closedSet = new List<Point>();
            var openSet = new List<Point> { start };
            var cameFrom = new Dictionary<Point, Point>();
            var currentDistance = new Dictionary<Point, int>();
            var predictedDistance = new Dictionary<Point, float>();

            currentDistance.Add(start, 0);
            predictedDistance.Add(start, 0 + +Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y));

            while (openSet.Count > 0)
            {
                // get the node with the lowest estimated cost to finish
                var current = (from p in openSet orderby predictedDistance[p] ascending select p).First();

                // if it is the finish, return the path
                if (current.X == end.X && current.Y == end.Y)
                {
                    // generate the found path
                    return ReconstructPath(cameFrom, end);
                }

                // move current node from open to closed
                openSet.Remove(current);
                closedSet.Add(current);

                // process each valid node around the current node
                foreach (var neighbor in GetNeighborNodes(current))
                {
                    var tempCurrentDistance = currentDistance[current] + 1;

                    // if we already know a faster way to this neighbor, use that route and ignore this one
                    if (closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor])
                    {
                        continue;
                    }

                    // if we don't know a route to this neighbor, or if this is faster, store this route
                    if (!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if (cameFrom.Keys.Contains(neighbor))
                        {
                            cameFrom[neighbor] = current;
                        }
                        else
                        {
                            cameFrom.Add(neighbor, current);
                        }

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] = currentDistance[neighbor] + Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y);

                        // if this is a new node, add it to processing
                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            throw new Exception(string.Format("unable to find a path between {0},{1} and {2},{3}", start.X, start.Y, end.X, end.Y));
        }

        /// <summary>
        /// Return a list of accessible nodes neighboring a specified node, taking faction into account.
        /// </summary>
        /// <param name="node">The center node to be analyzed.</param>
        /// <returns>A list of nodes neighboring the center node that a character of the specified faction may enter.</returns>
        private IEnumerable<Point> GetNeighborNodes(Point node)
        {
            var nodes = new List<Point>();

            // up
            if (Weight[node.X, node.Y - 1] > 0)
            {
                nodes.Add(new Point(node.X, node.Y - 1));
            }

            // right
            if (Weight[node.X + 1, node.Y] > 0)
            {
                nodes.Add(new Point(node.X + 1, node.Y));
            }

            // down
            if (Weight[node.X, node.Y + 1] > 0)
            {
                nodes.Add(new Point(node.X, node.Y + 1));
            }

            // left
            if (Weight[node.X - 1, node.Y] > 0)
            {
                nodes.Add(new Point(node.X - 1, node.Y));
            }

            return nodes;
        }

        /// <summary>
        /// Process a list of valid paths generated by the Pathfind function and return a coherent path to current.
        /// </summary>
        /// <param name="cameFrom">A list of nodes and the optimal origin to that node.</param>
        /// <param name="current">The destination node being sought out.</param>
        /// <returns>The shortest path possible from the start to the destination node.</returns>
        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            if (!cameFrom.Keys.Contains(current))
            {
                return new List<Point> { current };
            }

            var path = ReconstructPath(cameFrom, cameFrom[current]);
            path.Add(current);
            return path;
        }

        public Grid OverlayGridFromCenter(Grid overlay, Point center)
        {
            var grid = Clone();
            
            for(var x = 0; x < overlay.Size.Width - 1; x++)
            {
                for(var y = 0; y < overlay.Size.Height - 1; y++)
                {
                    var currX = x + center.X - overlay.Size.Width/2;
                    var currY = y + center.Y - overlay.Size.Height/2;

                    if (currX < 0 || currX >= grid.Size.Width || currY < 0 || currY >= grid.Size.Height) continue;

                    grid.Weight[currX, currY] = overlay.Weight[x, y];
                }
            }

            return grid;
        }

        public Grid Clone()
        {
            var grid = new Grid(Size.Width, Size.Height);

            for(var x = 0; x < Size.Width - 1; x++)
            {
                for(var y = 0; y < Size.Height - 1; y++)
                {
                    grid.Weight[x, y] = Weight[x, y];
                }
            }

            return grid;
        }
    }
}

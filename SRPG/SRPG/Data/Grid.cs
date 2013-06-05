using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}

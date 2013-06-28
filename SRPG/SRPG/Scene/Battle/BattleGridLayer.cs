using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class BattleGridLayer : Layer
    {
        private Grid _grid;

        public BattleGridLayer(Torch.Scene scene, string imageName, string gridName) : base(scene)
        {
            Objects.Add("bg", new ImageObject(imageName) { Z = -1 });
            _grid = Grid.FromBitmap(gridName);

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            ClearByName("grid");

            for(var i = 0; i < _grid.Size.Width; i++)
            {
                for(var j = 0; j < _grid.Size.Height; j++)
                {
                    if (_grid.Weight[i, j] > 128)
                    {
                        Objects.Add(
                            string.Format("grid/{0}-{1}", i, j),
                            new ImageObject("Battle/gridhighlight") 
                                {X = i*50, Y = j*50, Z = 10}
                        );
                    }
                }
            }
        }
    }
}

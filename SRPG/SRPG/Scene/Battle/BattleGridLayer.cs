using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class BattleGridLayer : Layer
    {
        private Grid _grid;
        public readonly int Width;
        public readonly int Height;

        public BattleGridLayer(Torch.Scene scene, string imageName, string gridName) : base(scene)
        {
            Objects.Add("bg", new ImageObject(imageName) { Z = -1 });
            _grid = Grid.FromBitmap(gridName);

            Width = Objects["bg"].Width;
            Height = Objects["bg"].Height;

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
                        var gridCell = new SpriteObject("Battle/gridhighlight") { X = i*50, Y = j*50, Z = 10 };
                        gridCell.AddAnimation("Normal", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 1});
                        gridCell.AddAnimation("Selectable", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 51 });
                        gridCell.AddAnimation("Targetted", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 101 });
                        gridCell.AddAnimation("Splashed", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 151 });
                        gridCell.SetAnimation("Normal");

                        Objects.Add(
                            string.Format("grid/{0}-{1}", i, j),
                            gridCell
                        );
                    }
                }
            }
        }

        public void HighlightCell(int x, int y, GridHighlight type)
        {
            if(Objects.ContainsKey(string.Format("grid/{0}-{1}", x, y)))
            {
                ((SpriteObject)Objects[string.Format("grid/{0}-{1}", x, y)]).SetAnimation(type.ToString());
            }
        }

        public void HighlightGrid(Point center, Grid grid, GridHighlight highlightType)
        {
            for (var i = 0; i < grid.Size.Width; i++)
            {
                for (var j = 0; j < grid.Size.Height; j++)
                {
                    if (grid.Weight[i, j] > 0)
                    {
                        HighlightCell(
                            center.X + i - (int)(Math.Floor(grid.Size.Width / 2.0)),
                            center.Y + j - (int)(Math.Floor(grid.Size.Height / 2.0)),
                            highlightType
                        );
                    }
                }
            }
        }

        public void ResetGrid()
        {
            foreach(SpriteObject grid in (from o in Objects.Keys where o.Length > 4 && o.Substring(0,4) == "grid" select Objects[o]))
            {
                grid.SetAnimation("Normal");
            }
        }
    }

    public enum GridHighlight
    {
        Normal,
        Selectable,
        Targetted,
        Splashed
    }
}

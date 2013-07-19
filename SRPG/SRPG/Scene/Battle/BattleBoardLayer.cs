using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class BattleBoardLayer : Layer
    {
        private Grid _grid;
        public int Width;
        public int Height;

        private BattleBoard _board;

        public BattleBoardLayer(Torch.Scene scene) : base(scene) { }

        public void SetBackground(string imageName)
        {
            Objects.Add("bg", new ImageObject(imageName) { Z = -1 });
            Width = Objects["bg"].Width;
            Height = Objects["bg"].Height;
        }

        public void SetGrid(string gridName)
        {
            _grid = Grid.FromBitmap(gridName);
            UpdateGrid();
        }

        public void SetBoard(BattleBoard board)
        {
            _board = board;

            foreach (var character in _board.Characters)
            {
                character.Avatar.Sprite.X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width / 2);
                character.Avatar.Sprite.Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2);
                character.Avatar.Sprite.Z = character.Avatar.Sprite.Y;
                Objects.Add("character/" + character.Name, character.Avatar.Sprite);
                Objects["character/" + character.Name].MouseClick = MouseClickCharacter(character);
            }
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            foreach(var character in _board.Characters)
            {
                Objects["character/" + character.Name].X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2);
                Objects["character/" + character.Name].Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2);
                Objects["character/" + character.Name].Z = character.Avatar.Sprite.Y;
            }
        }

        public void RemoveCharacter(Combatant character)
        {
            Objects.Remove("character/" + character.Name);
        }

        public EventHandler<MouseEventArgs> MouseClickCharacter(Combatant character)
        {
            return (sender, args) =>
                {
                    if (((BattleScene)Scene).FactionTurn != 0) return;

                    ((BattleScene) Scene).SelectCharacter(character);

                };
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

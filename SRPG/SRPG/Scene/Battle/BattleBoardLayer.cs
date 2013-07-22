using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Battle
{
    class BattleBoardLayer : Layer
    {
        private Grid _grid;
        public int Width;
        public int Height;

        public bool AllowAim;
        private Point _targettingCenter;
        private Grid _targettingGrid;
        private Grid _impactGrid;

        /// <summary>
        /// Default camera movement speed, in pixels per second
        /// </summary>
        private const int CamScrollSpeed = 450;

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

        public void SetTargettingGrid(Point center, Grid targetGrid, Grid impactGrid)
        {
            _targettingCenter = center;
            _targettingGrid = targetGrid;
            _impactGrid = impactGrid;
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

            var scene = ((BattleScene) Scene);

            foreach(var character in _board.Characters)
            {
                Objects["character/" + character.Name].X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2);
                Objects["character/" + character.Name].Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2);
                Objects["character/" + character.Name].Z = character.Avatar.Sprite.Y;
            }

            // if the player is in control, allow them to move the camera around
            if (scene.FactionTurn == 0)
            {
                float x = 0;
                float y = 0;

                if (input.IsKeyDown(Keys.A) && !input.IsKeyDown(Keys.D))
                {
                    x = 1;
                }
                else if (input.IsKeyDown(Keys.D) && !input.IsKeyDown(Keys.A))
                {
                    x = -1;
                }

                if (input.IsKeyDown(Keys.S) && !input.IsKeyDown(Keys.W))
                {
                    y = -1;
                }
                else if (input.IsKeyDown(Keys.W) && !input.IsKeyDown(Keys.S))
                {
                    y = 1;
                }

                X += x * gameTime.ElapsedGameTime.Milliseconds / 1000F * CamScrollSpeed;
                Y += y * gameTime.ElapsedGameTime.Milliseconds / 1000F * CamScrollSpeed;
            }

            var viewport = Game.GetInstance().GraphicsDevice.Viewport;

            // lock the camera to within the bounds of the map
            X = MathHelper.Clamp(X, 0 - Width + viewport.Width, 0);
            Y = MathHelper.Clamp(Y, 0 - Height + viewport.Height, 0);

            scene.UpdateCamera(X, Y);

            // check if the cursor is over a character
            foreach (var character in _board.Characters)
            {
                if (character.Avatar.Sprite.Rectangle.Contains((int)(input.Cursor.X - X), (int)(input.Cursor.Y - Y)))
                {
                    scene.ShowCharacterStats(character);
                    break;
                }
                
                scene.HideCharacterStats();
            }

            DefaultGrid();
            
            HighlightGrid(_targettingCenter, _targettingGrid, GridHighlight.Selectable);

            if (AllowAim == false) return;

            // compensate for the camera not being at 0,0
            // as well as do a 50:1 scaling to convert from the screen to the grid
            var cursor = new Point(
                (int)Math.Floor((input.Cursor.X - X) / 50.0),
                (int)Math.Floor((input.Cursor.Y - Y) / 50.0)
            );
            
            // todo i know this statement doesnt work, since _targettingGrid is offset relative to _grid
            if (ValidCell(cursor.X, cursor.Y) && _targettingGrid.Weight[cursor.X - _targettingCenter.X + _targettingGrid.Size.Width / 2, cursor.Y - _targettingCenter.Y + _targettingGrid.Size.Height / 2] > 0)
            {
                HighlightCell(cursor.X, cursor.Y, GridHighlight.Targetted);
                Objects[string.Format("grid/{0}-{1}", cursor.X, cursor.Y)].MouseClick = (s, a) => scene.ExecuteAimAbility(cursor.X, cursor.Y);
            }
            
        }

        private bool ValidCell(int x, int y)
        {
            if (x < 0 || x > _grid.Size.Width) return false;
            if (y < 0 || y > _grid.Size.Height) return false;

            return _grid.Weight[x, y] > 0;
        }

        public GridHighlight CellType(int x, int y)
        {
            switch(((SpriteObject)Objects[String.Format("grid/{0}-{1}", x, y)]).GetAnimation())
            {
                case "Normal": return GridHighlight.Normal;
                case "Selectable": return GridHighlight.Selectable;
                case "Targetted": return GridHighlight.Targetted;
                case "Splashed": return GridHighlight.Splashed;
                default:
                    throw new Exception(String.Format("location {0},{1} is not on the grid", x, y));
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
                        gridCell.AddAnimation("Normal", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 0 });
                        gridCell.AddAnimation("Selectable", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 50 });
                        gridCell.AddAnimation("Targetted", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 100 });
                        gridCell.AddAnimation("Splashed", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 150 });
                        gridCell.SetAnimation("Normal");

                        Objects.Add(
                            string.Format("grid/{0}-{1}", i, j),
                            gridCell
                        );
                    }
                }
            }
        }

        private void HighlightCell(int x, int y, GridHighlight type)
        {
            if(Objects.ContainsKey(string.Format("grid/{0}-{1}", x, y)))
            {
                ((SpriteObject)Objects[string.Format("grid/{0}-{1}", x, y)]).SetAnimation(type.ToString());
            }
        }

        private void HighlightGrid(Point center, Grid grid, GridHighlight highlightType)
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
            DefaultGrid();
            _targettingGrid = new Grid(0, 0);
            _impactGrid = new Grid(0, 0);
            _targettingCenter = new Point(0, 0);
            AllowAim = false;
        }

        private void DefaultGrid()
        {
            foreach (
                SpriteObject grid in
                    (from o in Objects.Keys where o.Length > 4 && o.Substring(0, 4) == "grid" select Objects[o]))
            {
                grid.SetAnimation("Normal");
                grid.MouseOver = (s, a) => { };
                grid.MouseOut = (s, a) => { };
                grid.MouseClick = (s, a) => { };
                grid.MouseRelease = (s, a) => { };
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

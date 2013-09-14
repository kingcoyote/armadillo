using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
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
        private Grid _targettingGrid;
        private Grid _impactGrid;

        private ImageObject _bg;

        /// <summary>
        /// Default camera movement speed, in pixels per second
        /// </summary>
        private const int CamScrollSpeed = 450;

        private BattleBoard _board;

        public BattleBoardLayer(Torch.Scene scene) : base(scene) { }

        public void SetBackground(string imageName)
        {
            _bg = new ImageObject(Game, imageName) { DrawOrder = -1 };
            Components.Add(_bg);
            Width = _bg.Width;
            Height = _bg.Height;
        }

        public void SetGrid(string gridName)
        {
            _grid = Grid.FromBitmap(Game.Services, gridName);
            UpdateGrid();
        }

        public void SetTargettingGrid(Grid targetGrid, Grid impactGrid)
        {
            _targettingGrid = targetGrid;
            _impactGrid = impactGrid;
        }

        public void SetBoard(BattleBoard board)
        {
            _board = board;

            foreach (var character in _board.Characters)
            {
                character.Avatar.Sprite.X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width / 2.0);
                character.Avatar.Sprite.Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2.0);
                character.Avatar.Sprite.Z = character.Avatar.Sprite.Y;
                // todo : Objects.Add("character/" + character.Name, character.Avatar.Sprite);
                // todo : Objects["character/" + character.Name].MouseClick = MouseClickCharacter(character);
            }
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            var scene = ((BattleScene) Scene);

            var mouse = ((IInputService) Game.Services.GetService(typeof (IInputService))).GetMouse().GetState();
            var keyboard = ((IInputService)Game.Services.GetService(typeof(IInputService))).GetKeyboard().GetState();

            foreach(var character in _board.Characters)
            {
                // todo : Objects["character/" + character.Name].X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2.0);
                // todo : Objects["character/" + character.Name].Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2.0);
                // todo : Objects["character/" + character.Name].Z = character.Avatar.Sprite.Y;
            }

            // if the player is in control, allow them to move the camera around
            if (scene.FactionTurn == 0)
            {
                float x = 0;
                float y = 0;

                if (keyboard.IsKeyDown(Keys.A) && !keyboard.IsKeyDown(Keys.D))
                {
                    x = 1;
                }
                else if (keyboard.IsKeyDown(Keys.D) && !keyboard.IsKeyDown(Keys.A))
                {
                    x = -1;
                }

                if (keyboard.IsKeyDown(Keys.S) && !keyboard.IsKeyDown(Keys.W))
                {
                    y = -1;
                }
                else if (keyboard.IsKeyDown(Keys.W) && !keyboard.IsKeyDown(Keys.S))
                {
                    y = 1;
                }

                X += x * gametime.ElapsedGameTime.Milliseconds / 1000F * CamScrollSpeed;
                Y += y * gametime.ElapsedGameTime.Milliseconds / 1000F * CamScrollSpeed;
            }

            var viewport = Game.GraphicsDevice.Viewport;

            // lock the camera to within the bounds of the map
            X = MathHelper.Clamp(X, 0 - Width + viewport.Width, 0);
            Y = MathHelper.Clamp(Y, 0 - Height + viewport.Height, 0);

            scene.UpdateCamera(X, Y);

            // check if the cursor is over a character
            foreach (var character in _board.Characters)
            {
                if (character.Avatar.Sprite.Rectangle.Contains((int)(mouse.X - X), (int)(mouse.Y - Y)))
                {
                    scene.ShowCharacterStats(character);
                    break;
                }
                
                scene.HideCharacterStats();
            }

            DefaultGrid();
            
            HighlightGrid(_targettingGrid, GridHighlight.Selectable);

            if (AllowAim == false) return;

            // compensate for the camera not being at 0,0
            // as well as do a 50:1 scaling to convert from the screen to the grid
            var cursor = new Point(
                (int)Math.Floor((mouse.X - X) / 50.0),
                (int)Math.Floor((mouse.Y - Y) / 50.0)
            );
            
            if (ValidCell(cursor.X, cursor.Y) && _targettingGrid.Weight[cursor.X, cursor.Y] > 0)
            {
               for (var x = 0; x < _impactGrid.Size.Width; x++)
                {
                    for (var y = 0; y < _impactGrid.Size.Height; y++)
                    {
                        if (_impactGrid.Weight[x, y] < 1) continue;

                        HighlightCell(
                            cursor.X - (_impactGrid.Size.Width / 2) + x,
                            cursor.Y - (_impactGrid.Size.Height / 2) + y,
                            GridHighlight.Splashed
                        );
                    }
                }

               HighlightCell(cursor.X, cursor.Y, GridHighlight.Targetted);
               //Objects[string.Format("grid/{0}-{1}", cursor.X, cursor.Y)].MouseClick = (s, a) => scene.ExecuteAimAbility(cursor.X, cursor.Y);
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
            // todo
            //switch(((SpriteObject)Objects[String.Format("grid/{0}-{1}", x, y)]).GetAnimation())
            //{
            //    case "Normal": return GridHighlight.Normal;
            //    case "Selectable": return GridHighlight.Selectable;
            //    case "Targetted": return GridHighlight.Targetted;
            //    case "Splashed": return GridHighlight.Splashed;
            //    default:
            //        throw new Exception(String.Format("location {0},{1} is not on the grid", x, y));
            //}
            throw new Exception("method not fixed");
        }

        public void RemoveCharacter(Combatant character)
        {
            // todo: Objects.Remove("character/" + character.Name);
        }

        // todo 
        //public EventHandler<MouseEventArgs> MouseClickCharacter(Combatant character)
        //{
        //    return (sender, args) =>
        //        {
        //            if (((BattleScene)Scene).FactionTurn != 0) return;

        //            ((BattleScene) Scene).SelectCharacter(character);

        //        };
        //}

        private void UpdateGrid()
        {
            for(var i = 0; i < _grid.Size.Width; i++)
            {
                for(var j = 0; j < _grid.Size.Height; j++)
                {
                    if (_grid.Weight[i, j] > 128)
                    {
                        var gridCell = new SpriteObject(Game, "Battle/gridhighlight") { X = i*50, Y = j*50, Z = 10 };
                        gridCell.AddAnimation("Normal", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 0 });
                        gridCell.AddAnimation("Selectable", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 50 });
                        gridCell.AddAnimation("Targetted", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 100 });
                        gridCell.AddAnimation("Splashed", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 150 });
                        gridCell.SetAnimation("Normal");

                        // todo 
                        //Objects.Add(
                        //    string.Format("grid/{0}-{1}", i, j),
                        //    gridCell
                        //);
                    }
                }
            }
        }

        private void HighlightCell(int x, int y, GridHighlight type)
        {
            // todo
            //if(Objects.ContainsKey(string.Format("grid/{0}-{1}", x, y)))
            //{
            //    ((SpriteObject)Objects[string.Format("grid/{0}-{1}", x, y)]).SetAnimation(type.ToString());
            //}
        }

        private void HighlightGrid(Grid grid, GridHighlight highlightType)
        {
            for (var i = 0; i < grid.Size.Width; i++)
            {
                for (var j = 0; j < grid.Size.Height; j++)
                {
                    if (grid.Weight[i, j] > 0)
                    {
                        HighlightCell(i, j, highlightType);
                    }
                }
            }
        }

        public void ResetGrid()
        {
            DefaultGrid();
            _targettingGrid = new Grid(0, 0);
            _impactGrid = new Grid(0, 0);
            AllowAim = false;
        }

        private void DefaultGrid()
        {
            // todo 
            //foreach (
            //    SpriteObject grid in
            //        (from o in Objects.Keys where o.Length > 4 && o.Substring(0, 4) == "grid" select Objects[o]))
            //{
            //    grid.SetAnimation("Normal");
                
            //}
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

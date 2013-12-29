using System;
using System.Collections.Generic;
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
        public new int Width;
        public new int Height;
        public bool AllowAim;

        public delegate void CharacterDelegate(Combatant character);
        public CharacterDelegate CharacterSelected = c => { };

        private Grid _targettingGrid;
        private Grid _impactGrid;
        private Grid _grid;
        private Dictionary<String, SpriteObject> _characters = new Dictionary<string, SpriteObject>();
        private SpriteObject[,] _gridCells;
        private ImageObject _bg;
        private const int CamScrollSpeed = 450;
        private BattleBoard _board;

        public BattleBoardLayer(Torch.Scene scene, Torch.Object parent) : base(scene, parent) { }

        public void SetBackground(string imageName)
        {
            _bg = new ImageObject(Game, this, imageName) { DrawOrder = -1000 };
            Components.Add(_bg);
            Width = _bg.Width;
            Height = _bg.Height;

            ((IInputService) Game.Services.GetService(typeof (IInputService))).GetMouse().MouseButtonPressed += OnMouseButtonPressed;
        }

        private void OnMouseButtonPressed(MouseButtons buttons)
        {
            var mouse = ((IInputService)Game.Services.GetService(typeof(IInputService))).GetMouse().GetState();

            foreach (var character in _board.Characters)
            {
                if (character.Avatar.Sprite.Rectangle.Contains(mouse.X, mouse.Y))
                {
                    CharacterSelected.Invoke(character);
                    break;
                }
            }
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
                character.Avatar.Sprite.X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width / 2.0) + OffsetX();
                character.Avatar.Sprite.Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2.0) + OffsetY();
                character.Avatar.Sprite.DrawOrder = (int)character.Avatar.Sprite.Y;
                _characters.Add(character.Name, character.Avatar.Sprite);
                Components.Add(character.Avatar.Sprite);
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
                _characters[character.Name].X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2.0) + OffsetX();
                _characters[character.Name].Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2.0) + OffsetY();
                _characters[character.Name].DrawOrder = (int)character.Avatar.Sprite.Y;
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
                if (character.Avatar.Sprite.Rectangle.Contains(mouse.X, mouse.Y))
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
               //_gridCells[cursor.X, cursor.Y].Click = (s, a) => scene.ExecuteAimAbility(cursor.X, cursor.Y);
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
            
            switch(_gridCells[x,y].GetAnimation())
            {
                case "Normal":     return GridHighlight.Normal;
                case "Selectable": return GridHighlight.Selectable;
                case "Targetted":  return GridHighlight.Targetted;
                case "Splashed":   return GridHighlight.Splashed;
                default:           throw new Exception(String.Format("location {0},{1} is not on the grid", x, y));
            }
        }

        public void RemoveCharacter(Combatant character)
        {
            Components.Remove(_characters[character.Name]);
            _characters.Remove(character.Name);
        }

        private void UpdateGrid()
        {
            _gridCells = new SpriteObject[_grid.Size.Width,_grid.Size.Height];

            for(var i = 0; i < _grid.Size.Width; i++)
            {
                for(var j = 0; j < _grid.Size.Height; j++)
                {
                    if (_grid.Weight[i, j] > 128)
                    {
                        var gridCell = new SpriteObject(Game, this, "Battle/gridhighlight") { X = i*50, Y = j*50, DrawOrder = -10 };
                        gridCell.AddAnimation("Normal", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 0 });
                        gridCell.AddAnimation("Selectable", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 50 });
                        gridCell.AddAnimation("Targetted", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 100 });
                        gridCell.AddAnimation("Splashed", new SpriteAnimation { FrameCount = 1, FrameRate = 1, Size = new Rectangle(0, 0, 50, 50), StartRow = 150 });
                        gridCell.SetAnimation("Normal");

                        _gridCells[i, j] = gridCell;
                        Components.Add(gridCell);
                    }
                }
            }
        }

        private void HighlightCell(int x, int y, GridHighlight type)
        {
            if (_gridCells[x, y] == null) return;

            _gridCells[x, y].SetAnimation(type.ToString());
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

            for (int i = 0; i < _grid.Size.Width; i++)
            {
                for (int j = 0; j < _grid.Size.Height; j++)
                {
                    if (_gridCells[i, j] == null) continue;
                    _gridCells[i, j].SetAnimation("Normal");
                }
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

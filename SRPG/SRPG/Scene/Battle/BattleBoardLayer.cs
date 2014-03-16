using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;
using SRPG.Data;
using Torch;
using Torch.UserInterface;
using Game = Torch.Game;

namespace SRPG.Scene.Battle
{
    class BattleBoardLayer : Layer
    {
        public new int Width;
        public new int Height;
        public bool AbilityAim;
        public bool FreeAim;

        public delegate void CharacterDelegate(Combatant character);
        public CharacterDelegate CharacterSelected = c => { };

        public delegate void GridDelegate(int x, int y);
        public GridDelegate GridCellSelected = (x, y) => { };

        private Grid _targettingGrid;
        private Grid _impactGrid;
        private Grid _grid;
        private Dictionary<String, SpriteObject> _characters = new Dictionary<string, SpriteObject>();
        private GridCellControl[,] _gridCells;
        private ImageObject _bg;
        private const int CamScrollSpeed = 450;
        private BattleBoard _board;
        private GuiManager _gui;

        public BattleBoardLayer(Torch.Scene scene, Torch.Object parent) : base(scene, parent)
        {
            _gui = new GuiManager(
                (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager)),
                (IInputService)Game.Services.GetService(typeof(IInputService))
            );
            _gui.DrawOrder = 0;
            _gui.Initialize();

            _gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");
            ((FlatGuiVisualizer)_gui.Visualizer).RendererRepository.AddAssembly(typeof(FlatGridCellControlRenderer).Assembly);

            Components.Add(_gui);

            ((IInputService)Game.Services.GetService(typeof(IInputService))).GetMouse().MouseButtonPressed += OnMouseButtonPressed;
        }

        public void SetBackground(string imageName)
        {
            _bg = new ImageObject(Game, this, imageName) { DrawOrder = -1000 };
            Components.Add(_bg);
            
            Width = _bg.Width;
            Height = _bg.Height;

            _gui.Screen = new Screen(_bg.Width, _bg.Height);
            _gui.Screen.Desktop.Bounds = new UniRectangle(0, 0, _bg.Width, _bg.Height);
        }

        public void AddImage(ImageObject image)
        {
            Components.Add(image);
        }

        private void OnMouseButtonPressed(MouseButtons buttons)
        {
            var mouse = ((IInputService)Game.Services.GetService(typeof(IInputService))).GetMouse().GetState();

            var cursor = new Point(
                (int)Math.Floor((mouse.X - X) / 50.0),
                (int)Math.Floor((mouse.Y - Y) / 50.0)
            );

            // check if the cursor is over a character
            foreach (var character in _board.Characters)
            {
                if (Math.Abs(character.Avatar.Location.X - cursor.X) < 1 && Math.Abs(character.Avatar.Location.Y - cursor.Y) < 1)
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
            _gui.Screen.Desktop.Bounds.Location.X.Offset = OffsetX();
            _gui.Screen.Desktop.Bounds.Location.Y.Offset = OffsetY();

            var scene = ((BattleScene) Scene);

            var mouse = ((IInputService) Game.Services.GetService(typeof (IInputService))).GetMouse().GetState();
            var keyboard = ((IInputService)Game.Services.GetService(typeof(IInputService))).GetKeyboard().GetState();

            foreach(var character in _board.Characters)
            {
                _characters[character.Name].X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2.0) + OffsetX();
                _characters[character.Name].Y = (int)(character.Avatar.Location.Y * 50 + 35 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2.0) + OffsetY();
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

            

            DefaultGrid();
            
            HighlightGrid(_targettingGrid, GridHighlight.Selectable);

            // compensate for the camera not being at 0,0
            // as well as do a 50:1 scaling to convert from the screen to the grid
            var cursor = new Point(
                (int)Math.Floor((mouse.X - X) / 50.0),
                (int)Math.Floor((mouse.Y - Y) / 50.0)
            );

            // check if the cursor is over a character
            foreach (var character in _board.Characters)
            {
                if (Math.Abs(character.Avatar.Location.X - cursor.X) < 1 && Math.Abs(character.Avatar.Location.Y - cursor.Y) < 1)
                {
                    // todo turn this into an event that BattleScene handles
                    scene.ShowCharacterStats(character);

                    break;
                }

                scene.HideCharacterStats();
            }

            if (FreeAim)
            {
                HighlightCell(cursor.X, cursor.Y, GridHighlight.Targetted);
            }
            else if (AbilityAim)
            {
                if (ValidCell(cursor.X, cursor.Y) && _targettingGrid.Weight[cursor.X, cursor.Y] > 0)
                {
                    for (var x = 0; x < _impactGrid.Size.Width; x++)
                    {
                        for (var y = 0; y < _impactGrid.Size.Height; y++)
                        {
                            if (_impactGrid.Weight[x, y] < 1) continue;

                            HighlightCell
                                (
                                    cursor.X - (_impactGrid.Size.Width/2) + x,
                                    cursor.Y - (_impactGrid.Size.Height/2) + y,
                                    GridHighlight.Splashed
                                );
                        }
                    }

                    HighlightCell(cursor.X, cursor.Y, GridHighlight.Targetted);
                }
            }
        }

        public override void Draw(GameTime gametime)
        {
            foreach (var component in (from IDrawable c in Components where c.DrawOrder <= _gui.DrawOrder orderby c.DrawOrder select c).ToArray())
            {
                component.Draw(gametime);
            }

            _spriteBatch.End();
            _spriteBatch.Begin();

            _gui.Draw(gametime);

            foreach (var component in (from IDrawable c in Components  where c.DrawOrder > _gui.DrawOrder orderby c.DrawOrder select c).ToArray())
            {
                component.Draw(gametime);
            }
        }

        private bool ValidCell(int x, int y)
        {
            if (x < 0 || x >= _grid.Size.Width) return false;
            if (y < 0 || y >= _grid.Size.Height) return false;

            return _grid.Weight[x, y] > 0;
        }

        public GridHighlight CellType(int x, int y)
        {
            return _gridCells[x, y].Highlight;
        }

        public void RemoveCharacter(Combatant character)
        {
            Components.Remove(_characters[character.Name]);
            _characters.Remove(character.Name);
        }

        private void UpdateGrid()
        {
            _gridCells = new GridCellControl[_grid.Size.Width,_grid.Size.Height];

            for(var i = 0; i < _grid.Size.Width; i++)
            {
                for(var j = 0; j < _grid.Size.Height; j++)
                {
                    if (_grid.Weight[i, j] <= 128) continue;

                    // create local instances of i and js so the anon GridClicked function won't misbehave
                    var li = i;
                    var lj = j;

                    var gridCell = new GridCellControl { Bounds = new UniRectangle(i*50, j*50, 50, 50) };
                    gridCell.GridClicked += () => GridCellSelected.Invoke(li, lj);
                    _gridCells[i, j] = gridCell;
                    _gui.Screen.Desktop.Children.Add(gridCell);
                    gridCell.GridClicked += () => { ((BattleScene) Scene).ExecuteAimAbility(li, lj); };
                }
            }
        }

        private void HighlightCell(int x, int y, GridHighlight type)
        {
            if (x < 0 || x >= _grid.Size.Width || y < 0 || y >= _grid.Size.Height) return;
            if (_gridCells[x, y] == null) return;

            _gridCells[x, y].Highlight = type;
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
            AbilityAim = false;
        }

        private void DefaultGrid()
        {

            for (int i = 0; i < _grid.Size.Width; i++)
            {
                for (int j = 0; j < _grid.Size.Height; j++)
                {
                    if (_gridCells[i, j] == null) continue;
                    _gridCells[i, j].Highlight = GridHighlight.Normal;
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

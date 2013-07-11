using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Battle
{
    class BattleScene : Torch.Scene
    {
        public BattleBoard BattleBoard;
        public int FactionTurn;
        public bool AwaitingAction;
        public int RoundNumber;

        public BattleScene(Game game) : base(game) { }

        private Dictionary<Direction, bool> _camDirections = new Dictionary<Direction, bool>() 
            { {Direction.Up, false}, {Direction.Right, false}, {Direction.Down, false}, {Direction.Left, false} }; 

        private const int CamScrollSpeed = 450;

        private float _x;
        private float _y;
        private Combatant _selectedCharacter = null;
        private BattleState _state;
        private Action<int, int> _aimAbility = (int x, int y) => { };
        private Grid _aimGrid;
        private Command _currentCommand;
        private List<Point> _movementCoords;

        /// <summary>
        /// Pre-battle initialization sequence to load characters, the battleboard and the image layers.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Layers.Add("character stats", new CharacterStats(this) { ZIndex = 5000, Visible = false});
            Layers.Add("hud", new HUD(this) { ZIndex = 5000 });
        }

        public override void Start()
        {
            base.Start();
            Game.GetInstance().IsMouseVisible = true;
        }

        public override void Stop()
        {
            base.Stop();
            Game.GetInstance().IsMouseVisible = false;
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            float dt = gameTime.ElapsedGameTime.Milliseconds/1000F;

            if(FactionTurn == 0)
            {
                float x = 0;
                float y = 0;

                if(input.IsKeyDown(Keys.A) && !input.IsKeyDown(Keys.D))
                {
                    x = 1;
                } else if (input.IsKeyDown(Keys.D) && !input.IsKeyDown(Keys.A))
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

                _x += x * dt * CamScrollSpeed;
                _y += y * dt * CamScrollSpeed;
            }

            var viewport = Game.GetInstance().GraphicsDevice.Viewport;

            if (_x > 0) _x = 0;
            if (_x < 0 - ((BattleGridLayer)Layers["battlegrid"]).Width + viewport.Width) _x = 0 - ((BattleGridLayer)Layers["battlegrid"]).Width + viewport.Width;

            if (_y > 0) _y = 0;
            if (_y < 0 - ((BattleGridLayer)Layers["battlegrid"]).Height + viewport.Height) _y = 0 - ((BattleGridLayer)Layers["battlegrid"]).Height + viewport.Height;

            if (_selectedCharacter != null)
            {
                var cursorDistance = CalculateDistance(
                    input.Cursor.X + (0 - _x), 
                    input.Cursor.Y + (0 - _y),
                    _selectedCharacter.Avatar.Sprite.X + _selectedCharacter.Avatar.Sprite.Width / 2,
                    _selectedCharacter.Avatar.Sprite.Y + _selectedCharacter.Avatar.Sprite.Height / 2
                );

                if(cursorDistance > 125)
                {
                    DeselectCharacter();
                }
            }

            if (_state == BattleState.AimingAbility)
            {
                ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
                ShowGrid(_selectedCharacter, _aimGrid, GridHighlight.Selectable);

                // compensate for the camera not being at 0,0
                // as well as do a 50:1 scaling to convert from the screen to the grid
                var cursor = new Point(
                    (int)Math.Floor((input.Cursor.X - _x)/50.0), 
                    (int)Math.Floor((input.Cursor.Y - _y)/50.0)
                );

                // compensate for the character's position, since they likely aren't at 0, 0
                // also adjust for them being centered in the aim grid
                var checkX = (int) (cursor.X - _selectedCharacter.Avatar.Location.X + Math.Floor(_aimGrid.Size.Width/2.0));
                var checkY = (int) (cursor.Y - _selectedCharacter.Avatar.Location.Y + Math.Floor(_aimGrid.Size.Height/2.0));

                if(checkX >= 0 && checkX < _aimGrid.Size.Width && checkY >= 0 && checkY < _aimGrid.Size.Height && _aimGrid.Weight[checkX, checkY] > 0)
                {
                    ((BattleGridLayer) Layers["battlegrid"]).HighlightGrid(cursor.X, cursor.Y, GridHighlight.Targetted);
                    if (input.LeftButton == ButtonState.Pressed)
                    {
                        _aimAbility(cursor.X, cursor.Y);
                    }
                }
            }

            if(_state == BattleState.ExecutingCommand && _currentCommand.Ability.Name == "Move")
            {
                UpdateMovement(dt);
            }

            UpdateCamera();
        }

        private double CalculateDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        private void UpdateCamera()
        {
            Layers["battlegrid"].X = _x;
            Layers["battlegrid"].Y = _y;

            Layers["battleboardlayer"].X = _x;
            Layers["battleboardlayer"].Y = _y;

            if (Layers.ContainsKey("radial menu"))
            {
                Layers["radial menu"].X = _x;
                Layers["radial menu"].Y = _y;
            }
        }

        public void SetBattle(string battleName)
        {
            BattleBoard = new BattleBoard();

            RoundNumber = 1;
            FactionTurn = 0;

            _state = BattleState.PlayerTurn;

            var partyGrid = new List<Point>();

            switch(battleName)
            {
                case "coliseum/halls":
                    Layers.Add("battlegrid", new BattleGridLayer(this, "Zones/Coliseum/Halls/halls", "Zones/Coliseum/Halls/battle"));
                    BattleBoard.Sandbag = Grid.FromBitmap("Zones/Coliseum/Halls/battle");

                    partyGrid.Add(new Point(14,35));
                    partyGrid.Add(new Point(15,35));
                    partyGrid.Add(new Point(13,35));
                    partyGrid.Add(new Point(14,36));
                    partyGrid.Add(new Point(15,36));
                    partyGrid.Add(new Point(13,36));

                    BattleBoard.Characters.Add(GenerateCombatant("Guard", "coliseum/guard", new Vector2(9, 2)));

                    break;
                default:
                    throw new Exception("unknown battle " + battleName);
            }


            for(var i = 0; i < ((SRPGGame)Game).Party.Count; i++)
            {
                var character = ((SRPGGame) Game).Party[i];
                character.Avatar.Location.X = partyGrid[i].X;
                character.Avatar.Location.Y = partyGrid[i].Y;
                BattleBoard.Characters.Add(character);
            }

            Layers.Add("battleboardlayer", new BattleBoardLayer(this, BattleBoard));

            _x = 0 - partyGrid[0].X*50 + Game.GetInstance().GraphicsDevice.Viewport.Width / 2;
            _y = 0 - partyGrid[0].Y*50 + Game.GetInstance().GraphicsDevice.Viewport.Height / 2;

            UpdateCamera();

            ChangeFaction(0);
        }

        /// <summary>
        /// Process a faction change, including swapping out UI elements and preparing for the AI / human input.
        /// </summary>
        /// <param name="faction"></param>
        public void ChangeFaction(int faction)
        {
            _state = faction == 0 ? BattleState.PlayerTurn : BattleState.EnemyTurn;

            foreach(var character in BattleBoard.Characters)
            {
                if (character.Faction == faction)
                {
                    character.BeginRound();
                }
                else
                {
                    character.EndRound();
                }
            }
        }

        /// <summary>
        /// Process a round change, including status ailments.
        /// </summary>
        public void ChangeRound()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process a command given by either the AI or the player. These commands can be attacking, casting a spell,
        /// using an item or moving.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        public void ExecuteCommand(Command command)
        {
            _state = BattleState.ExecutingCommand;

            _currentCommand = command;

            if (command.Ability.Name == "Move")
            {
                // special case for movement
                _movementCoords = Pathfind(new Point((int)_selectedCharacter.Avatar.Location.X, (int)_selectedCharacter.Avatar.Location.Y), command.Target, _selectedCharacter.Faction);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void UpdateMovement(float dt)
        {
            var x = 0 - (_selectedCharacter.Avatar.Location.X - _movementCoords[0].X);
            var y = 0 - (_selectedCharacter.Avatar.Location.Y - _movementCoords[0].Y);

            if (x < -0.1) x = -1;
            if (x > 0.1) x = 1;
            if (y < -0.1) y = -1;
            if (y > 0.1) y = 1;

            _selectedCharacter.Avatar.UpdateVelocity(x, y);

            _selectedCharacter.Avatar.Location.X += x*dt*_selectedCharacter.Avatar.Speed/50;
            _selectedCharacter.Avatar.Location.Y += y*dt*_selectedCharacter.Avatar.Speed/50;

            if (Math.Abs(x - 0) < 0.05 && Math.Abs(y - 0) < 0.05)
            {
                _selectedCharacter.Avatar.Location.X = _movementCoords[0].X;
                _selectedCharacter.Avatar.Location.Y = _movementCoords[0].Y;
                _movementCoords.RemoveAt(0);
            }

            if (_movementCoords.Count == 0)
            {
                _state = _selectedCharacter.Faction == 0 ? BattleState.PlayerTurn : BattleState.EnemyTurn;
                _selectedCharacter.Avatar.UpdateVelocity(0, 0);
                _selectedCharacter = null;
                HideCharacterStats();
            }
        }

        /// <summary>
        /// A* algorithm for calculating a path between two points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="faction"></param>
        /// <returns></returns>
        private List<Point> Pathfind(Point start, Point end, int faction = -1)
        {
            var closedSet = new List<Point>();
            var openSet = new List<Point> { start };
            var cameFrom = new Dictionary<Point, Point>();
            var currentDistance = new Dictionary<Point, int>();
            var predictedDistance = new Dictionary<Point, float>();

            currentDistance.Add(start, 0);
            predictedDistance.Add(start, 0 + (int)CalculateDistance(start.X, start.Y, end.X, end.Y));

            while (openSet.Count > 0)
            {
                // get the node with the lowest estimated cost to finish
                var current = (from p in openSet orderby predictedDistance[p] ascending select p).First();

                // if it is the finish, return the path
                if(current.X == end.X && current.Y == end.Y)
                {
                    // generate the found path
                    var path = ReconstructPath(cameFrom, end);
                    
                    // filter out items that are not a direction change
                    for(var i = path.Count - 2; i > 0; i--)
                    {
                        if(path.ElementAt(i - 1).X == path.ElementAt(i + 1).X || path.ElementAt(i - 1).Y == path.ElementAt(i + 1).Y)
                        {
                            path.RemoveAt(i);
                        }
                    }

                    return path;
                }

                // move current node from open to closed
                openSet.Remove(current);
                closedSet.Add(current);

                // process each valid node around the current node
                foreach(var neighbor in GetNeighborNodes(current, faction))
                {
                    var tempCurrentDistance = currentDistance[current] + 1;
                    
                    // if we already know a faster way to this neighbor, use that route and ignore this one
                    if(closedSet.Contains(neighbor) && tempCurrentDistance >= currentDistance[neighbor])
                    {
                        continue;
                    }

                    // if we don't know a route to this neighbor, or if this is faster, store this route
                    if(!closedSet.Contains(neighbor) || tempCurrentDistance < currentDistance[neighbor])
                    {
                        if(cameFrom.Keys.Contains(neighbor))
                        {
                            cameFrom[neighbor] = current;
                        }
                        else
                        {
                            cameFrom.Add(neighbor, current);
                        }

                        currentDistance[neighbor] = tempCurrentDistance;
                        predictedDistance[neighbor] = currentDistance[neighbor] + (float)CalculateDistance(neighbor.X, neighbor.Y, end.X, end.Y);

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

        private List<Point> GetNeighborNodes(Point node, int faction)
        {
            var nodes = new List<Point>();
            
            // up
            if(BattleBoard.IsAccessible(new Point(node.X, node.Y - 1), faction))
            {
                nodes.Add(new Point(node.X, node.Y - 1));
            }

            // right
            if (BattleBoard.IsAccessible(new Point(node.X + 1, node.Y), faction))
            {
                nodes.Add(new Point(node.X + 1, node.Y));
            }

            // down
            if (BattleBoard.IsAccessible(new Point(node.X, node.Y + 1), faction))
            {
                nodes.Add(new Point(node.X, node.Y + 1));
            }

            // left
            if (BattleBoard.IsAccessible(new Point(node.X - 1, node.Y), faction))
            {
                nodes.Add(new Point(node.X - 1, node.Y));
            }

            return nodes;
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            if(cameFrom.Keys.Contains(current))
            {
                var path = ReconstructPath(cameFrom, cameFrom[current]);
                path.Add(current);
                return path;
            }
            else
            {
                return new List<Point> { current };
            }
        }

        public void ShowCharacterStats(Combatant character)
        {
            if (_state != BattleState.PlayerTurn) return;

            ((CharacterStats) Layers["character stats"]).SetCharacter(character);
            Layers["character stats"].Visible = true;
        }

        public void HideCharacterStats()
        {
            if (_state != BattleState.PlayerTurn) return;

            Layers["character stats"].Visible = false;
        }

        public Combatant GenerateCombatant(string name, string template, Vector2 location)
        {
            var combatant = Combatant.FromTemplate(template);
            combatant.Name = name;
            combatant.Avatar.Location = location;
            combatant.Faction = 1;

            return combatant;
        }

        public void SelectCharacter(Combatant character)
        {
            if (character == _selectedCharacter)
            {
                DeselectCharacter();
                return;
            }
            if (_state != BattleState.PlayerTurn) return;

            if (Layers.ContainsKey("radial menu"))
            {
                Layers.Remove("radial menu");
            }

            var menu = new RadialMenu(this)
            {
                CenterX = character.Avatar.Sprite.X + character.Avatar.Sprite.Width / 2,
                CenterY = character.Avatar.Sprite.Y + character.Avatar.Sprite.Height / 2,
                ZIndex = 5000
            };

            var icon = new SpriteObject("Battle/Menu/move");
            SetCharacterMenuAnimations(icon);
            if (character.CanMove)
            {
                icon.MouseOver += (sender, args) =>
                    {
                        if (!character.CanMove) return;
                        ShowGrid(character, GetMovementGrid(character), GridHighlight.Selectable);
                    };
                icon.MouseOut += (sender, args) => ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
                icon.MouseRelease += SelectMovementTarget(character);
            }
            else
            {
                icon.MouseOver = (sender, args) => { };
                icon.MouseOut = (sender, args) => { };
                icon.MouseClick = (sender, args) => { };
                icon.MouseRelease = (sender, args) => { };
                
            }
            menu.AddOption("move", icon);

            icon = new SpriteObject("Battle/Menu/attack");
            SetCharacterMenuAnimations(icon);
            icon.MouseOver += (sender, args) =>
                {
                    if (!character.CanAct) return;
                    ShowGrid(character, character.GetEquippedWeapon().TargetGrid, GridHighlight.Selectable);
                };
            icon.MouseOut += (sender, args) => ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
            icon.MouseRelease += SelectAttackTarget(character);
            menu.AddOption("attack", icon);

            icon = new SpriteObject("Battle/Menu/special");
            SetCharacterMenuAnimations(icon);
            menu.AddOption("special", icon);

            icon = new SpriteObject("Battle/Menu/item");
            SetCharacterMenuAnimations(icon);
            menu.AddOption("item", icon);

            Layers.Add("radial menu", menu);

            _selectedCharacter = character;
            _state = BattleState.CharacterSelected;
        }

        public void DeselectCharacter()
        {
            if (_state != BattleState.CharacterSelected) return;

            if(Layers.ContainsKey("radial menu"))
            {
                Layers.Remove("radial menu");
            }

            ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();

            _selectedCharacter = null;

            _state = BattleState.PlayerTurn;

            HideCharacterStats();
        }

        private static void SetCharacterMenuAnimations(SpriteObject icon)
        {
            icon.AddAnimation(
                "normal",
                new SpriteAnimation {FrameCount = 1, Size = new Rectangle(0, 0, 50, 50), FrameRate = 1, StartRow = 1}
            );
            icon.AddAnimation(
                "hover",
                new SpriteAnimation {FrameCount = 1, Size = new Rectangle(0, 0, 50, 50), FrameRate = 1, StartRow = 51}
            );
            icon.AddAnimation(
                "click",
                new SpriteAnimation {FrameCount = 1, Size = new Rectangle(0, 0, 50, 50), FrameRate = 1, StartRow = 101}
            );

            icon.SetAnimation("normal");

            icon.MouseOver += (sender, args) => ((SpriteObject)args.Target).SetAnimation("hover"); 
            icon.MouseOut += (sender, args) => ((SpriteObject)args.Target).SetAnimation("normal");
            icon.MouseClick += (sender, args) => ((SpriteObject) args.Target).SetAnimation("click");
            icon.MouseRelease += (sender, args) => ((SpriteObject) args.Target).SetAnimation("normal");
        }

        private EventHandler<MouseEventArgs> SelectMovementTarget(Combatant character)
        {
            return (sender, args) =>
                {
                    _state = BattleState.AimingAbility;
                    Layers.Remove("radial menu");
                    _aimGrid = GetMovementGrid(character);
                    _aimAbility = (x, y) =>
                        {
                            if (BattleBoard.IsOccupied(new Point(x, y)) != -1) return;

                            ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();

                            var command = new Command();
                            command.Character = character;
                            command.Target = new Point(x, y);
                            command.Ability = Ability.Factory("move");
                            ExecuteCommand(command);
                            character.CanMove = false;
                        };
                };
        }

        private EventHandler<MouseEventArgs> SelectAttackTarget(Combatant character)
        {
            return (sender, args) =>
            {
                _state = BattleState.AimingAbility;
                Layers.Remove("radial menu");
                _aimGrid = character.GetEquippedWeapon().TargetGrid;
                _aimAbility = (x, y) =>
                {
                    
                };
            };
        }

        private void ShowGrid(Combatant character, Grid grid, GridHighlight highlightType)
        {
            for (var i = 0; i < grid.Size.Width; i++)
            {
                for (var j = 0; j < grid.Size.Height; j++)
                {
                    if (grid.Weight[i, j] > 0)
                    {
                        ((BattleGridLayer)Layers["battlegrid"]).HighlightGrid(
                            (int)character.Avatar.Location.X + i - (int)(Math.Floor(grid.Size.Width / 2.0)), 
                            (int)character.Avatar.Location.Y + j - (int)(Math.Floor(grid.Size.Height / 2.0)), 
                            highlightType
                        );
                    }
                }
            }
        }

        private Grid GetMovementGrid(Combatant character)
        {
            var grid = new Grid(25, 25);

            grid.Weight[12, 12] = 1;

            List<int[]> currentRound;
            var neighbors = new List<int[]> {new[] {0, -1}, new[] {1, 0}, new[] {0, 1}, new[] {-1, 0}};
            var lastRound = new List<int[]> {new[] {12, 12}};

            for (var i = 0; i < character.Stats[Stat.Speed] / 3; i++)
            {
                currentRound = new List<int[]>();

                foreach (var square in lastRound)
                {
                    foreach (var neighbor in neighbors)
                    {
                        // check if this cell has already been processed
                        if (grid.Weight[square[0] + neighbor[0], square[1] + neighbor[1]] == 1) continue;

                        var checkPoint = new Point(
                            (int) (character.Avatar.Location.X + square[0] + neighbor[0]) - 12,
                            (int) (character.Avatar.Location.Y + square[1] + neighbor[1]) - 12
                        );

                        if (BattleBoard.IsAccessible(checkPoint, character.Faction))
                        {
                            currentRound.Add(new[] {square[0] + neighbor[0], square[1] + neighbor[1]});
                        }
                    }
                }

                foreach(var newSquare in currentRound)
                {
                    grid.Weight[newSquare[0], newSquare[1]] = 1;
                }

                lastRound = currentRound;
            }

            return grid;
        }
    }

    enum BattleState
    {
        EnemyTurn,
        PlayerTurn,
        CharacterSelected,
        SelectingAbility,
        AimingAbility,
        ExecutingAbility,
        ExecutingCommand
    }
}

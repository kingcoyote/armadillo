using System;
using System.Collections.Generic;
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
        private Action _aimAbility = () => { };
        private Grid _aimGrid;

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
            
        }
        // execute a command
        // onclick handlers for selecting characters
        // onclick handlers for selecting menu items
        // round change

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
            icon.MouseOver += (sender, args) =>
                {
                    if (!character.CanMove) return;
                    ShowGrid(character, GetMovementGrid(character), GridHighlight.Selectable);
                };
            icon.MouseOut += (sender, args) => ((BattleGridLayer)Layers["battlegrid"]).ResetGrid();
            icon.MouseRelease += SelectMovementTarget(character);
            menu.AddOption("move", icon);

            icon = new SpriteObject("Battle/Menu/attack");
            SetCharacterMenuAnimations(icon);
            icon.MouseOver += (sender, args) =>
                {
                    if (!character.CanAct) return;
                    ShowGrid(character, character.GetEquippedWeapon().TargetGrid, GridHighlight.Selectable);
                };
            icon.MouseOut += (sender, args) => ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
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
                    _aimAbility = () => { };
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
    }
}

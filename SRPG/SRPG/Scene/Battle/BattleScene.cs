using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Battle
{
    class BattleScene : Torch.Scene
    {
        /// <summary>
        /// The BattleBoard representing the current characters in battle, as well as the battle grid.
        /// </summary>
        public BattleBoard BattleBoard;
        /// <summary>
        /// A number representing which faction currently has control. 0 is the player, 1 is the computer.
        /// </summary>
        public int FactionTurn;
        /// <summary>
        /// The current round number. This incremenmts once for each cycle of player/computer.
        /// </summary>
        public int RoundNumber;
        /// <summary>
        /// Default camera movement speed, in pixels per second
        /// </summary>
        private const int CamScrollSpeed = 450;
        /// <summary>
        /// X value offset of the camera compared to the board.
        /// </summary>
        private float _x;
        /// <summary>
        /// Y value offset of the camera compared to the board.
        /// </summary>
        private float _y;
        /// <summary>
        /// The character currently being selected by either the player or the computer, used
        /// to determine command targets and stats to display.
        /// </summary>
        private Combatant _selectedCharacter;
        /// <summary>
        /// Finite state machine indicating the current state of the battle. This changes on certain actions, such as clicking
        /// a character, executing a command, selecting a command, or switching to the enemy turn. Large numbers of methods
        /// check the state to determine fi the current action is valid.
        /// </summary>
        private BattleState _state;
        /// <summary>
        /// Callback that executes when an ability is aimed. This can be a move ability callback moving a character, or a combat ability
        /// callback queuing the command.
        /// </summary>
        private Func<int, int, bool> _aimAbility = (x, y) => true; 
        /// <summary>
        /// A grid associated with the aimed ability, showing what tiles can be aimed at. For instance - a movement grid for aiming a move
        /// or a targeting grid for aiming an attack.
        /// </summary>
        private Grid _aimGrid;
        /// <summary>
        /// A queued list of commands awaiting execution. This list is added to when telling a character to attack, use an ability or use an item.
        /// </summary>
        public List<Command> QueuedCommands { get; private set; }
        /// <summary>
        /// When the current command is a move, and it is being executed, this is the coordinates for the character to follow from point A to point B.
        /// </summary>
        private List<Point> _movementCoords;

        private List<Hit> _hits = new List<Hit>();

        public BattleScene(Game game) : base(game) { }

        /// <summary>
        /// Pre-battle initialization sequence to load characters, the battleboard and the image layers.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            QueuedCommands = new List<Command>();

            Layers.Add("character stats", new CharacterStats(this) { ZIndex = 5000, Visible = false});
            Layers.Add("hud", new HUD(this) { ZIndex = 5000 });
            Layers.Add("queuedcommands", new QueuedCommands(this) { ZIndex = 5000, Visible = false });
            Layers.Add("hitlayer", new HitLayer(this) { ZIndex = 5000 });
            Layers.Add("abilitystat", new AbilityStatLayer(this) { ZIndex = 5000, Visible = false });
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
            else if (input.IsKeyDown(Keys.Enter))
            {
                ChangeFaction(0);
            }

            var viewport = Game.GetInstance().GraphicsDevice.Viewport;

            _x = MathHelper.Clamp(_x, 0 - ((BattleGridLayer) Layers["battlegrid"]).Width + viewport.Width, 0);
            _y = MathHelper.Clamp(_y, 0 - ((BattleGridLayer) Layers["battlegrid"]).Height + viewport.Height, 0);

            Layers["queuedcommands"].Visible = _state != BattleState.EnemyTurn && QueuedCommands.Count > 0;

            UpdateBattleState(input, dt);
            UpdateCamera();
        }

        private void UpdateBattleState(Input input, float dt)
        {
            switch(_state)
            {
                case BattleState.CharacterSelected:
                    UpdateBattleStateCharacterSelected(input, dt);
                    break;
                case BattleState.AimingAbility:
                    UpdateBattleStateAimingAbility(input, dt);
                    break;
                case BattleState.ExecutingCommand:
                    UpdateBattleStateExecutingCommand(input, dt);
                    break;
                case BattleState.DisplayingHits:
                    UpdateBattleStateDisplayingHits(input, dt);
                    break;
                case BattleState.MovingCharacter:
                    UpdateBattleStateMovingCharacter(input, dt);
                    break;
            }
        }

        private void UpdateBattleStateDisplayingHits(Input input, float dt)
        {
            // newhits stores hits that will remain in queue
            var newHits = new List<Hit>();
            
            // process each hit in the old queue
            for(var i = 0; i < _hits.Count; i++)
            {
                var hit = _hits.ElementAt(i);
                
                // decrease the delay
                hit.Delay -= (int)(dt*1000);

                // if the hit is ready to display
                if(hit.Delay <= 0)
                {
                    // display it
                    var character = BattleBoard.GetCharacterAt(hit.Target);
                    var damage = character.ProcessHit(hit);
                    ((HitLayer) Layers["hitlayer"]).DisplayHit(damage, Color.White, hit.Target);
                }
                else
                {
                    // keep it in queue
                    newHits.Add(hit);
                }
            }

            if (newHits.Count > 0)
            {
                // update the queue
                _hits = newHits;
            }
            else
            {
                // todo : 1 sec delay between commands
                CheckCharacterDeath();
                _state = BattleState.ExecutingCommand;
                ResetState();
            }
        }

        private void UpdateBattleStateExecutingCommand(Input input, float dt)
        {
            if (QueuedCommands.Count > 0)
            {
                ExecuteCommand(QueuedCommands[0]);
                QueuedCommands.RemoveAt(0);
            }
            else
            {
                _state = FactionTurn == 0 ? BattleState.PlayerTurn : BattleState.EnemyTurn;
            }
        }

        private void UpdateBattleStateMovingCharacter(Input input, float dt)
        {
            var x = 0 - (_selectedCharacter.Avatar.Location.X - _movementCoords[0].X);
            var y = 0 - (_selectedCharacter.Avatar.Location.Y - _movementCoords[0].Y);

            if (x < -0.1) x = -1;
            if (x > 0.1) x = 1;
            if (y < -0.1) y = -1;
            if (y > 0.1) y = 1;

            _selectedCharacter.Avatar.UpdateVelocity(x, y);

            _selectedCharacter.Avatar.Location.X += x * dt * _selectedCharacter.Avatar.Speed / 50;
            _selectedCharacter.Avatar.Location.Y += y * dt * _selectedCharacter.Avatar.Speed / 50;

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
                ResetState();
            }
        }

        private void UpdateBattleStateAimingAbility(Input input, float dt)
        {
            ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
            ((BattleGridLayer) Layers["battlegrid"]).HighlightGrid(
                new Point((int)_selectedCharacter.Avatar.Location.X, (int)_selectedCharacter.Avatar.Location.Y), 
                _aimGrid, 
                GridHighlight.Selectable
            );

            // compensate for the camera not being at 0,0
            // as well as do a 50:1 scaling to convert from the screen to the grid
            var cursor = new Point(
                (int) Math.Floor((input.Cursor.X - _x)/50.0),
                (int) Math.Floor((input.Cursor.Y - _y)/50.0)
                );

            // compensate for the character's position, since they likely aren't at 0, 0
            // also adjust for them being centered in the aim grid
            var checkX = (int) (cursor.X - _selectedCharacter.Avatar.Location.X + Math.Floor(_aimGrid.Size.Width/2.0));
            var checkY = (int) (cursor.Y - _selectedCharacter.Avatar.Location.Y + Math.Floor(_aimGrid.Size.Height/2.0));

            if (checkX >= 0 && checkX < _aimGrid.Size.Width && checkY >= 0 && checkY < _aimGrid.Size.Height &&
                _aimGrid.Weight[checkX, checkY] > 0)
            {
                ((BattleGridLayer) Layers["battlegrid"]).HighlightCell(cursor.X, cursor.Y, GridHighlight.Targetted);
                if (input.LeftButton == ButtonState.Pressed)
                {
                    if (_aimAbility(cursor.X, cursor.Y))
                    {
                        ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
                    }
                }
            }
        }

        private void UpdateBattleStateCharacterSelected(Input input, float dt)
        {
            var cursorDistance = Math.Sqrt(
                Math.Pow((input.Cursor.X + (0 - _x)) - (_selectedCharacter.Avatar.Sprite.X + _selectedCharacter.Avatar.Sprite.Width/2.0), 2) + 
                Math.Pow((input.Cursor.Y + (0 - _y)) - (_selectedCharacter.Avatar.Sprite.Y + _selectedCharacter.Avatar.Sprite.Height/2.0), 2)
            );

            if (cursorDistance > 125)
            {
                DeselectCharacter();
            }
        }

        private void CheckCharacterDeath()
        {
            var dead = (from c in BattleBoard.Characters where c.CurrentHealth <= 0 select c);

            IEnumerable<Combatant> combatants = dead as Combatant[] ?? dead.ToArray();

            if (!combatants.Any()) return;

            foreach(var character in combatants.ToArray())
            {
                character.Die();
                BattleBoard.Characters.Remove(character);
                ((BattleBoardLayer)Layers["battleboardlayer"]).RemoveCharacter(character);
            }
        }

        /// <summary>
        /// Move layers that are relevant to the battlegrid in relation to the camera.
        /// </summary>
        private void UpdateCamera()
        {
            var layers = new List<string> {"battlegrid", "battleboardlayer", "radial menu", "hitlayer"};

            foreach (var layer in layers)
            {
                if (Layers.ContainsKey(layer))
                {
                    Layers[layer].X = _x;
                    Layers[layer].Y = _y;
                }
            }
        }

        public void SetBattle(string battleName)
        {
            BattleBoard = new BattleBoard();

            RoundNumber = 0;
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
                    BattleBoard.Characters.Add(GenerateCombatant("Guard Captain", "coliseum/guard", new Vector2(14, 33)));

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
            FactionTurn = faction;
            if (faction == 0) RoundNumber++;

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

            switch (command.Ability.Name)
            {
                case "Move":
                    {
                        // special case for movement
                        var grid = BattleBoard.GetAccessibleGrid(command.Character.Faction);
                        var coords = grid.Pathfind(new Point((int)command.Character.Avatar.Location.X, (int)command.Character.Avatar.Location.Y), command.Target);

                        for (var i = coords.Count - 2; i > 0; i--)
                        {
                            if(coords[i - 1].X == coords[i + 1].X || coords[i - 1].Y == coords[i + 1].Y)
                            {
                                coords.RemoveAt(i);
                            }
                        }

                        _movementCoords = coords;

                        _state = BattleState.MovingCharacter;

                        command.Character.CanMove = false;
                    }
                    break;
                default:
                    var hits = command.Ability.GenerateHits(BattleBoard, command.Target);

                    for (var i = hits.Count - 1; i >= 0; i--)
                    {
                        if(BattleBoard.GetCharacterAt(hits[i].Target) == null)
                        {
                            hits.RemoveAt(i);
                        }
                    }

                    if (hits.Count > 0)
                    {
                        command.Character.CanAct = false;
                        DisplayHits(hits);
                        command.Character.CurrentMana -= command.Ability.ManaCost;
                    }

                    break;
            }
        }

        /// <summary>
        /// Display a HUD element indicating basic stats for a specified character
        /// </summary>
        /// <param name="character">The character to be shown.</param>
        public void ShowCharacterStats(Combatant character)
        {
            if (_state != BattleState.PlayerTurn) return;

            ((CharacterStats) Layers["character stats"]).SetCharacter(character);
            Layers["character stats"].Visible = true;
        }

        /// <summary>
        /// Hide the HUD element created by ShowCharacterStats
        /// </summary>
        public void HideCharacterStats()
        {
            if (_state != BattleState.PlayerTurn) return;

            Layers["character stats"].Visible = false;
        }

        /// <summary>
        /// Create a Combatant object from a template to inject into the battle
        /// </summary>
        /// <param name="name">The human readable name of the combatant.</param>
        /// <param name="template">The template name to build the combatant from.</param>
        /// <param name="location">The location of the combatant on the battleboard.</param>
        /// <returns>The generated combatant.</returns>
        public Combatant GenerateCombatant(string name, string template, Vector2 location)
        {
            var combatant = Combatant.FromTemplate(template);
            combatant.Name = name;
            combatant.Avatar.Location = location;
            combatant.Faction = 1;

            return combatant;
        }

        /// <summary>
        /// Process a character that has been selected by the player, showing a radial menu of options.
        /// </summary>
        /// <param name="character">The character whose options are to be displayed.</param>
        public void SelectCharacter(Combatant character)
        {
            if (character == _selectedCharacter)
            {
                DeselectCharacter();
                return;
            }

            if (_state != BattleState.PlayerTurn) return;
            if (character.Faction != 0) return;

            if (Layers.ContainsKey("radial menu"))
            {
                Layers.Remove("radial menu");
            }

            var menu = new RadialMenu(this)
                {
                    CenterX = character.Avatar.Sprite.X + character.Avatar.Sprite.Width/2,
                    CenterY = character.Avatar.Sprite.Y + character.Avatar.Sprite.Height/2,
                    ZIndex = 5000
                };

            var icon = new SpriteObject("Battle/Menu/move");
            SetCharacterMenuAnimations(icon);
            if (character.CanMove)
            {
                icon.MouseOver += (sender, args) =>
                    {
                        if (!character.CanMove) return;
                        ((BattleGridLayer) Layers["battlegrid"]).HighlightGrid(
                            new Point((int) character.Avatar.Location.X, (int) character.Avatar.Location.Y),
                            character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction)),
                            GridHighlight.Selectable
                            );
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
            if (character.CanAct)
            {
                icon.MouseOver += (sender, args) =>
                    {
                        if (!character.CanAct) return;
                        ((BattleGridLayer) Layers["battlegrid"]).HighlightGrid(
                            new Point((int) character.Avatar.Location.X, (int) character.Avatar.Location.Y),
                            character.GetEquippedWeapon().TargetGrid,
                            GridHighlight.Selectable
                            );
                    };
                icon.MouseOut += (sender, args) => ((BattleGridLayer)Layers["battlegrid"]).ResetGrid();
                var ability = Ability.Factory("attack");
                ability.Character = character;
                icon.MouseRelease += SelectAbilityTarget(character, ability);
            }
            else
            {
                icon.MouseOver = (sender, args) => { };
                icon.MouseOut = (sender, args) => { };
                icon.MouseClick = (sender, args) => { };
                icon.MouseRelease = (sender, args) => { };
            }

            menu.AddOption("attack", icon);

            icon = new SpriteObject("Battle/Menu/special");
            SetCharacterMenuAnimations(icon);
            if (character.CanAct)
            {
                icon.MouseRelease += SelectSpecialAbility(character);
            }
            else
            {
                icon.MouseOver = (sender, args) => { };
                icon.MouseOut = (sender, args) => { };
                icon.MouseClick = (sender, args) => { };
                icon.MouseRelease = (sender, args) => { };
            }
            menu.AddOption("special", icon);

            icon = new SpriteObject("Battle/Menu/item");
            SetCharacterMenuAnimations(icon);
            menu.AddOption("item", icon);

            Layers.Add("radial menu", menu);

            _selectedCharacter = character;
            _state = BattleState.CharacterSelected;
        }

        /// <summary>
        /// Deselect a character, hiding the radial menu of options and resetting the battlegrid to normal highlighting.
        /// </summary>
        public void DeselectCharacter()
        {
            if (_state != BattleState.CharacterSelected) return;

            if(Layers.ContainsKey("radial menu"))
            {
                Layers.Remove("radial menu");
            }

            ResetState();

            _state = BattleState.PlayerTurn;
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

        /// <summary>
        /// Create a callback function to process if the player chooses to move a character. This callback will display the movement grid
        /// and bind _aimAbility to a function that moves the character.
        /// </summary>
        /// <param name="character">The character whose movement is being chosen.</param>
        /// <returns></returns>
        private EventHandler<MouseEventArgs> SelectMovementTarget(Combatant character)
        {
            return (sender, args) =>
                {
                    _state = BattleState.AimingAbility;
                    Layers.Remove("radial menu");
                    _aimGrid = character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction));
                    _aimAbility = (x, y) =>
                        {
                            if (BattleBoard.IsOccupied(new Point(x, y)) != -1) return false;

                            var command = new Command
                                {
                                    Character = character, 
                                    Target = new Point(x, y), 
                                    Ability = Ability.Factory("move")
                                };
                            ExecuteCommand(command);
                            

                            return true;
                        };
                };
        }

        /// <summary>
        /// Create a callback function to process if the player chooses to have a character attack. This callback will display the targetting grid
        /// and bind _aimAbility to a function that queues an attack command from the character onto the target.
        /// </summary>
        /// <param name="character">The character whose attack is being chosen.</param>
        /// <param name="ability">The ability currently being aimed.</param>
        /// <returns></returns>
        private EventHandler<MouseEventArgs> SelectAbilityTarget(Combatant character, Ability ability)
        {
            return (sender, args) =>
            {
                _state = BattleState.AimingAbility;
                Layers.Remove("radial menu");
                _aimGrid = ability.GenerateTargetGrid();
                _aimAbility = (x, y) =>
                    {
                        if (BattleBoard.IsOccupied(new Point(x, y)) == -1 || BattleBoard.IsOccupied(new Point(x, y)) != ((ability.AbilityTarget == AbilityTarget.Enemy && character.Faction == 0) ? 1 : 0))
                            return false;

                        ability.Character = character;

                        var command = new Command
                            {
                                Character = character, 
                                Target = new Point(x, y), 
                                Ability = ability
                            };
                        QueuedCommands.Add(command);

                        _state = BattleState.PlayerTurn;

                        return true;
                    };
            };
        }

        private EventHandler<MouseEventArgs> SelectSpecialAbility(Combatant character)
        {
            return (sender, args) =>
                {
                    var radialMenu = ((RadialMenu) Layers["radial menu"]);

                    radialMenu.ClearOptions();

                    foreach (var ability in character.GetAbilities().Where(character.CanUseAbility).Where(a => a.AbilityType == AbilityType.Active))
                    {
                        var tempAbility = ability;

                        if (ability.ManaCost <= character.CurrentMana)
                        {
                            ability.Icon.MouseOver = (o, eventArgs) => PreviewAbility(tempAbility);
                            ability.Icon.MouseOut = (o, eventArgs) =>
                                {
                                    Layers["abilitystat"].Visible = false;
                                    ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
                                };
                            ability.Icon.MouseClick = (o, eventArgs) => { };
                            ability.Icon.MouseRelease = SelectAbilityTarget(character, tempAbility);
                        }
                        else
                        {
                            ability.Icon.MouseRelease =  (o, eventArgs) => { };;
                        }

                        radialMenu.AddOption(ability.Name, ability.Icon);
                    }
                };
        }

        private void PreviewAbility(Ability ability)
        {
            ((AbilityStatLayer) Layers["abilitystat"]).SetAbility(ability);
            Layers["abilitystat"].Visible = true;
            ((BattleGridLayer)Layers["battlegrid"]).HighlightGrid(
                new Point((int)ability.Character.Avatar.Location.X, (int)ability.Character.Avatar.Location.Y), 
                ability.GenerateTargetGrid(), 
                GridHighlight.Selectable
            );
        }

        /// <summary>
        /// Handle a cancel request from the player. This will change behavior depending on the BattleState.
        /// </summary>
        public void Cancel()
        {
            switch(_state)
            {
                case BattleState.CharacterSelected:
                    DeselectCharacter();
                    ResetState();
                    break;
                case BattleState.SelectingAbility:
                    ResetState();
                    break;
                case BattleState.AimingAbility:
                    ResetState();
                    _state = BattleState.PlayerTurn;
                    HideCharacterStats();
                    break;
            }
        }

        private void ResetState()
        {
            ((BattleGridLayer) Layers["battlegrid"]).ResetGrid();
            _aimGrid = new Grid(1, 1);
            _aimAbility = null;
            _selectedCharacter = null;
            HideCharacterStats();
            Layers["abilitystat"].Visible = false;
        }

        public void ExecuteQueuedCommands()
        {
            _state = BattleState.ExecutingCommand;
        }

        public void DisplayHits(List<Hit> hits)
        {
            _hits = hits;
            _state = BattleState.DisplayingHits;
        }

        public void EndPlayerTurn()
        {
            if (_state != BattleState.PlayerTurn) return;

            ChangeFaction(1);
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
        ExecutingCommand,
        DisplayingHits,
        MovingCharacter
    }
}

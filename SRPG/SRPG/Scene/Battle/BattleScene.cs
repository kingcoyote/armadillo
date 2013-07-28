using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SRPG.AI;
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
        /// A queued list of commands awaiting execution. This list is added to when telling a character to attack, use an ability or use an item.
        /// </summary>
        public List<Command> QueuedCommands { get; private set; }
        /// <summary>
        /// When the current command is a move, and it is being executed, this is the coordinates for the character to follow from point A to point B.
        /// </summary>
        private List<Point> _movementCoords;
        /// <summary>
        /// A list of hits that are currently being displayed by a recently executed command.
        /// </summary>
        private List<Hit> _hits = new List<Hit>();

        private BattleState _delayState;
        private float _delayTimer;
        private readonly BattleCommander _commander = new BattleCommander();

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
    
            // get the amount of time, in seconds, since the last frame. this should be 0.016 at 60fps.
            float dt = gameTime.ElapsedGameTime.Milliseconds/1000F;

            // during player turn, show queued commands if possible
            Layers["queuedcommands"].Visible = _state != BattleState.EnemyTurn && QueuedCommands.Count > 0;

            // misc update logic for the current state
            UpdateBattleState(dt);
        }

        /// <summary>
        /// Depending on the current state, this will simply redirect to another function
        /// allowing for custom logic for that state to update itself.
        /// </summary>
        /// <param name="dt">The number of seconds since the last frame, typically 0.016 @ 60fps.</param>
        private void UpdateBattleState(float dt)
        {
            switch(_state)
            {
                case BattleState.EnemyTurn:
                    UpdateBattleStateEnemyTurn();
                    break;
                case BattleState.ExecutingCommand:
                    UpdateBattleStateExecutingCommand();
                    break;
                case BattleState.DisplayingHits:
                    UpdateBattleStateDisplayingHits(dt);
                    break;
                case BattleState.MovingCharacter:
                    UpdateBattleStateMovingCharacter(dt);
                    break;
                case BattleState.Delay:
                    _delayTimer -= dt;
                    if(_delayTimer <= 0)
                    {
                        _state = _delayState;
                    }
                    break;
            }
        }

        private void UpdateBattleStateEnemyTurn()
        {
            var combatants = (from c in BattleBoard.Characters where c.Faction == 1 && c.CanMove select c).ToArray();

            if (combatants.Any())
            {
                _commander.BattleBoard = BattleBoard;

                // find first character that can move
                _selectedCharacter = combatants[0];

                // find the optimal location
                var decision = _commander.CalculateAction(_selectedCharacter);
                
                // create move command to that location
                var moveCommand = new Command
                    {
                        Ability = Ability.Factory("move"),
                        Character = _selectedCharacter,
                        Target = decision.Destination
                    };

                QueuedCommands.Add(decision.Command);
                
                ExecuteCommand(moveCommand);

                return;
            }
            else
            {
                ExecuteQueuedCommands();
            }

            // all enemy players have moved / attacked
            ChangeFaction(0);
        }

        private void UpdateBattleStateDisplayingHits(float dt)
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
                    hit = character.ProcessHit(hit);
                    character.ReceiveHit(hit);
                    var damage = hit.Damage;
                    ((HitLayer) Layers["hitlayer"]).DisplayHit(
                        Math.Abs(damage), 
                        hit.Damage > 0 ? Color.White : Color.LightGreen, 
                        hit.Target
                    );
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
                // all hits are done displaying - did anyone die?
                CheckCharacterDeath();

                // move on to the next command
                _state = BattleState.Delay;
                _delayTimer = 1.0F;
                _delayState = BattleState.ExecutingCommand;

                ResetState();
            }
        }

        private void UpdateBattleStateExecutingCommand()
        {
            if (QueuedCommands.Count > 0)
            {
                // process the next command
                ExecuteCommand(QueuedCommands[0]);
                QueuedCommands.RemoveAt(0);
            }
            else
            {
                // all done with queued commands, return to either Player or Enemy turn
                _state = FactionTurn == 0 ? BattleState.PlayerTurn : BattleState.EnemyTurn;
            }
        }

        private void UpdateBattleStateMovingCharacter(float dt)
        {
            // calculate distance to target
            var x = 0 - (_selectedCharacter.Avatar.Location.X - _movementCoords[0].X);
            var y = 0 - (_selectedCharacter.Avatar.Location.Y - _movementCoords[0].Y);

            // set X/Y speeds
            if (x < -0.1) x = -1;
            if (x > 0.1) x = 1;
            if (y < -0.1) y = -1;
            if (y > 0.1) y = 1;
            _selectedCharacter.Avatar.UpdateVelocity(x, y);

            // move character based on speeds
            _selectedCharacter.Avatar.Location.X += x * dt * _selectedCharacter.Avatar.Speed / 50;
            _selectedCharacter.Avatar.Location.Y += y * dt * _selectedCharacter.Avatar.Speed / 50;

            // if they are at their destination
            if (Math.Abs(x - 0) < 0.05 && Math.Abs(y - 0) < 0.05)
            {
                // snap to the grid to prevent movement errors on the next move
                _selectedCharacter.Avatar.Location.X = _movementCoords[0].X;
                _selectedCharacter.Avatar.Location.Y = _movementCoords[0].Y;

                // remove coordinate from list
                _movementCoords.RemoveAt(0);
            }

            // any more coordinates to go to, or are we at the final destination?
            if (_movementCoords.Count == 0)
            {
                // move to the next state
                _state = _selectedCharacter.Faction == 0 ? BattleState.PlayerTurn : BattleState.EnemyTurn;

                // change the character to have a standing animation
                _selectedCharacter.Avatar.UpdateVelocity(0, 0);
                ResetState();
            }
        }

        private void CheckCharacterDeath()
        {
            // kill off any character with 0 or less health
            var deadCombatants = (from c in BattleBoard.Characters where c.CurrentHealth <= 0 select c).ToArray();

            if (deadCombatants.Length == 0) return;

            foreach(var character in deadCombatants)
            {
                character.Die();
                BattleBoard.Characters.Remove(character);
                ((BattleBoardLayer)Layers["battleboard"]).RemoveCharacter(character);
            }
        }

        /// <summary>
        /// Move layers that are relevant to the battlegrid in relation to the camera.
        /// </summary>
        public void UpdateCamera(float x, float y)
        {
            var layers = new List<string> {"battleboard", "radial menu", "hitlayer"};

            // update all layers that are locked to the camera
            foreach (var layer in layers)
            {
                if (Layers.ContainsKey(layer))
                {
                    Layers[layer].X = x;
                    Layers[layer].Y = y;
                }
            }
        }

        /// <summary>
        /// Initialize a battle sequence by name. This call happens at the start of a battle and is required
        /// for this scene to not die a horrible death.
        /// </summary>
        /// <param name="battleName">Name of the battle to be initialized</param>
        public void SetBattle(string battleName)
        {
            // set up defaults
            
            BattleBoard = new BattleBoard();
            RoundNumber = 0;
            FactionTurn = 0;
            _state = BattleState.PlayerTurn;
            var partyGrid = new List<Point>();

            Layers.Add("battleboard", new BattleBoardLayer(this));

            switch(battleName)
            {
                case "coliseum/halls":
                    ((BattleBoardLayer)Layers["battleboard"]).SetBackground("Zones/Coliseum/Halls/halls");
                    ((BattleBoardLayer)Layers["battleboard"]).SetGrid("Zones/Coliseum/Halls/battle");
                    BattleBoard.Sandbag = Grid.FromBitmap("Zones/Coliseum/Halls/battle");

                    partyGrid.Add(new Point(14,35));
                    partyGrid.Add(new Point(15,35));
                    partyGrid.Add(new Point(13,35));
                    partyGrid.Add(new Point(14,36));
                    partyGrid.Add(new Point(15,36));
                    partyGrid.Add(new Point(13,36));

                    BattleBoard.Characters.Add(GenerateCombatant("Guard 1", "coliseum/guard", new Vector2(13, 31)));
                    BattleBoard.Characters.Add(GenerateCombatant("Guard 2", "coliseum/guard", new Vector2(15, 31)));
                    BattleBoard.Characters.Add(GenerateCombatant("Guard Captain", "coliseum/guard", new Vector2(14, 31)));

                    

                    break;
                default:
                    throw new Exception("unknown battle " + battleName);
            }

            // add party to the party grid for this battle, in order
            for(var i = 0; i < ((SRPGGame)Game).Party.Count; i++)
            {
                var character = ((SRPGGame) Game).Party[i];
                character.Avatar.Location.X = partyGrid[i].X;
                character.Avatar.Location.Y = partyGrid[i].Y;
                BattleBoard.Characters.Add(character);
            }

            ((BattleBoardLayer)Layers["battleboard"]).SetBoard(BattleBoard);

            // center camera on partyGrid[0]
            UpdateCamera(
                0 - partyGrid[0].X*50 + Game.GetInstance().GraphicsDevice.Viewport.Width/2,
                0 - partyGrid[0].Y*50 + Game.GetInstance().GraphicsDevice.Viewport.Height/2
            );

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

            // clear out any commands left un-executed at the end of the turn
            QueuedCommands.Clear();

            // increment round number on player's turn
            if (faction == 0) ChangeRound();
        }

        /// <summary>
        /// Process a round change, including status ailments.
        /// </summary>
        public void ChangeRound()
        {
            RoundNumber++;

            // process begin/end round events on characters
            foreach (var character in BattleBoard.Characters)
            {
                character.BeginRound();
            }
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

                        // run an A* pathfinding algorithm to get a route
                        var coords = grid.Pathfind(new Point((int)command.Character.Avatar.Location.X, (int)command.Character.Avatar.Location.Y), command.Target);

                        // remove any points on the path that don't require a direction change
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

                        // if this character has any queued commands, cancel them upon moving
                        var queuedCommands = (from c in QueuedCommands where c.Character == command.Character select c).ToArray();

                        if(queuedCommands.Length > 0)
                        {
                            QueuedCommands.Remove(queuedCommands[0]);
                            command.Character.CanAct = true;
                        }
                    }
                    break;
                default:
                    var hits = command.Ability.GenerateHits(BattleBoard, command.Target);

                    // remove any hits on characters that no longer exist
                    for (var i = hits.Count - 1; i >= 0; i--)
                    {
                        if(BattleBoard.GetCharacterAt(hits[i].Target) == null)
                        {
                            hits.RemoveAt(i);
                        }
                    }

                    // if this ability still has hits left, process them normally
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
            // if they clicked on the character already being shown, assume they want to close the menu
            if (character == _selectedCharacter)
            {
                DeselectCharacter();
                return;
            }

            // you can only click on your characters, during your turn
            if (_state != BattleState.PlayerTurn) return;
            if (character.Faction != 0) return;

            // if another menu is up, get rid of it
            if (Layers.ContainsKey("radial menu"))
            {
                Layers.Remove("radial menu");
            }

            var menu = new RadialMenu(this)
                {
                    CenterX = character.Avatar.Sprite.X + character.Avatar.Sprite.Width/2,
                    CenterY = character.Avatar.Sprite.Y + character.Avatar.Sprite.Height/2,
                    ZIndex = 5000,
                    OnExit = DeselectCharacter
                };

            // move icon, plus event handlers
            var icon = new SpriteObject("Battle/Menu/move");
            SetCharacterMenuAnimations(icon);
            if (character.CanMove)
            {
                icon.MouseOver += (sender, args) =>
                    {
                        if (!character.CanMove) return;

                        ((BattleBoardLayer) Layers["battleboard"]).SetTargettingGrid(
                            character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction)),
                            new Grid(1, 1)
                            );
                    };
                icon.MouseOut += (sender, args) => ((BattleBoardLayer) Layers["battleboard"]).ResetGrid();
                icon.MouseRelease += SelectAbilityTarget(character, Ability.Factory("move"));
            }
            else
            {
                // if they can't move, this icon does nothing
                icon.MouseOver = (sender, args) => { };
                icon.MouseOut = (sender, args) => { };
                icon.MouseClick = (sender, args) => { };
                icon.MouseRelease = (sender, args) => { };
            }

            menu.AddOption("move", icon);

            // attack icon, plus handlers
            icon = new SpriteObject("Battle/Menu/attack");
            SetCharacterMenuAnimations(icon);
            if (character.CanAct)
            {
                var ability = Ability.Factory("attack");
                ability.Character = character;

                icon.MouseOver += (sender, args) =>
                    {
                        if (!character.CanAct) return;

                        ((BattleBoardLayer) Layers["battleboard"]).SetTargettingGrid(
                            ability.GenerateTargetGrid(BattleBoard.Sandbag.Clone()),
                            new Grid(1, 1)
                            );
                    };
                icon.MouseOut += (sender, args) => ((BattleBoardLayer)Layers["battleboard"]).ResetGrid();
                
                icon.MouseRelease += SelectAbilityTarget(character, ability);
            }
            else
            {
                // if they can't act, this icon does nothing
                icon.MouseOver = (sender, args) => { };
                icon.MouseOut = (sender, args) => { };
                icon.MouseClick = (sender, args) => { };
                icon.MouseRelease = (sender, args) => { };
            }

            menu.AddOption("attack", icon);

            // special abilities icon, plus event handlers
            icon = new SpriteObject("Battle/Menu/special");
            SetCharacterMenuAnimations(icon);
            if (character.CanAct)
            {
                icon.MouseRelease += SelectSpecialAbility(character);
            }
            else
            {
                // if they can't act, this icon does nothing
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

        /// <summary>
        /// Take an icon and set the default behaviors for the radial menu.
        /// this is a very specialized function and shouldn't be used for most things.
        /// </summary>
        /// <param name="icon">The icon being configured.</param>
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

                ((BattleBoardLayer)Layers["battleboard"]).SetTargettingGrid(
                    ability.Name == "Move" ? 
                        character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction)) : 
                        ability.GenerateTargetGrid(BattleBoard.Sandbag.Clone()),
                    ability.GenerateImpactGrid()
                );
                
                ((BattleBoardLayer)Layers["battleboard"]).AllowAim = true;

                _aimAbility = (x, y) =>
                    {
                        // only target enemies with angry spells and allies with friendly spells
                        if (!ability.CanTarget(BattleBoard.IsOccupied(new Point(x, y))))
                            return false;

                        character.Avatar.UpdateVelocity(x - character.Avatar.Location.X, y - character.Avatar.Location.Y);
                        character.Avatar.UpdateVelocity(0, 0);

                        // make sure the ability knows who is casting it. this probably shouldn't
                        // be done here, but there are issues doing it elsewhere.
                        ability.Character = character;

                        var command = new Command
                            {
                                Character = character, 
                                Target = new Point(x, y), 
                                Ability = ability
                            };

                        if(ability.Name == "Move")
                        {
                            ExecuteCommand(command);
                            return true;
                        }

                        character.CanAct = false;
                        QueuedCommands.Add(command);
                        _state = BattleState.Delay;
                        _delayState = BattleState.PlayerTurn;
                        _delayTimer = 0.05F;
                        ResetState();

                        return true;
                    };
            };
        }

        /// <summary>
        /// Show a list of special abilities that a character may use
        /// </summary>
        /// <param name="character">The character being selected for special abilities</param>
        /// <returns>An event handler to execute to show the abilities.</returns>
        private EventHandler<MouseEventArgs> SelectSpecialAbility(Combatant character)
        {
            return (sender, args) =>
                {
                    var radialMenu = ((RadialMenu) Layers["radial menu"]);

                    // delete the current radial menu options, which should be move/attack/special/item for the character.
                    // should.
                    radialMenu.ClearOptions();

                    // go through each ability the character can currently use
                    foreach (var ability in character.GetAbilities().Where(character.CanUseAbility).Where(a => a.AbilityType == AbilityType.Active))
                    {
                        var tempAbility = ability;

                        // only bind event handlers onto abilities that are cheap enough to use
                        if (ability.ManaCost <= character.CurrentMana)
                        {
                            ability.Icon.MouseOver = (o, eventArgs) => PreviewAbility(tempAbility);
                            ability.Icon.MouseOut = (o, eventArgs) =>
                                {
                                    Layers["abilitystat"].Visible = false;
                                    ((BattleBoardLayer) Layers["battleboard"]).ResetGrid();
                                };
                            ability.Icon.MouseClick = (o, eventArgs) => { };
                            ability.Icon.MouseRelease = SelectAbilityTarget(character, tempAbility);
                        }
                        else
                        {
                            ability.Icon.MouseRelease =  (o, eventArgs) => { };
                        }

                        radialMenu.AddOption(ability.Name, ability.Icon);
                    }
                };
        }

        /// <summary>
        /// Show a stats panel indicating the name, mana and description of an ability
        /// </summary>
        /// <param name="ability"></param>
        private void PreviewAbility(Ability ability)
        {
            ((AbilityStatLayer) Layers["abilitystat"]).SetAbility(ability);
            Layers["abilitystat"].Visible = true;
            ((BattleBoardLayer)Layers["battleboard"]).SetTargettingGrid(
                ability.GenerateTargetGrid(BattleBoard.Sandbag.Clone()),
                new Grid(1, 1)
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

        /// <summary>
        /// General purpose function during PlayerTurn state to undo various options and settings
        /// and hide panels.
        /// </summary>
        private void ResetState()
        {
            ((BattleBoardLayer) Layers["battleboard"]).ResetGrid();
            _aimAbility = null;
            _selectedCharacter = null;
            HideCharacterStats();
            Layers["abilitystat"].Visible = false;
        }

        /// <summary>
        /// Change state to execute commands
        /// </summary>
        public void ExecuteQueuedCommands()
        {
            _state = BattleState.ExecutingCommand;
        }

        /// <summary>
        /// Receive a list of hits and change state to display those hits on screen, as well
        /// as process the damage.
        /// </summary>
        /// <param name="hits"></param>
        public void DisplayHits(List<Hit> hits)
        {
            _hits = hits;
            _state = BattleState.DisplayingHits;
        }

        /// <summary>
        /// End the player turn, proceeding to the enemy turn.
        /// </summary>
        public void EndPlayerTurn()
        {
            if (_state != BattleState.PlayerTurn) return;

            ChangeFaction(1);
        }

        public void ExecuteAimAbility(int x, int y)
        {
            if(_aimAbility(x, y))
            {
                ((BattleBoardLayer)Layers["battleboard"]).ResetGrid();
            }
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
        MovingCharacter,
        Delay
    }
}

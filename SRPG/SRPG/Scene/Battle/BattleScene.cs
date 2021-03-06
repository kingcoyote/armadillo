﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Visuals.Flat;
using SRPG.AI;
using SRPG.Data;
using SRPG.Data.Layers;
using Torch;
using Torch.UserInterface;
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
        private Func<int, int, bool> _aimAbility;

        private DateTime _aimTime;
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
        private readonly BattleCommander _commander;

        private CharacterStatsDialog _characterStats;
        private HUDDialog _hud;
        private QueuedCommandsDialog _queuedCommands;
        private AbilityStatDialog _abilityStatLayer;
        private BattleBoardLayer _battleBoardLayer;
        private RadialMenuControl _radialMenuControl;
        private DialogLayer _dialog;

        private Dialog _startingDialog;

        private readonly Dictionary<string, int> _displayedHits = new Dictionary<string, int>();
        private readonly Dictionary<string, float> _hitLocations = new Dictionary<string, float>();
        private readonly Dictionary<string, LabelControl> _hitLabels = new Dictionary<string, LabelControl>();

        private float _x;
        private float _y;

        public BattleScene(Game game) : base(game)
        {
            QueuedCommands = new List<Command>();
            _commander = new BattleCommander(game);
        }

        /// <summary>
        /// Pre-battle initialization sequence to load characters, the battleboard and the image layers.
        /// </summary>
        protected override void OnEntered()
        {
            base.OnEntered();

            QueuedCommands = new List<Command>();

            _characterStats = new CharacterStatsDialog();
            Gui.Screen.Desktop.Children.Add(_characterStats);

            _hud = new HUDDialog();
            _hud.EndTurnPressed += EndPlayerTurn;
            Gui.Screen.Desktop.Children.Add(_hud);

            _queuedCommands = new QueuedCommandsDialog(QueuedCommands);
            _queuedCommands.ExecuteClicked += ExecuteQueuedCommands;

            _abilityStatLayer = new AbilityStatDialog();

            Game.IsMouseVisible = true;

            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");

            ((FlatGuiVisualizer)Gui.Visualizer).RendererRepository.AddAssembly(typeof(FlatImageButtonControlRenderer).Assembly);
            ((FlatGuiVisualizer)Gui.Visualizer).RendererRepository.AddAssembly(typeof(FlatTiledIconControlRenderer).Assembly);
            ((FlatGuiVisualizer)Gui.Visualizer).RendererRepository.AddAssembly(typeof(FlatRadialButtonControlRenderer).Assembly);
            ((FlatGuiVisualizer)Gui.Visualizer).RendererRepository.AddAssembly(typeof(FlatQueuedCommandControlRenderer).Assembly);

            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, Cancel);
            Components.Add(keyboard);

            if (_startingDialog != null)
            {
                StartDialog(_startingDialog);
                _startingDialog = null;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            Game.IsMouseVisible = true;
            
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            if (_radialMenuControl != null)
            {
                _radialMenuControl.Update(gametime);
            }

            _battleBoardLayer.FreeAim = _state == BattleState.PlayerTurn;

            // get the amount of time, in seconds, since the last frame. this should be 0.016 at 60fps.
            float dt = gametime.ElapsedGameTime.Milliseconds/1000F;

            // during player turn, show queued commands if possible
            if(FactionTurn == 0 && QueuedCommands.Count > 0)
            {
                ShowGui(_queuedCommands);
            } else
            {
                HideGui(_queuedCommands);   
            }

            _hud.SetStatus(RoundNumber, FactionTurn);

            // misc update logic for the current state
            UpdateBattleState(dt);
            UpdateHits(dt);
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
                case BattleState.Dialog:
                    UpdateBattleStateDialog(dt);
                    break;
            }
        }

        private void UpdateBattleStateEnemyTurn()
        {
            var combatants = (from c in BattleBoard.Characters where c.Faction == 1 && c.CanMove select c).ToArray();

            if (combatants.Any())
            {
                // find first character that can move
                _selectedCharacter = combatants[0];

                // find the optimal location
                var decision = _commander.CalculateAction(_selectedCharacter);
                
                // create move command to that location
                var moveCommand = new Command
                    {
                        Ability = Ability.Factory(Game, "move"),
                        Character = _selectedCharacter,
                        Target = decision.Destination
                    };

                QueuedCommands.Add(moveCommand);

                if (decision.Command.Ability != null)
                    QueuedCommands.Add(decision.Command);

                ExecuteQueuedCommands();

                return;
            }

            ExecuteQueuedCommands();

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
                    DisplayHit(
                        Math.Abs(damage), 
                        hit.Damage > 0 ? "damage" : "healing", 
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
                _state = FactionTurn == 0 ? BattleState.PlayerTurn : BattleState.ExecutingCommand;

                // change the character to have a standing animation
                _selectedCharacter.Avatar.UpdateVelocity(0, 0);
                ResetState();
            }
        }

        private void UpdateBattleStateDialog(float dt)
        {
            
        }

        private void UpdateHits(float dt)
        {
            for (var i = _displayedHits.Count; i > 0; i--)
            {
                var key = _displayedHits.Keys.ElementAt(i - 1);

                _displayedHits[key] += (int)(dt * 1000);
                if (_displayedHits[key] > 1200)
                {
                    Gui.Screen.Desktop.Children.Remove(_hitLabels[key]);
                    _hitLabels.Remove(key);
                    _displayedHits.Remove(key);
                }
                else
                {

                    _hitLocations[key] -= 75F * dt;
                    _hitLabels[key].Bounds.Location.Y.Offset = (int)_hitLocations[key];
                }
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
                _battleBoardLayer.RemoveCharacter(character);
            }
        }

        /// <summary>
        /// Move layers that are relevant to the battlegrid in relation to the camera.
        /// </summary>
        public void UpdateCamera(float x, float y)
        {
            var layers = new List<Layer> {_battleBoardLayer};

            if (_battleBoardLayer.Width < Game.GraphicsDevice.Viewport.Width)
            {
                x = Game.GraphicsDevice.Viewport.Width/2 - _battleBoardLayer.Width/2;
            }

            if (_battleBoardLayer.Height < Game.GraphicsDevice.Viewport.Height)
            {
                y = Game.GraphicsDevice.Viewport.Height / 2 - _battleBoardLayer.Height / 2;
            }

            _x = x;
            _y = y;

            // update all layers that are locked to the camera
            foreach (var layer in layers)
            {
                if (layer != null && layer.Visible)
                {
                    layer.X = x;
                    layer.Y = y;
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

            _battleBoardLayer = new BattleBoardLayer(this, null);
            _battleBoardLayer.CharacterSelected += SelectCharacter;

            Components.Add(_battleBoardLayer);

            switch(battleName)
            {
                case "coliseum/halls":
                    _battleBoardLayer.SetBackground("Zones/Coliseum/Halls/north");
                    _battleBoardLayer.SetGrid("Zones/Coliseum/Halls/battle");
                    BattleBoard.Sandbag = Grid.FromBitmap(Game.Services, "Zones/Coliseum/Halls/battle");
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/Halls/north"));
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/pillar") { X = 393, Y = 190, DrawOrder = 248 });
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/pillar") { X = 393, Y = 390, DrawOrder = 448 });
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/pillar") { X = 393, Y = 590, DrawOrder = 648 });
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/pillar") { X = 593, Y = 190, DrawOrder = 248 });
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/pillar") { X = 593, Y = 390, DrawOrder = 448 });
                    _battleBoardLayer.AddImage(new ImageObject(Game, _battleBoardLayer, "Zones/Coliseum/pillar") { X = 593, Y = 590, DrawOrder = 648 });

                    partyGrid.Add(new Point(10,13));
                    partyGrid.Add(new Point(11,13));
                    partyGrid.Add(new Point(9,13));
                    partyGrid.Add(new Point(10,12));
                    partyGrid.Add(new Point(11,12));
                    partyGrid.Add(new Point(9,12));

                    BattleBoard.Characters.Add(GenerateCombatant("Guard 1", "coliseum/guard", new Vector2(9, 3)));
                    BattleBoard.Characters.Add(GenerateCombatant("Guard 2", "coliseum/guard", new Vector2(11, 3)));
                    BattleBoard.Characters.Add(GenerateCombatant("Guard Captain", "coliseum/guard", new Vector2(10, 3)));

                    BattleBoard.Characters.Add(GenerateCombatant("Guard 3", "coliseum/guard", new Vector2(9, 10)));
                    BattleBoard.Characters.Add(GenerateCombatant("Guard 4", "coliseum/guard", new Vector2(11, 10)));
                    BattleBoard.Characters.Add(GenerateCombatant("Guard 5", "coliseum/guard", new Vector2(10, 10)));

                    _startingDialog = Dialog.Fetch("coliseum", "guards");

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

            _battleBoardLayer.SetBoard(BattleBoard);
            _commander.BattleBoard = BattleBoard;

            // center camera on partyGrid[0]
            UpdateCamera(
                0 - partyGrid[0].X*50 + Game.GraphicsDevice.Viewport.Width/2,
                0 - partyGrid[0].Y*50 + Game.GraphicsDevice.Viewport.Height/2
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

                        if (FactionTurn == 1) break;

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

                    _commander.RecordCommand(command, hits);

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
            if (_state != BattleState.PlayerTurn && _state != BattleState.AimingAbility) return;

            _characterStats.SetCharacter(character);
            ShowGui(_characterStats);
        }

        /// <summary>
        /// Hide the HUD element created by ShowCharacterStats
        /// </summary>
        public void HideCharacterStats()
        {
            if (_state != BattleState.PlayerTurn && _state != BattleState.AimingAbility) return;

            HideGui(_characterStats);
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
            var combatant = Combatant.FromTemplate(Game, template);
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
            
            // you can only click on your characters during your turn
            if (_state != BattleState.PlayerTurn) return;
            if (character.Faction != 0) return;

            if (_radialMenuControl != null)
            {
                Gui.Screen.Desktop.Children.Remove(_radialMenuControl);
                _radialMenuControl = null;
            }


            var menu = new RadialMenuControl(((IInputService)Game.Services.GetService(typeof(IInputService))).GetMouse())
                {
                    CenterX = (int)character.Avatar.Sprite.X + character.Avatar.Sprite.Width/2 - 10,
                    CenterY = (int)character.Avatar.Sprite.Y + character.Avatar.Sprite.Height/2,
                    OnExit = DeselectCharacter
                };

            // move icon, plus event handlers
            var icon = new RadialButtonControl {ImageFrame = "move", Bounds = new UniRectangle(0, 0, 64, 64)};
            if (character.CanMove)
            {
                icon.MouseOver += () =>
                    {
                        if (!character.CanMove) return;

                        _battleBoardLayer.SetTargettingGrid(
                            character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction)),
                            new Grid(1, 1)
                            );
                    };
                icon.MouseOut += () => { if (_aimAbility == null) _battleBoardLayer.ResetGrid(); };
                icon.MouseClick += () => { SelectAbilityTarget(character, Ability.Factory(Game, "move"));
                                             _aimTime = DateTime.Now;
                };
            }
            else
            {

                // if they can't move, this icon does nothing
                icon.MouseOver = () => { };
                icon.MouseOut = () => { };
                icon.MouseClick = () => { };
                icon.MouseRelease = () => { };
                icon.Enabled = false;
            }

            menu.AddOption("move", icon);

            //// attack icon, plus handlers
            icon = new RadialButtonControl { ImageFrame = "attack", Bounds = new UniRectangle(0, 0, 64, 64) };
            if (character.CanAct)
            {
                var ability = Ability.Factory(Game, "attack");
                ability.Character = character;

                icon.MouseOver += () =>
                    {
                        if (!character.CanAct) return;

                        _battleBoardLayer.SetTargettingGrid(
                            ability.GenerateTargetGrid(BattleBoard.Sandbag.Clone()),
                            new Grid(1, 1)
                            );
                    };
                icon.MouseOut += () => { if (_aimAbility == null) _battleBoardLayer.ResetGrid(); };

                icon.MouseRelease += () => { SelectAbilityTarget(character, ability);
                                               _aimTime = DateTime.Now;
                };
            }
            else
            {
                // if they can't act, this icon does nothing
                icon.MouseOver = () => { };
                icon.MouseOut = () => { };
                icon.MouseClick = () => { };
                icon.MouseRelease = () => { };
                icon.Enabled = false;
            }

            menu.AddOption("attack", icon);

            //// special abilities icon, plus event handlers
            icon = new RadialButtonControl { ImageFrame = "special", Bounds = new UniRectangle(0, 0, 64, 64) };
            if (character.CanAct)
            {
                icon.MouseRelease += () => SelectSpecialAbility(character);  
            }
            else
            {
                // if they can't act, this icon does nothing
                icon.MouseOver = () => { };
                icon.MouseOut = () => { };
                icon.MouseClick = () => { };
                icon.MouseRelease = () => { };
                icon.Enabled = false;
            }
            menu.AddOption("special", icon);

            icon = new RadialButtonControl
                {ImageFrame = "item", Bounds = new UniRectangle(0, 0, 64, 64), Enabled = false};
            menu.AddOption("item", icon);

            _radialMenuControl = menu;
            Gui.Screen.Desktop.Children.Add(_radialMenuControl);

            _selectedCharacter = character;
            _state = BattleState.CharacterSelected;
        }

        /// <summary>
        /// Deselect a character, hiding the radial menu of options and resetting the battlegrid to normal highlighting.
        /// </summary>
        public void DeselectCharacter()
        {
            if (_state != BattleState.CharacterSelected) return;

            if (Gui.Screen.Desktop.Children.Contains(_radialMenuControl))
            {
                Gui.Screen.Desktop.Children.Remove(_radialMenuControl);
                _radialMenuControl = null;
            }

            ResetState();

            _state = BattleState.PlayerTurn;
        }

        /// <summary>
        /// Create a callback function to process if the player chooses to have a character attack. This callback will display the targetting grid
        /// and bind _aimAbility to a function that queues an attack command from the character onto the target.
        /// </summary>
        /// <param name="character">The character whose attack is being chosen.</param>
        /// <param name="ability">The ability currently being aimed.</param>
        /// <returns></returns>
        private void SelectAbilityTarget(Combatant character, Ability ability)
        {

            _state = BattleState.AimingAbility;
            Gui.Screen.Desktop.Children.Remove(_radialMenuControl);


            _battleBoardLayer.SetTargettingGrid(
                ability.Name == "Move" ? 
                    character.GetMovementGrid(BattleBoard.GetAccessibleGrid(character.Faction)) : 
                    ability.GenerateTargetGrid(BattleBoard.Sandbag.Clone()),
                ability.GenerateImpactGrid()
            );
                
            _battleBoardLayer.AbilityAim = true;

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
                    _queuedCommands.UpdateControls();
                    _state = BattleState.Delay;
                    _delayState = BattleState.PlayerTurn;
                    _delayTimer = 0.05F;
                    ResetState();

                    return true;
                };
        }

        /// <summary>
        /// Show a list of special abilities that a character may use
        /// </summary>
        /// <param name="character">The character being selected for special abilities</param>
        /// <returns>An event handler to execute to show the abilities.</returns>
        private void SelectSpecialAbility(Combatant character)
        {
            // delete the current radial menu options, which should be move/attack/special/item for the character.
            // should.
            _radialMenuControl.ClearOptions();

            // go through each ability the character can currently use
            foreach (var ability in character.GetAbilities().Where(character.CanUseAbility).Where(a => a.AbilityType == AbilityType.Active))
            {
                var tempAbility = ability;

                var button = new RadialButtonControl
                    {ImageFrame = ability.ImageName, Bounds = new UniRectangle(0, 0, 64, 64), Enabled = false};

                // only bind event handlers onto abilities that are cheap enough to use
                if (ability.ManaCost <= character.CurrentMana)
                {
                    button.Enabled = true;
                    button.MouseOver = () => PreviewAbility(tempAbility);
                    button.MouseOut = () =>
                        {
                            if (_aimAbility != null) return;

                            HideGui(_abilityStatLayer);
                            _battleBoardLayer.ResetGrid();
                        };
                    button.MouseClick = () => { };
                    button.MouseRelease += () => { SelectAbilityTarget(character, tempAbility);
                                                     _aimTime = DateTime.Now;
                    };
                }
                else
                {
                    button.MouseRelease =  () => { };
                }

                _radialMenuControl.AddOption(ability.Name, button);
            }
                
        }

        /// <summary>
        /// Show a stats panel indicating the name, mana and description of an ability
        /// </summary>
        /// <param name="ability"></param>
        private void PreviewAbility(Ability ability)
        {
            _abilityStatLayer.SetAbility(ability);
            ShowGui(_abilityStatLayer);
            _battleBoardLayer.SetTargettingGrid(
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
            _battleBoardLayer.ResetGrid();
            _aimAbility = null;
            _selectedCharacter = null;
            HideCharacterStats();
            HideGui(_abilityStatLayer);
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
            if (DateTime.Now.Subtract(_aimTime).TotalMilliseconds <= 250) return;
            if (_aimAbility == null) return;

            if(_aimAbility(x, y))
            {
                _battleBoardLayer.ResetGrid();
            }
        }

        public void DisplayHit(int amount, string style, Point target)
        {
            var key = string.Format(
                "hit/{0}/{1}/{2},{3}/{4}",
                amount,
                style,
                target.X,
                target.Y,
                new Random().Next()
            );

            var label = new StyledTextControl
                {
                    Text = amount.ToString(),
                    Bounds = new UniRectangle(_x + target.X*50 + 5, _y + target.Y*50 - 75, 75, 30),
                    Font = "hitlabel." + style
                };
            Gui.Screen.Desktop.Children.Add(label);
            _hitLabels.Add(key, label);
            _hitLocations.Add(key, target.Y * 50 - 75);
            _displayedHits.Add(key, 0);
        }

        public void StartDialog(Dialog dialog)
        {
            dialog.OnExit += EndDialogEvent;
            _dialog = new DialogLayer(this, dialog);
            Gui.Screen.Desktop.Children.Add(_dialog);
            
            _state = BattleState.Dialog;
            _battleBoardLayer.FreeAim = false;
            _battleBoardLayer.AbilityAim = false;
            HideGui(_characterStats);
            HideGui(_hud);
        }

        public void EndDialog()
        {
            _state = BattleState.PlayerTurn;
            Gui.Screen.Desktop.Children.Remove(_dialog);
            _dialog = null;
            ShowGui(_hud);
        }

        public void EndDialogEvent(object sender, EventArgs args)
        {
            EndDialog();
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
        Delay,
        Dialog
    }
}

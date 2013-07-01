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

        public BattleScene(Game game) : base(game) { }

        private Dictionary<Direction, bool> _camDirections = new Dictionary<Direction, bool>() 
            { {Direction.Up, false}, {Direction.Right, false}, {Direction.Down, false}, {Direction.Left, false} }; 

        private const int CamScrollSpeed = 450;

        private float _x;
        private float _y;

        /// <summary>
        /// Pre-battle initialization sequence to load characters, the battleboard and the image layers.
        /// </summary>
        public override void Initialize()
        {
            Layers.Add("character stats", new CharacterStats(this) { ZIndex = 5000, Visible = false});
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

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            Layers["battlegrid"].X = _x;
            Layers["battlegrid"].Y = _y;

            Layers["battleboardlayer"].X = _x;
            Layers["battleboardlayer"].Y = _y;
        }

        public void SetBattle(string battleName)
        {
            BattleBoard = new BattleBoard();

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

                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard1", new Vector2(5, 5)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard2", new Vector2(5, 6)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard3", new Vector2(5, 7)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard4", new Vector2(5, 8)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard5", new Vector2(5, 9)));

                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard6", new Vector2(34, 22)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard7", new Vector2(33, 21)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard8", new Vector2(35, 21)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard9", new Vector2(33, 23)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard10", new Vector2(35, 23)));

                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard11", new Vector2(14, 21)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard12", new Vector2(15, 21)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard13", new Vector2(13, 21)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard14", new Vector2(13, 23)));
                    BattleBoard.Characters.Add(GenerateCombatant("enemy", "guard15", new Vector2(15, 23)));

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
        }

        /// <summary>
        /// Process a faction change, including swapping out UI elements and preparing for the AI / human input.
        /// </summary>
        /// <param name="faction"></param>
        public void ChangeFaction(int faction)
        {
            throw new NotImplementedException();
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
            ((CharacterStats) Layers["character stats"]).SetCharacter(character);
            Layers["character stats"].Visible = true;
        }

        public void HideCharacterStats(Combatant character)
        {
            if (((CharacterStats)Layers["character stats"]).Character == character)
            {
                Layers["character stats"].Visible = false;
            }
        }

        private Combatant GenerateCombatant(string className, string charName, Vector2 location)
        {
            var combatant = new Combatant();
            combatant.MaxHealth = 20;
            combatant.CurrentHealth = 20;
            combatant.MaxMana = 20;
            combatant.CurrentMana = 20;
            combatant.Name = charName;
            combatant.Avatar = CharacterClass.GenerateCharacter(className);
            combatant.Class = new CharacterClass(className, ItemType.Sword, ItemType.Plate);
            combatant.Avatar.Location = location;

            return combatant;
        }
    }
}

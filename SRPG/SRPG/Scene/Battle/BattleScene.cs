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

        }

        public override void Update(GameTime gameTime, Input input)
        {
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

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            Layers["bg"].X = _x;
            Layers["bg"].Y = _y;

            Layers["battlegrid"].X = _x;
            Layers["battlegrid"].Y = _y;
        }

        public void SetBattle(string battleName)
        {
            switch(battleName)
            {
                case "coliseum/hall":
                    Layers.Add("bg", new BackgroundLayer(this, "Zones/Coliseum/Halls/halls"));
                    Layers.Add("battlegrid", new BattleGridLayer(this, "Zones/Coliseum/Halls/battle"));
                    break;
            }
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
    }
}

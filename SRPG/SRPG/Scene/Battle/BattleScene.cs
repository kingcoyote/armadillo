using System;
using System.Collections.Generic;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class BattleScene : Torch.Scene
    {
        public BattleBoard BattleBoard;
        public int FactionTurn;
        public List<SpriteObject> ImageLayers;
        public bool AwaitingAction;

        public BattleScene(Game game) : base(game) { }

        /// <summary>
        /// Pre-battle initialization sequence to load characters, the battleboard and the image layers.
        /// </summary>
        public override void Initialize()
        {
            throw new NotImplementedException();
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

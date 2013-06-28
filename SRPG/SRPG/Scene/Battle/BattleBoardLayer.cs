using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class BattleBoardLayer : Layer
    {
        private BattleBoard _board;

        public BattleBoardLayer(Torch.Scene scene, BattleBoard board) : base(scene)
        {
            _board = board;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            ClearByName("character");

            foreach(var character in _board.Characters)
            {
                character.Avatar.Sprite.X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2);
                character.Avatar.Sprite.Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2);
                character.Avatar.Sprite.Z = character.Avatar.Sprite.Y;
                Objects.Add("character/" + character.Name, character.Avatar.Sprite);
            }
        }
    }
}

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
                character.Sprite.Sprite.X = (int)(character.Sprite.Location.X * 50 + 25 - character.Sprite.Sprite.Width/2);
                character.Sprite.Sprite.Y = (int)(character.Sprite.Location.Y * 50 + 25 - character.Sprite.Sprite.Height + character.Sprite.GetFeet().Height / 2);
                character.Sprite.Sprite.Z = character.Sprite.Sprite.Y;
                Objects.Add("character/" + character.Name, character.Sprite.Sprite);
            }
        }
    }
}

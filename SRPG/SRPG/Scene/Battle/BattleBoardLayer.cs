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

            foreach (var character in _board.Characters)
            {
                character.Avatar.Sprite.X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width / 2);
                character.Avatar.Sprite.Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2);
                character.Avatar.Sprite.Z = character.Avatar.Sprite.Y;
                Objects.Add("character/" + character.Name, character.Avatar.Sprite);
                Objects["character/" + character.Name].MouseOver = MouseOverCharacter(character);
                Objects["character/" + character.Name].MouseOut = MouseOutCharacter(character);
                Objects["character/" + character.Name].MouseClick = MouseClickCharacter(character);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            foreach(var character in _board.Characters)
            {
                Objects["character/" + character.Name].X = (int)(character.Avatar.Location.X * 50 + 25 - character.Avatar.Sprite.Width/2);
                Objects["character/" + character.Name].Y = (int)(character.Avatar.Location.Y * 50 + 25 - character.Avatar.Sprite.Height + character.Avatar.GetFeet().Height / 2);
                Objects["character/" + character.Name].Z = character.Avatar.Sprite.Y;
            }
        }

        public EventHandler<MouseEventArgs> MouseOverCharacter(Combatant character)
        {
            return (sender, args) =>
                {
                    ((BattleScene) Scene).ShowCharacterStats(character);
                };
        }

        public EventHandler<MouseEventArgs> MouseOutCharacter(Combatant character)
        {
            return (sender, args) =>
                {
                    ((BattleScene) Scene).HideCharacterStats(character);
                };
        }

        public EventHandler<MouseEventArgs> MouseClickCharacter(Combatant character)
        {
            return (sender, args) => { };
        }
    }
}

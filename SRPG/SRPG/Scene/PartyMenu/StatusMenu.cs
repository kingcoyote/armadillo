using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class StatusMenu : Layer
    {
        public StatusMenu(Torch.Scene scene) : base(scene)
        {
            var game = ((SRPGGame)Torch.Game.GetInstance());
            var font = game.Content.Load<SpriteFont>("menu");


            for (var i = 0; i < game.Party.Count; i++)
            {
                var character = game.Party[i];
                Objects.Add("party " + character.Name, new TextObject
                    {
                        Font = font,
                        Value = character.Name,
                        X = 50,
                        Y = 50 + (50 * i),
                        Color = Color.White
                    });
                Objects["party " + character.Name].MouseClick += (sender, args) =>
                    {
                        ((PartyMenuScene) Scene).SetCharacter(character);
                    };
            }
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            ((PartyMenuScene) Scene).AnchorLayer("character menu", (int)X + 225, (int)Y);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Battle
{
    class CharacterStats : Layer
    {
        public Combatant Character { get; private set; }
        
        public CharacterStats(Torch.Scene scene) : base(scene)
        {
            var font = Game.GetInstance().Content.Load<SpriteFont>("menu");

            var x = Game.GetInstance().GraphicsDevice.Viewport.Width - 450;
            var y = 50;

            Objects.Add("box", new TextureObject { Color = Color.Blue, X = x, Y = y, Width = 400, Height = 600 });
            Objects.Add("name", new TextObject { Font = font, X = x + 25, Y = y + 25, Color = Color.White, Value = "" });
        }

        public void SetCharacter(Combatant character)
        {
            Character = character;

            ((TextObject) Objects["name"]).Value = Character.Name;
        }
    }
}

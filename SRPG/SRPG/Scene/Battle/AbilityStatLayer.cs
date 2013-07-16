using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class AbilityStatLayer : Layer
    {
        public Ability Ability;

        public AbilityStatLayer(Torch.Scene scene) : base(scene)
        {
            var x = Torch.Game.GetInstance().GraphicsDevice.Viewport.Width - 425;
            var y = Torch.Game.GetInstance().GraphicsDevice.Viewport.Height - 225;

            var font = FontManager.Get("Menu");

            Objects.Add("box", new TextureObject { Color = Color.Blue, X = x, Y = y, Width = 400, Height = 200 });
            Objects.Add("name", new TextObject { Font = font, Color = Color.White, Value = "", X = x + 10, Y = y + 10 });
            Objects.Add("mana", new TextObject { Font = font, Color = Color.White, Value = "", X = x + 400 - 10, Y = y + 10, Alignment = TextObject.AlignTypes.Right });

            // ability name
            // mana cost
            // description
        }

        public void SetAbility(Ability ability)
        {
            Ability = ability;
            UpdateText();
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            if (Ability == null) return;

            UpdateText();
        }

        private void UpdateText()
        {
            ((TextObject) Objects["name"]).Value = Ability.Name;
            ((TextObject) Objects["mana"]).Value = Ability.ManaCost.ToString();
        }
    }
}

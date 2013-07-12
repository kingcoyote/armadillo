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
            var font = FontManager.Get("Menu");

            var x = Game.GetInstance().GraphicsDevice.Viewport.Width - 425;
            var y = 25;

            Objects.Add("box", new TextureObject { Color = Color.Blue, X = x, Y = y, Width = 400, Height = 200, Z = -1 });
            
            Objects.Add("name", new TextObject { Font = font, X = x + 25, Y = y + 25, Color = Color.White, Value = "" });
            Objects.Add("class", new TextObject { Font = font, X = x + 375, Y = y + 25, Color = Color.White, Value = "", Alignment = TextObject.AlignTypes.Right });

            Objects.Add("health bar back", new TextureObject { Color = Color.Black, Height = 25, X = x + 25, Y = y + 75, Width = 300, Z = 0 });
            Objects.Add("mana bar back", new TextureObject { Color = Color.Black, Height = 25, X = x + 25, Y = y + 125, Width = 300, Z = 0 });

            Objects.Add("health bar", new TextureObject { Color = Color.Green, Height = 25, X = x + 25, Y = y + 75, Width = 300, Z = 1 });
            Objects.Add("mana bar", new TextureObject { Color = Color.Purple, Height = 25, X = x + 25, Y = y + 125, Width = 300, Z = 1 });

            Objects.Add("health num", new TextObject() { Color = Color.White, Font = font, Value = "", X = x + 375, Alignment = TextObject.AlignTypes.Right, Y = y + 68 });
            Objects.Add("mana num", new TextObject() { Color = Color.White, Font = font, Value = "", X = x + 375, Alignment = TextObject.AlignTypes.Right, Y = y + 118 });
        }

        public void SetCharacter(Combatant character)
        {
            Character = character;

            ((TextObject)Objects["name"]).Value = Character.Name;
            ((TextObject)Objects["class"]).Value = Character.Class;

            ((TextObject)Objects["health num"]).Value = Character.CurrentHealth.ToString();
            ((TextObject)Objects["mana num"]).Value = Character.CurrentMana.ToString();

            Objects["health bar"].Width = (int)((Character.CurrentHealth/(float)Character.MaxHealth)*300);
            Objects["mana bar"].Width = (int)((Character.CurrentMana / (float)Character.MaxMana) * 300);

        }
    }
}

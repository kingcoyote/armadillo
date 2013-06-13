using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.PartyMenu
{
    class StatusMenu : Layer
    {
        private Character _character;

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
                Objects["party " + character.Name].MouseClick += (sender, args) => ((PartyMenuScene) Scene).SetCharacter(character);
            }
        }

        public void SetCharacter(Character character)
        {
            _character = character;
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            ClearObjects();

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            // labels
            Objects.Add("class label", new TextObject() { Font = font, X = 225, Y = 50, Value = "Class", Color = Color.White });
            Objects.Add("weapon label", new TextObject() { Font = font, X = 225, Y = 100, Value = "Weapon", Color = Color.White });
            Objects.Add("armor label", new TextObject() { Font = font, X = 225, Y = 150, Value = "Armor", Color = Color.White });

            // values
            //Objects.Add("portrait", new ImageObject(_character.Portrait));
            Objects.Add("class", new TextObject() { Font = font, Y = 50, X = 450, Value = _character.Class.Name, Color = Color.Yellow });
            var weaponName = _character.GetEquippedWeapon() != null ? _character.GetEquippedWeapon().Name : "--";
            Objects.Add("weapon", new TextObject() { Font = font, Y = 100, X = 450, Value = weaponName, Color = Color.Yellow });
            var armorName = _character.GetEquippedArmor() != null ? _character.GetEquippedArmor().Name : "--";
            Objects.Add("armor", new TextObject() { Font = font, Y = 150, X = 450, Value = armorName, Color = Color.Yellow });

            // when you click on one of the equipped items, display a list of items that can be used in that slot...
            Objects["weapon"].MouseClick += ChangeWeapon;
            Objects["armor"].MouseClick += ChangeArmor;
        }

        private void ClearObjects()
        {
            Objects.Remove("class label");
            Objects.Remove("weapon label");
            Objects.Remove("armor label");
            Objects.Remove("class");
            Objects.Remove("weapon");
            Objects.Remove("armor");
        }

        private void ChangeWeapon(object sender, MouseEventArgs args)
        {
            var weapons = (from item in ((SRPGGame) Game.GetInstance()).Inventory where item.ItemType == _character.Class.WeaponTypes select item);
            GenerateItemMenu(weapons.ToList());
        }

        private void ChangeArmor(object sender, MouseEventArgs args)
        {
            var armors = (from item in ((SRPGGame)Game.GetInstance()).Inventory where item.ItemType == _character.Class.ArmorTypes select item);
            GenerateItemMenu(armors.ToList());
        }

        private void GenerateItemMenu(IEnumerable<Item> items)
        {
            var oldKeys = (from key in Objects.Keys where key.Length > 15 && key.Substring(0, 15) == "selectable item" select key);

            foreach(var key in oldKeys.ToList())
            {
                Objects.Remove(key);
            }
            
            var i = 0;

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            foreach(var item in items.ToList())
            {
                var tempItem = item;
                
                Objects.Add("selectable item " + i, new TextObject()
                    {
                        Font = font,
                        Value = item.Name,
                        X = 675,
                        Y = 50 * (i + 1),
                        Color = Color.White
                    });
                
                Objects["selectable item " + i].MouseClick += (sender, args) =>
                    {
                        ((SRPGGame) Game.GetInstance()).EquipCharacter(_character, tempItem);
                        UpdateObjects();
                        for(var j = 0; j < items.Count(); j++)
                        {
                            Objects.Remove("selectable item " + j);
                        }
                    };

                i++;
            }
        }
    }
}

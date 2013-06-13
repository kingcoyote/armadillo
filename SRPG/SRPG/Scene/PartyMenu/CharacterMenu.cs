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
    class CharacterMenu : Layer
    {
        private Character _character;

        public CharacterMenu(Torch.Scene scene) : base(scene) { }

        public void SetCharacter(Character character)
        {
            _character = character;
            UpdateObjects();
        }

        private void UpdateObjects()
        {
            Objects.Clear();

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            // labels
            Objects.Add("class label", new TextObject() { Font = font, Y = 50, Value = "Class", Color = Color.White });
            Objects.Add("weapon label", new TextObject() { Font = font, Y = 100, Value = "Weapon", Color = Color.White });
            Objects.Add("armor label", new TextObject() { Font = font, Y = 150, Value = "Armor", Color = Color.White });
            

            // values
            //Objects.Add("portrait", new ImageObject(_character.Portrait));
            Objects.Add("class", new TextObject() { Font = font, Y = 50, X = 225, Value = _character.Class.Name, Color = Color.Yellow });
            var weaponName = _character.GetEquippedWeapon() != null ? _character.GetEquippedWeapon().Name : "--";
            Objects.Add("weapon", new TextObject() { Font = font, Y = 100, X = 225, Value = weaponName, Color = Color.Yellow });
            var armorName = _character.GetEquippedArmor() != null ? _character.GetEquippedArmor().Name : "--";
            Objects.Add("armor", new TextObject() { Font = font, Y = 150, X = 225, Value = armorName, Color = Color.Yellow });

            Objects["weapon"].MouseClick += ChangeWeapon;
            Objects["armor"].MouseClick += ChangeArmor;
        }

        private void ChangeWeapon(object sender, MouseEventArgs args)
        {
            var weapons = (from item in ((SRPGGame) Game.GetInstance()).Inventory where item.ItemType == _character.Class.WeaponTypes select item);
            GenerateItemMenu(weapons.ToList(), (newItem) => (s, a) =>
                {
                    var currWeapon = from item in _character.Inventory
                                     where item.ItemType == _character.Class.WeaponTypes
                                     select item;

                    if (currWeapon.Any())
                    {
                        _character.Inventory.Remove(currWeapon.First());
                    }

                    _character.Inventory.Add(newItem);

                    UpdateObjects();
                });
        }

        private void ChangeArmor(object sender, MouseEventArgs args)
        {
            var armors = (from item in ((SRPGGame)Game.GetInstance()).Inventory where item.ItemType == _character.Class.ArmorTypes select item);
            GenerateItemMenu(armors.ToList(), (item) => (s, a) => { });
        }

        private void GenerateItemMenu(List<Item> items, Func<Item, EventHandler<MouseEventArgs>> callback)
        {
            var oldKeys = (from key in Objects.Keys where key.Length > 15 && key.Substring(0, 15) == "selectable item" select key);

            foreach(var key in oldKeys.ToList())
            {
                Objects.Remove(key);
            }
            
            var i = 0;

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            foreach(var item in items)
            {
                Objects.Add("selectable item " + i, new TextObject()
                    {
                        Font = font,
                        Value = item.Name,
                        X = 450,
                        Y = 50 * (i + 1),
                        Color = Color.White
                    });
                Objects["selectable item " + i].MouseClick += callback(item);

                i++;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.PartyMenu
{
    class StatusMenu : SubmenuLayer
    {
        private Character _character;

        public StatusMenu(Torch.Scene scene) : base(scene)
        {
            var game = ((SRPGGame)Game.GetInstance());
            var font = game.Content.Load<SpriteFont>("menu");

            for (var i = 0; i < game.Party.Count; i++)
            {
                var character = game.Party[i];
                Objects.Add("party/" + character.Name, new TextObject
                    {
                        Font = font,
                        Value = character.Name,
                        X = 50,
                        Y = 125 + (50 * i),
                        Color = Color.White
                    });
                Objects["party/" + character.Name].MouseClick += (sender, args) => ((PartyMenuScene) Scene).SetCharacter(character);
            }
        }

        public void SetCharacter(Character character)
        {
            _character = character;
            UpdateObjects();
        }

        public override void Reset()
        {
            ClearStats();
            ClearItems();
        }

        private void UpdateObjects()
        {
            ClearStats();

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            // labels
            Objects.Add("stat/class label", new TextObject{ Font = font, X = 275, Y = 125, Value = "Class", Color = Color.White });
            Objects.Add("stat/health label", new TextObject { Font = font, X = 275, Y = 175, Value = "Health", Color = Color.White });
            Objects.Add("stat/mana label", new TextObject { Font = font, X = 275, Y = 225, Value = "Mana", Color = Color.White });

            Objects.Add("stat/def label", new TextObject { Font = font, X = 275, Y = 275, Value = "DEF", Color = Color.White });
            Objects.Add("stat/atk label", new TextObject { Font = font, X = 500, Y = 275, Value = "ATK", Color = Color.White });

            Objects.Add("stat/wis label", new TextObject { Font = font, X = 275, Y = 325, Value = "WIS", Color = Color.White });
            Objects.Add("stat/int label", new TextObject { Font = font, X = 500, Y = 325, Value = "INT", Color = Color.White });
            
            Objects.Add("stat/spd label", new TextObject { Font = font, X = 275, Y = 375, Value = "SPD", Color = Color.White });
            Objects.Add("stat/hit label", new TextObject { Font = font, X = 500, Y = 375, Value = "HIT", Color = Color.White });

            Objects.Add("stat/weapon label", new TextObject { Font = font, X = 275, Y = 475, Value = "Weapon", Color = Color.White });
            Objects.Add("stat/armor label", new TextObject { Font = font, X = 275, Y = 525, Value = "Armor", Color = Color.White });
            Objects.Add("stat/accessory label", new TextObject { Font = font, X = 275, Y = 575, Value = "Accessory", Color = Color.White });

            Objects.Add("stat/ability label", new TextObject { Font = font, X = 275, Y = 675, Value = "Abilities", Color = Color.White });

            // health
            // mana
            // dawish
            // portrait
            // abilities

            // values
            //Objects.Add("portrait", new ImageObject(_character.Portrait));
            Objects.Add("stat/class", new TextObject { Font = font, Y = 125, X = 500, Value = _character.Class.Name, Color = Color.Yellow });
            Objects.Add("stat/health", new TextObject { Font = font, Y = 175, X = 500, Value = _character.MaxHealth.ToString(), Color = Color.Yellow });
            Objects.Add("stat/mana", new TextObject{ Font = font, Y = 225, X = 500, Value = _character.MaxMana.ToString(), Color = Color.Yellow });

            Objects.Add("stat/def", new TextObject { Font = font, Y = 275, X = 387, Value = _character.ReadStat(Stat.Defense).ToString(), Color = Color.Yellow });
            Objects.Add("stat/atk", new TextObject { Font = font, Y = 275, X = 612, Value = _character.ReadStat(Stat.Attack).ToString(), Color = Color.Yellow });

            Objects.Add("stat/wis", new TextObject { Font = font, Y = 325, X = 387, Value = _character.ReadStat(Stat.Wisdom).ToString(), Color = Color.Yellow });
            Objects.Add("stat/int", new TextObject { Font = font, Y = 325, X = 612, Value = _character.ReadStat(Stat.Intelligence).ToString(), Color = Color.Yellow });

            Objects.Add("stat/spd", new TextObject { Font = font, Y = 375, X = 387, Value = _character.ReadStat(Stat.Speed).ToString(), Color = Color.Yellow });
            Objects.Add("stat/hit", new TextObject { Font = font, Y = 375, X = 612, Value = _character.ReadStat(Stat.Hit).ToString(), Color = Color.Yellow });

            var weaponName = _character.GetEquippedWeapon() != null ? _character.GetEquippedWeapon().Name : "--";
            var armorName = _character.GetEquippedArmor() != null ? _character.GetEquippedArmor().Name : "--";
            var accessoryName = _character.GetEquippedAccessory() != null ? _character.GetEquippedAccessory().Name : "--";

            Objects.Add("stat/weapon", new TextObject { Font = font, Y = 475, X = 500, Value = weaponName, Color = Color.Yellow });
            Objects.Add("stat/armor", new TextObject { Font = font, Y = 525, X = 500, Value = armorName, Color = Color.Yellow });
            Objects.Add("stat/accessory", new TextObject { Font = font, Y = 575, X = 500, Value = accessoryName, Color = Color.Yellow });

            // when you click on one of the equipped items, display a list of items that can be used in that slot...
            Objects["stat/weapon"].MouseClick += ChangeWeapon;
            Objects["stat/armor"].MouseClick += ChangeArmor;
            Objects["stat/accessory"].MouseClick += ChangeAccessory;

            var abilities = _character.GetAbilities();
            var x = 0;
            var y = 0;

            for(var i = 0; i < abilities.Count; i++)
            {
                Objects.Add("stat/ability-" + i, new TextObject()
                    {
                        Font = font, 
                        Y = 725 + (y * 50), 
                        X = 275 + (x * 225), 
                        Value = abilities[i].Name,
                        Color = _character.CanUseAbility(abilities[i]) ? Color.Yellow : Color.Gray
                    });

                x++;
                if(x == 3)
                {
                    x = 0;
                    y++;
                }
            }
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

        private void ChangeAccessory(object sender, MouseEventArgs args)
        {
            var accessories = (from item in ((SRPGGame)Game.GetInstance()).Inventory where item.ItemType == ItemType.Accessory select item);
            GenerateItemMenu(accessories.ToList());
        }

        private void GenerateItemMenu(IEnumerable<Item> items)
        {
            ClearItems();

            var i = 0;

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            foreach(var item in items.ToList())
            {
                var tempItem = item;
                
                Objects.Add("selectable item/" + i, new TextObject()
                    {
                        Font = font,
                        Value = item.Name,
                        X = 725,
                        Y = 125 + 50 * (i),
                        Color = Color.White
                    });
                
                Objects["selectable item/" + i].MouseClick += (sender, args) =>
                    {
                        ((SRPGGame) Game.GetInstance()).EquipCharacter(_character, tempItem);
                        UpdateObjects();
                        ClearItems();
                    };

                i++;
            }
        }

        private void ClearItems()
        {
            var oldKeys =
                (from key in Objects.Keys where key.Length > 15 && key.Substring(0, 15) == "selectable item" select key);

            foreach (var key in oldKeys.ToList())
            {
                Objects.Remove(key);
            }
        }

        private void ClearStats()
        {
            var oldKeys =
                (from key in Objects.Keys where key.Length > 4 && key.Substring(0, 4) == "stat" select key);

            foreach (var key in oldKeys.ToList())
            {
                Objects.Remove(key);
            }
        }
    }
}

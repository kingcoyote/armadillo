﻿using System.Collections.Generic;
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
        private Combatant _character;

        public StatusMenu(Torch.Scene scene) : base(scene)
        {

            var font = FontManager.Get("Menu");

            for (var i = 0; i < ((SRPGGame)Game).Party.Count; i++)
            {
                var character = ((SRPGGame)Game).Party[i];
                Objects.Add("party/" + character.Name, new TextObject
                    {
                        Font = font,
                        Value = character.Name,
                        X = 50,
                        Y = 125 + (int)(font.LineSpacing * 1.5 * i),
                        Color = Color.White
                    });
                //Objects["party/" + character.Name].MouseClick += (sender, args) => ((PartyMenuScene) Scene).SetCharacter(character);
            }
        }

        public void SetCharacter(Combatant character)
        {
            _character = character;
            UpdateObjects();
        }

        public override void Reset()
        {
            ClearByName("stat");
            ClearByName("selectable item");
        }

        private void UpdateObjects()
        {
            ClearByName("stat");
            ClearByName("selectable item");

            var font = FontManager.Get("Menu");

            // labels
            Objects.Add("stat/class label", new TextObject{ Font = font, X = 275, Y = 125, Value = "Class", Color = Color.White });
            Objects.Add("stat/health label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 1.5), Value = "Health", Color = Color.White });
            Objects.Add("stat/mana label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 3), Value = "Mana", Color = Color.White });

            Objects.Add("stat/def label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 4.5), Value = "DEF", Color = Color.White });
            Objects.Add("stat/atk label", new TextObject { Font = font, X = 500, Y = (int)(125 + font.LineSpacing * 4.5), Value = "ATK", Color = Color.White });

            Objects.Add("stat/wis label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 6), Value = "WIS", Color = Color.White });
            Objects.Add("stat/int label", new TextObject { Font = font, X = 500, Y = (int)(125 + font.LineSpacing * 6), Value = "INT", Color = Color.White });

            Objects.Add("stat/spd label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 7.5), Value = "SPD", Color = Color.White });
            Objects.Add("stat/hit label", new TextObject { Font = font, X = 500, Y = (int)(125 + font.LineSpacing * 7.5), Value = "HIT", Color = Color.White });

            Objects.Add("stat/weapon label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 10.5), Value = "Weapon", Color = Color.White });
            Objects.Add("stat/armor label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 12), Value = "Armor", Color = Color.White });
            Objects.Add("stat/accessory label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 13.5), Value = "Accessory", Color = Color.White });

            Objects.Add("stat/ability label", new TextObject { Font = font, X = 275, Y = (int)(125 + font.LineSpacing * 16.5), Value = "Abilities", Color = Color.White });

            // health
            // mana
            // dawish
            // portrait
            // abilities

            // values
            //Objects.Add("portrait", new ImageObject(_character.Portrait));
            Objects.Add("stat/class", new TextObject { Font = font, Y = 125, X = 500, Value = _character.Class, Color = Color.Yellow });
            Objects.Add("stat/health", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 1.5), X = 500, Value = _character.MaxHealth.ToString(), Color = Color.Yellow });
            Objects.Add("stat/mana", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 3), X = 500, Value = _character.MaxMana.ToString(), Color = Color.Yellow });

            Objects.Add("stat/def", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 4.5), X = 357, Value = _character.ReadStat(Stat.Defense).ToString(), Color = Color.Yellow });
            Objects.Add("stat/atk", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 4.5), X = 582, Value = _character.ReadStat(Stat.Attack).ToString(), Color = Color.Yellow });

            Objects.Add("stat/wis", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 6), X = 357, Value = _character.ReadStat(Stat.Wisdom).ToString(), Color = Color.Yellow });
            Objects.Add("stat/int", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 6), X = 582, Value = _character.ReadStat(Stat.Intelligence).ToString(), Color = Color.Yellow });

            Objects.Add("stat/spd", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 7.5), X = 357, Value = _character.ReadStat(Stat.Speed).ToString(), Color = Color.Yellow });
            Objects.Add("stat/hit", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 7.5), X = 582, Value = _character.ReadStat(Stat.Hit).ToString(), Color = Color.Yellow });

            var weaponName = _character.GetEquippedWeapon() != null ? _character.GetEquippedWeapon().Name : "--";
            var armorName = _character.GetEquippedArmor() != null ? _character.GetEquippedArmor().Name : "--";
            var accessoryName = _character.GetEquippedAccessory() != null ? _character.GetEquippedAccessory().Name : "--";

            Objects.Add("stat/weapon", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 10.5), X = 500, Value = weaponName, Color = Color.Yellow });
            Objects.Add("stat/armor", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 12), X = 500, Value = armorName, Color = Color.Yellow });
            Objects.Add("stat/accessory", new TextObject { Font = font, Y = (int)(125 + font.LineSpacing * 13.5), X = 500, Value = accessoryName, Color = Color.Yellow });

            // when you click on one of the equipped items, display a list of items that can be used in that slot...
            //Objects["stat/weapon"].MouseClick += ChangeWeapon;
            //Objects["stat/armor"].MouseClick += ChangeArmor;
            //Objects["stat/accessory"].MouseClick += ChangeAccessory;

            var abilities = _character.GetAbilities();
            var x = 0;
            var y = 0;

            for(var i = 0; i < abilities.Count; i++)
            {
                Objects.Add("stat/ability-" + i, new TextObject()
                    {
                        Font = font,
                        Y = (int)(125 + font.LineSpacing * (18 + y * 1.5)), 
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

        private void ChangeWeapon()
        {
            var weapons = (from item in ((SRPGGame) Game).Inventory where item.ItemType == _character.WeaponTypes select item);
            GenerateItemMenu(weapons.ToList());
        }

        private void ChangeArmor()
        {
            var armors = (from item in ((SRPGGame)Game).Inventory where item.ItemType == _character.ArmorTypes select item);
            GenerateItemMenu(armors.ToList());
        }

        private void ChangeAccessory()
        {
            var accessories = (from item in ((SRPGGame)Game).Inventory where item.ItemType == ItemType.Accessory select item);
            GenerateItemMenu(accessories.ToList());
        }

        private void GenerateItemMenu(IEnumerable<Item> items)
        {
            ClearByName("selectable item");

            var i = 0;

            var font = FontManager.Get("Menu");

            foreach(var item in items.ToList())
            {
                var tempItem = item;
                
                Objects.Add("selectable item/" + i, new TextObject()
                    {
                        Font = font,
                        Value = item.Name,
                        X = 725,
                        Y = 125 + (int)(font.LineSpacing * (i) * 1.5),
                        Color = Color.White
                    });
                
                //Objects["selectable item/" + i].MouseClick += (sender, args) =>
                    {
                        ((SRPGGame) Game).EquipCharacter(_character, tempItem);
                        UpdateObjects();
                        ClearByName("selectable item");
                        ClearItemPreview();
                    };

                //Objects["selectable item/" + i].MouseOver += (sender, args) => SetItemPreview(tempItem);
                //Objects["selectable item/" + i].MouseOut += (sender, args) => ClearItemPreview();

                i++;
            }
        }

        public void SetItemPreview(Item item)
        {
            var oldStats = _character.ReadAllStats();
            var oldItem = _character.EquipItem(item);
            var newStats = _character.ReadAllStats();
            if(oldItem != null)
            {
                _character.EquipItem(oldItem);
            } else
            {
                _character.Inventory.Remove(item);
            }

            var font = FontManager.Get("Menu");

            foreach(var stat in oldStats.Keys)
            {
                newStats[stat] -= oldStats[stat];
            }

            if(newStats[Stat.Defense] != 0 ) Objects.Add("preview/defense", new TextObject()
            {
                Y = (int)(125 + font.LineSpacing * 4.5),
                X = 387 + 30,
                Color = newStats[Stat.Defense] < 0 ? Color.Red : Color.LightGreen,
                Value = newStats[Stat.Defense].ToString(),
                Font = font
            });

            if (newStats[Stat.Attack] != 0) Objects.Add("preview/attack", new TextObject()
            {
                Y = (int)(125 + font.LineSpacing * 4.5),
                X = 612 + 30,
                Color = newStats[Stat.Attack] < 0 ? Color.Red : Color.LightGreen,
                Value = newStats[Stat.Attack].ToString(),
                Font = font
            });

            if (newStats[Stat.Wisdom] != 0) Objects.Add("preview/wisdom", new TextObject()
            {
                Y = (int)(125 + font.LineSpacing * 6),
                X = 387 + 30,
                Color = newStats[Stat.Wisdom] < 0 ? Color.Red : Color.LightGreen,
                Value = newStats[Stat.Wisdom].ToString(),
                Font = font
            });

            if (newStats[Stat.Intelligence] != 0) Objects.Add("preview/intelligence", new TextObject()
            {
                Y = (int)(125 + font.LineSpacing * 6),
                X = 612 + 30,
                Color = newStats[Stat.Intelligence] < 0 ? Color.Red : Color.LightGreen,
                Value = newStats[Stat.Intelligence].ToString(),
                Font = font
            });

            if (newStats[Stat.Speed] != 0) Objects.Add("preview/speed", new TextObject()
            {
                Y = (int)(125 + font.LineSpacing * 7.5),
                X = 387 + 30,
                Color = newStats[Stat.Speed] < 0 ? Color.Red : Color.LightGreen,
                Value = newStats[Stat.Speed].ToString(),
                Font = font
            });

            if (newStats[Stat.Hit] != 0) Objects.Add("preview/hit", new TextObject()
            {
                Y = (int)(125 + font.LineSpacing * 7.5),
                X = 612 + 30,
                Color = newStats[Stat.Hit] < 0 ? Color.Red : Color.LightGreen,
                Value = newStats[Stat.Hit].ToString(),
                Font = font
            });
        }

        public void ClearItemPreview()
        {
            ClearByName("preview");
        }
    }
}

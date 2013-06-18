using System;
using System.Collections.Generic;
using SRPG.Data;
using SRPG.Scene.Battle;
using SRPG.Scene.Intro;
using SRPG.Scene.PartyMenu;
using SRPG.Scene.Options;
using SRPG.Scene.Overworld;
using SRPG.Scene.Shop;

namespace SRPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SRPGGame : Torch.Game
    {
        public List<Character> Party;
        public List<Item> Inventory;
        public int Money;

        public const int TileSize = 64;

        public SRPGGame()
        {
            IsFullScreen = false;

            Scenes.Add("intro", new IntroScene(this));
            Scenes.Add("party menu", new PartyMenuScene(this));
            Scenes.Add("overworld", new OverworldScene(this));
            Scenes.Add("battle", new BattleScene(this));
            Scenes.Add("shop", new ShopScene(this));
            Scenes.Add("options", new OptionsScene(this));

            CurrentScene = "intro";
        }

        public void StartGame()
        {
            ChangeScenes("overworld");
            
            ((OverworldScene)Scenes["overworld"]).SetZone(Zone.Factory("kakariko/village"), "arch");

            BeginNewGame();
        }

        public void LaunchShop(List<Item> inventory)
        {
            ((ShopScene)Scenes["shop"]).SetInventory(inventory);
            ChangeScenes("shop");
        }

        public void EquipCharacter(Character character, Item newItem)
        {
            if(Inventory.Contains(newItem))
            {
                var oldItem = character.EquipItem(newItem);
                Inventory.Remove(newItem);
                if(oldItem != null) Inventory.Add(oldItem);
            }
            else
            {
                throw new Exception("this item cannot be equipped because it is not in the inventory");
            }
        }

        public void SellItem(Item item)
        {
            if( ! Inventory.Contains(item))
            {
                throw new Exception("cannot sell item that isn't in inventory");
            }

            Inventory.Remove(item);

            Money += item.Cost;
        }

        public void BuyItem(Item item)
        {
            if(item.Cost > Money)
            {
                throw new Exception("cannot buy item - too expensive");
            }

            Inventory.Add(item);

            Money -= item.Cost;
        }

        private void BeginNewGame()
        {
            Party = new List<Character>();
            Inventory = new List<Item>();

            var character = CharacterClass.GenerateCharacter("fighter");
            character.Name = "Jaha";
            character.MaxHealth = 20;
            character.Stats[Stat.Defense] = 22;
            character.Stats[Stat.Attack] = 18;
            character.Stats[Stat.Wisdom] = 6;
            character.Stats[Stat.Intelligence] = 8;
            character.Stats[Stat.Speed] = 12;
            character.Stats[Stat.Hit] = 15;
            character.AbilityExperienceLevels.Add(Ability.Factory("lunge"), 200);
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("cleric");
            character.Name = "Aeris";
            character.MaxHealth = 12;
            character.Stats[Stat.Defense] = 8;
            character.Stats[Stat.Attack] = 6;
            character.Stats[Stat.Wisdom] = 22;
            character.Stats[Stat.Intelligence] = 15;
            character.Stats[Stat.Speed] = 12;
            character.Stats[Stat.Hit] = 18;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("ranger");
            character.Name = "Meera";
            character.MaxHealth = 14;
            character.Stats[Stat.Defense] = 12;
            character.Stats[Stat.Attack] = 15;
            character.Stats[Stat.Wisdom] = 8;
            character.Stats[Stat.Intelligence] = 6;
            character.Stats[Stat.Speed] = 18;
            character.Stats[Stat.Hit] = 22;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("wizard");
            character.Name = "Raistlin";
            character.MaxHealth = 12;
            character.Stats[Stat.Defense] = 6;
            character.Stats[Stat.Attack] = 12;
            character.Stats[Stat.Wisdom] = 22;
            character.Stats[Stat.Intelligence] = 18;
            character.Stats[Stat.Speed] = 8;
            character.Stats[Stat.Hit] = 15;
            Party.Add(character);

            character = CharacterClass.GenerateCharacter("monk");
            character.Name = "Gratho";
            character.MaxHealth = 18;
            character.Stats[Stat.Defense] = 8;
            character.Stats[Stat.Attack] = 15;
            character.Stats[Stat.Wisdom] = 12;
            character.Stats[Stat.Intelligence] = 6;
            character.Stats[Stat.Speed] = 22;
            character.Stats[Stat.Hit] = 18;
            character.AbilityExperienceLevels.Add(Ability.Factory("lunge"), 200);
            character.AbilityExperienceLevels.Add(Ability.Factory("cobra punch"), 200);
            character.AbilityExperienceLevels.Add(Ability.Factory("flying knee"), 200);
            character.AbilityExperienceLevels.Add(Ability.Factory("whip kick"), 200);
            character.AbilityExperienceLevels.Add(Ability.Factory("sprint"), 200);
            character.AbilityExperienceLevels.Add(Ability.Factory("untouchable"), 200);
            character.AbilityExperienceLevels.Add(Ability.Factory("blur"), 200);
            Party.Add(character);

            Inventory.Add(Item.Factory("sword/shortsword"));
            Inventory.Add(Item.Factory("sword/longsword"));
            Inventory.Add(Item.Factory("sword/greatsword"));

            Inventory.Add(Item.Factory("staff/firewand"));
            Inventory.Add(Item.Factory("staff/sparkstaff"));
            Inventory.Add(Item.Factory("staff/groundstaff"));

            Inventory.Add(Item.Factory("book/scroll"));
            Inventory.Add(Item.Factory("book/holybook"));
            Inventory.Add(Item.Factory("book/anthology"));

            Inventory.Add(Item.Factory("gun/pistol"));
            Inventory.Add(Item.Factory("gun/revolver"));
            Inventory.Add(Item.Factory("gun/bigiron"));

            Inventory.Add(Item.Factory("unarmed/brassknuckles"));
            Inventory.Add(Item.Factory("unarmed/tigerclaws"));
            Inventory.Add(Item.Factory("unarmed/oldshoes"));

            Inventory.Add(Item.Factory("cloth/vest"));
            Inventory.Add(Item.Factory("cloth/robe"));
            Inventory.Add(Item.Factory("cloth/kimono"));

            Inventory.Add(Item.Factory("cloth/vest"));
            Inventory.Add(Item.Factory("cloth/robe"));
            Inventory.Add(Item.Factory("cloth/kimono"));

            Inventory.Add(Item.Factory("leather/jerkin"));
            Inventory.Add(Item.Factory("leather/studdedleather"));
            Inventory.Add(Item.Factory("leather/brigandine"));

            Inventory.Add(Item.Factory("mail/hauberk"));
            Inventory.Add(Item.Factory("mail/scalemail"));
            Inventory.Add(Item.Factory("mail/dragonskin"));

            Inventory.Add(Item.Factory("plate/breastplate"));
            Inventory.Add(Item.Factory("plate/fullplate"));
            Inventory.Add(Item.Factory("plate/gothicplate"));
        }
    }
}

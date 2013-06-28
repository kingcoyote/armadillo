using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SRPG.Data;
using SRPG.Scene.Battle;
using SRPG.Scene.Intro;
using SRPG.Scene.MainMenu;
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
        public List<Combatant> Party;
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
            Scenes.Add("main menu", new MainMenu(this));

            CurrentScene = "intro";
        }

        public void StartGame()
        {
            ChangeScenes("overworld");
            
            ((OverworldScene)Scenes["overworld"]).SetZone(Zone.Factory("coliseum/cell"), "bed");

            BeginNewGame();
        }

        public void LaunchShop(string filename, string merchantname)
        {
            var inventory = GenerateShopInventory(filename, merchantname);
            ((ShopScene)Scenes["shop"]).SetInventory(inventory);
            ChangeScenes("shop");
        }

        public void StartBattle(string battleName)
        {
            ChangeScenes("battle");
            ((BattleScene) Scenes["battle"]).SetBattle(battleName);
        }

        public void EquipCharacter(Combatant character, Item newItem)
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

        private List<Item> GenerateShopInventory(string filename, string merchantname)
        {
            string settingString = String.Join("\r\n", File.ReadAllLines("Content/Dialog/" + filename + ".js"));

            var nodeList = Newtonsoft.Json.Linq.JObject.Parse(settingString);

            return nodeList[merchantname]["inventory"].Select(node => Item.Factory(node.ToString())).ToList();
        }

        private void BeginNewGame()
        {
            Party = new List<Combatant>();
            Inventory = new List<Item>();

            Combatant character;

            character = new Combatant();
            character.Class = new CharacterClass("fighter", ItemType.Sword, ItemType.Plate);
            character.Name = "Jaha";
            character.MaxHealth = 20;
            character.Stats[Stat.Defense] = 22;
            character.Stats[Stat.Attack] = 18;
            character.Stats[Stat.Wisdom] = 6;
            character.Stats[Stat.Intelligence] = 8;
            character.Stats[Stat.Speed] = 12;
            character.Stats[Stat.Hit] = 15;
            character.AbilityExperienceLevels.Add(Ability.Factory("lunge"), 200);
            character.Sprite = CharacterClass.GenerateCharacter("fighter");
            Party.Add(character);

            character = new Combatant();
            character.Class = new CharacterClass("cleric", ItemType.Book, ItemType.Cloth);
            character.Name = "Aeris";
            character.MaxHealth = 12;
            character.Stats[Stat.Defense] = 8;
            character.Stats[Stat.Attack] = 6;
            character.Stats[Stat.Wisdom] = 22;
            character.Stats[Stat.Intelligence] = 15;
            character.Stats[Stat.Speed] = 12;
            character.Stats[Stat.Hit] = 18;
            character.Sprite = CharacterClass.GenerateCharacter("cleric");
            Party.Add(character);

            character = new Combatant();
            character.Class = new CharacterClass("ranger", ItemType.Gun, ItemType.Mail);
            character.Name = "Meera";
            character.MaxHealth = 14;
            character.Stats[Stat.Defense] = 12;
            character.Stats[Stat.Attack] = 15;
            character.Stats[Stat.Wisdom] = 8;
            character.Stats[Stat.Intelligence] = 6;
            character.Stats[Stat.Speed] = 18;
            character.Stats[Stat.Hit] = 22;
            character.Sprite = CharacterClass.GenerateCharacter("ranger");
            Party.Add(character);

            character = new Combatant();
            character.Class = new CharacterClass("wizard", ItemType.Staff, ItemType.Cloth);
            character.Name = "Raistlin";
            character.MaxHealth = 12;
            character.Stats[Stat.Defense] = 6;
            character.Stats[Stat.Attack] = 12;
            character.Stats[Stat.Wisdom] = 22;
            character.Stats[Stat.Intelligence] = 18;
            character.Stats[Stat.Speed] = 8;
            character.Stats[Stat.Hit] = 15;
            character.Sprite = CharacterClass.GenerateCharacter("wizard");
            Party.Add(character);

            character = new Combatant();
            character.Class = new CharacterClass("monk", ItemType.Unarmed, ItemType.Leather);
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
            character.Inventory.Add(Item.Factory("unarmed/tigerclaws"));
            character.Sprite = CharacterClass.GenerateCharacter("monk");
            Party.Add(character);


            Money = 500;
        }
    }
}

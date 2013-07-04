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

            var character = Combatant.FromTemplate("party/jaha");
            character.Name = "Jaha";
            Party.Add(character);

            character = Combatant.FromTemplate("party/meera");
            character.Name = "Meera";
            Party.Add(character);

            character = Combatant.FromTemplate("party/aeris");
            character.Name = "Aeris";
            Party.Add(character);

            character = Combatant.FromTemplate("party/raistlin");
            character.Name = "Raistlin";
            Party.Add(character);
        }
    }
}

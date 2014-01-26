using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
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
            CurrentScene = "intro";
        }

        protected override void Initialize()
        {
            FontManager.Initialize(ScreenHeight >= 1024 ? FontSize.Normal : FontSize.Small);
            
            FontManager.Add("Menu", FontSize.Normal, Content.Load<SpriteFont>("Fonts/MenuNormal"));
            FontManager.Add("Menu", FontSize.Small, Content.Load<SpriteFont>("Fonts/MenuSmall"));

            FontManager.Add("Dialog", FontSize.Normal, Content.Load<SpriteFont>("Fonts/DialogNormal"));
            FontManager.Add("Dialog", FontSize.Small, Content.Load<SpriteFont>("Fonts/DialogSmall"));

            IsFullScreen = true;
            ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

#if DEBUG
            IsFullScreen = false;
            ScreenWidth = 1024;
            ScreenHeight = 768;
#endif

            base.Initialize();      

            PushScene(new MainMenu(this));
            PushScene(new IntroScene(this));
        }

        public void StartGame()
        {
            var overworldScene = new OverworldScene(this);
            overworldScene.SetZone(Zone.Factory(this, null, "coliseum/cell"), "bed");
            PushScene(overworldScene);

            BeginNewGame();
        }

        public void LaunchShop(string filename, string merchantname)
        {
            var inventory = GenerateShopInventory(filename, merchantname);
            var shopScene = new ShopScene(this);
            shopScene.SetInventory(inventory);
            PushScene(shopScene);
        }

        public void StartBattle(string battleName)
        {
            var battleScene = new BattleScene(this);
            battleScene.SetBattle(battleName);
            PushScene(battleScene);
        }

        public void EquipCharacter(Combatant character, Item newItem)
        {
            if (Inventory.Contains(newItem))
            {
                var oldItem = character.EquipItem(newItem);
                Inventory.Remove(newItem);
                if (oldItem != null) Inventory.Add(oldItem);
            }
            else
            {
                throw new Exception("this item cannot be equipped because it is not in the inventory");
            }
        }

        public void SellItem(Item item)
        {
            if (!Inventory.Contains(item))
            {
                throw new Exception("cannot sell item that isn't in inventory");
            }

            Inventory.Remove(item);

            Money += item.Cost;
        }

        public void BuyItem(Item item)
        {
            if (item.Cost > Money)
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

            return nodeList[merchantname]["inventory"].Select(node => Item.Factory(this, node.ToString())).ToList();
        }

        private void BeginNewGame()
        {
            Party = new List<Combatant>();
            Inventory = new List<Item>();

#if DEBUG
            Inventory.Add(Item.Factory(this, "sword/longsword"));
#endif

            var character = Combatant.FromTemplate(this, "party/jaha");
            character.Name = "Jaha";
            Party.Add(character);

            character = Combatant.FromTemplate(this, "party/meera");
            character.Name = "Meera";
            Party.Add(character);

            character = Combatant.FromTemplate(this, "party/hahn");
            character.Name = "Hahn";
            Party.Add(character);

            character = Combatant.FromTemplate(this, "party/sheena");
            character.Name = "Sheena";
            Party.Add(character);
        }

        public override void ChangeResolution(int width, int height)
        {
            base.ChangeResolution(width, height);

            FontManager.Initialize(ScreenHeight >= 1024 ? FontSize.Normal : FontSize.Small);
        }

        
        public void ReturnToMainMenu()
        {
            while (!(GameStates.ActiveState is MainMenu))
            {
                PopScene();
            }
        }
    }
}

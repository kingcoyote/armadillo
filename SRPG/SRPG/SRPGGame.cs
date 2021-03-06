using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using SRPG.Scene.Battle;
using SRPG.Scene.Intro;
using SRPG.Scene.LoadGame;
using SRPG.Scene.MainMenu;
using SRPG.Scene.PartyMenu;
using SRPG.Scene.Options;
using SRPG.Scene.Overworld;
using SRPG.Scene.SaveGame;
using SRPG.Scene.Shop;
using Environment = System.Environment;

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
        public long GameTime;

        public readonly Dictionary<string, byte[]> ZoneData = new Dictionary<string, byte[]>(); 

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

            ResetGame();
            BeginNewGame();
        }

        public void LaunchShop(string filename, string merchantname)
        {
            PushScene(new ShopScene(
                this, 
                GenerateShopInventory(filename, merchantname)
            ));
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
            Inventory.Add(Item.Factory(this, "consumable/potion"));
            Inventory.Add(Item.Factory(this, "consumable/potion"));

            Money = 250;
#endif

            Party.Add(Combatant.FromTemplate(this, "party/jaha"));
            Party.Add(Combatant.FromTemplate(this, "party/meera"));
            Party.Add(Combatant.FromTemplate(this, "party/hahn"));
            Party.Add(Combatant.FromTemplate(this, "party/sheena"));
        }
        
        public void SaveGame(int filenumber, string zone, string door)
        {
            new SaveGame
                {
                    Inventory = Inventory, 
                    Party = Party, 
                    Money = Money, 
                    Zone = zone, 
                    Door = door, 
                    Name=DateTime.Now.ToString(),
                    GameTime = GameTime / 1000 * 1000
                }.Save(filenumber);
        }

        public void LoadGame(int filenumber)
        {
            ResetGame();

            var save = Data.SaveGame.Load(this, filenumber);
            Inventory = save.Inventory;
            Party = save.Party;
            Money = save.Money;
            GameTime = save.GameTime;

            var overworldScene = new OverworldScene(this);
            overworldScene.SetZone(Zone.Factory(this, null, save.Zone), save.Door);
            PushScene(overworldScene);
        }

        public void ShowLoadScreen()
        {
            PushScene(new LoadGameScene(this));
        }

        public void ShowSaveScreen(string zone, string door)
        {
            PushScene(new SaveGameScene(this, zone, door));
        }

        public void NewSaveGame(string zone, string door)
        {
            var files = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Armadillo\\Save").GetFiles("save*.asg", SearchOption.TopDirectoryOnly);
            var n = 0;
            foreach (var f in files)
            {
                int number;
                if (int.TryParse(f.Name.Replace("save", "").Replace(".asg", ""), out number))
                {
                    if (number >= n) n = number + 1;
                }
            }
            SaveGame(n, zone, door);
        }

        public void ContinueLastGame()
        {
            var files = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Armadillo\\Save").GetFiles("save*.asg", SearchOption.TopDirectoryOnly);
            var n = 0;
            var modTime = DateTime.FromBinary(0);
            foreach (var f in files)
            {
                int number;
                if (!int.TryParse(f.Name.Replace("save", "").Replace(".asg", ""), out number))
                {
                    continue;
                }

                if (f.LastWriteTime > modTime)
                {
                    modTime = f.LastWriteTime;
                    n = number;
                }
            }
            LoadGame(n);
        }

        public void ReturnToMainMenu()
        {
            while (!(GameStates.ActiveState is MainMenu))
            {
                PopScene();
            }
        }

        private void ResetGame()
        {
            GameTime = 0;
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (GameStates.ActiveState is OverworldScene || GameStates.ActiveState is ShopScene || GameStates.ActiveState is BattleScene)
            {
                GameTime += (long)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class ShopScene : Torch.Scene
    {
        private List<Item> _inventory = new List<Item>();

        private ShopInventory _shopInventory;
        private PlayerInventory _playerInventory;

        public ShopScene(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            Components.Clear();

            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Components.Add(keyboard);

            Components.Add(new Background(this));

            // store inventory layer
            _playerInventory = new PlayerInventory(this, ((SRPGGame)Game).Inventory) { X = Game.GetInstance().GraphicsDevice.Viewport.Width - 400, Y = 50 };
            Components.Add(_playerInventory);

            // player inventory layer
            _shopInventory = new ShopInventory(this, _inventory) { X = 50, Y = 50 };
            Components.Add(_shopInventory);

            Components.Add(new Options(this));
            Components.Add(new PlayerStats(this));

            // buy selected
            // sell selected

            // try on
            // buy and equip
        }

        public void RefreshShop()
        {
            
        }

        public override void Start()
        {
            Game.IsMouseVisible = true;
        }

        public override void Stop()
        {
            Game.IsMouseVisible = false;
        }

        public void SetInventory(List<Item> inventory)
        {
            _inventory = inventory;
        }

        public void SellSelectedItems()
        {
            var items = _playerInventory.GetSelectedInventory();

            foreach (var item in items)
            {
                ((SRPGGame) Game).SellItem(item);
            }

            Initialize();
        }

        public void BuySelectedItems()
        {
            var items = _shopInventory.GetSelectedInventory();
            var cost = (from item in items select item.Cost).Sum();

            if (cost > ((SRPGGame)Game.GetInstance()).Money) return;

            foreach(var item in items)
            {
                ((SRPGGame) Game).BuyItem(item);
            }

            Initialize();
        }
    }
}

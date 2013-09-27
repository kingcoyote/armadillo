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

        protected override void OnEntered()
        {
            Components.Clear();

            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());
            Components.Add(keyboard);

            Components.Add(new Background(this, null));

            // store inventory layer
            _playerInventory = new PlayerInventory(this, null, ((SRPGGame)Game).Inventory) { X = Game.GraphicsDevice.Viewport.Width - 400, Y = 50 };
            Components.Add(_playerInventory);

            // player inventory layer
            _shopInventory = new ShopInventory(this, null, _inventory) { X = 50, Y = 50 };
            Components.Add(_shopInventory);

            Components.Add(new Options(this, null));
            Components.Add(new PlayerStats(this, null));

            // buy selected
            // sell selected

            // try on
            // buy and equip
        }

        public void RefreshShop()
        {
            
        }

        protected override void OnResume()
        {
            Game.IsMouseVisible = true;
        }

        protected override void OnPause()
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
        }

        public void BuySelectedItems()
        {
            var items = _shopInventory.GetSelectedInventory();
            var cost = (from item in items select item.Cost).Sum();

            if (cost > ((SRPGGame)Game).Money) return;

            foreach(var item in items)
            {
                ((SRPGGame) Game).BuyItem(item);
            }
        }
    }
}

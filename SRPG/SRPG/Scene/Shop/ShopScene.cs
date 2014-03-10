using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Visuals.Flat;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class ShopScene : Torch.Scene
    {
        private List<Item> _inventory = new List<Item>();

        public ShopScene(Game game) : base(game)
        {
            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());
            Components.Add(keyboard);

            Game.IsMouseVisible = true;

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");

            // GUI elements:
            // shop inventory
            // player inventory
            // exit button
            // money
            // party

            // buy selected
            // sell selected

            // try on
            // buy and equip
        }

        protected override void OnEntered()
        {
            Game.IsMouseVisible = true;

            base.OnEntered();
        }

        protected override void OnResume()
        {
            Game.IsMouseVisible = true;

            base.OnResume();
        }

        public void RefreshShop()
        {
            
        }

        public void SetInventory(List<Item> inventory)
        {
            _inventory = inventory;
        }

        public void SellSelectedItems()
        {
            var items = new List<Item>();

            foreach (var item in items)
            {
                ((SRPGGame) Game).SellItem(item);
            }
        }

        public void BuySelectedItems()
        {
            var items = new List<Item>();
            var cost = (from item in items select item.Cost).Sum();

            if (cost > ((SRPGGame)Game).Money) return;

            foreach(var item in items)
            {
                ((SRPGGame) Game).BuyItem(item);
            }
        }
    }
}

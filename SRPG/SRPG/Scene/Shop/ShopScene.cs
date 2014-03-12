﻿using System.Collections.Generic;
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
        private List<Item> _playerInventory; 
        private List<Item> _shopInventory;
        
        private InventoryDialog _shopInventoryDialog;
        private InventoryDialog _playerInventoryDialog;
        private InfoDialog _infoDialog;
        private PartyDialog _partyDialog;
        private ItemPreviewDialog _itemPreviewDialog;

        private ButtonControl _exitButton;
        private ButtonControl _buyButton;
        private ButtonControl _sellButton;

        public ShopScene(Game game, List<Item> playerInventory, List<Item> shopInventory) : base(game)
        {
            _playerInventory = playerInventory;
            _shopInventory = shopInventory;

            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());
            Components.Add(keyboard);

            Game.IsMouseVisible = true;

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");

            // GUI elements:
            _shopInventoryDialog = new InventoryDialog(_shopInventory);
            _shopInventoryDialog.Bounds = new UniRectangle(
                new UniScalar(0), new UniScalar(0),
                new UniScalar(225), new UniScalar(0.6F, -45) 
            );
            _shopInventoryDialog.SelectionChanged += ShopSelectionChanged;
            _shopInventoryDialog.HoverChanged += UpdateItemFocus;
            Gui.Screen.Desktop.Children.Add(_shopInventoryDialog);

            _playerInventoryDialog = new InventoryDialog(_playerInventory);
            _playerInventoryDialog.Bounds = new UniRectangle(
                new UniScalar(235), new UniScalar(0),
                new UniScalar(225), new UniScalar(0.6F, -45) 
            );
            _playerInventoryDialog.SelectionChanged += PlayerSelectionChanged;
            _playerInventoryDialog.HoverChanged += UpdateItemFocus;
            Gui.Screen.Desktop.Children.Add(_playerInventoryDialog);

            _itemPreviewDialog = new ItemPreviewDialog();
            _itemPreviewDialog.Bounds = new UniRectangle(
                new UniScalar(470), new UniScalar(0),
                new UniScalar(1.0f, -630), new UniScalar(0.6F, 0)
            );
            Gui.Screen.Desktop.Children.Add(_itemPreviewDialog);

            // money / information
            _infoDialog = new InfoDialog();
            _infoDialog.Bounds = new UniRectangle(
                new UniScalar(1.0F, -150), new UniScalar(0.0F, 45),
                new UniScalar(150), new UniScalar(0.6F, -45)
            );
            Gui.Screen.Desktop.Children.Add(_infoDialog);
            
            // party
            _partyDialog = new PartyDialog();
            _partyDialog.Bounds = new UniRectangle(
                new UniScalar(0), new UniScalar(0.6F, 10),
                new UniScalar(1.0F, 0), new UniScalar(0.4F, -10)
            );
            Gui.Screen.Desktop.Children.Add(_partyDialog);

            // exit button
            _exitButton = new ButtonControl();
            _exitButton.Text = "Exit";
            _exitButton.Bounds = new UniRectangle(
                new UniScalar(1.0F, -150), new UniScalar(0),
                new UniScalar(150), new UniScalar(35)
            );
            _exitButton.Pressed += (s, a) => Game.PopScene();
            Gui.Screen.Desktop.Children.Add(_exitButton);

            // buy selected
            _buyButton = new ButtonControl();
            _buyButton.Text = "Buy";
            _buyButton.Bounds = new UniRectangle(
                new UniScalar(0), new UniScalar(0.6F, -35),
                new UniScalar(225), new UniScalar(35) 
            );
            Gui.Screen.Desktop.Children.Add(_buyButton);

            // sell selected
            _sellButton = new ButtonControl();
            _sellButton.Text = "Sell";
            _sellButton.Bounds = new UniRectangle(
                new UniScalar(235), new UniScalar(0.6F, -35),
                new UniScalar(225), new UniScalar(35) 
            );
            Gui.Screen.Desktop.Children.Add(_sellButton);
        }

        private void PlayerSelectionChanged(List<Item> items)
        {
            
        }

        private void ShopSelectionChanged(List<Item> items)
        {
            
        }

        private void UpdateItemFocus(Item item)
        {
            
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

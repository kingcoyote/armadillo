using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.Shop
{
    public partial class InventoryDialog : WindowControl
    {
        public List<Item> SelectedItems;
        
        private List<Item> _inventory;

        public delegate void InventoryChangeDelegate(List<Item> items);

        public delegate void HoverChangeDelegate(Item item);

        public delegate void HoverClearedDelegate();

        public InventoryChangeDelegate SelectionChanged = i => { };
        public HoverChangeDelegate HoverChanged = i => { };
        public HoverClearedDelegate HoverCleared = () => { };

        public InventoryDialog()
        {
            InitializeComponent();
        }

        public void SetInventory(List<Item> inventory)
        {
            _inventory = inventory;

            _itemList.Items.Clear();
            _itemList.SelectedItems.Clear();

            foreach (var item in _inventory)
            {
                _itemList.Items.Add(item.Name);
            }

            _itemList.SelectionChanged += ItemSelectionChanged;
            _itemList.HoverChange += (i) => HoverChanged.Invoke(_inventory[i]);
            _itemList.HoverCleared += () => HoverCleared.Invoke();
        }

        private void ItemSelectionChanged(object sender, EventArgs e)
        {
            SelectedItems = _itemList.SelectedItems.Select(i => _inventory[i]).ToList();
            SelectionChanged.Invoke(SelectedItems);
        }
    }
}

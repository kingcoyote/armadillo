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
        private readonly List<Item> _inventory;
        private Item _hover;

        public delegate void InventoryChangeDelegate(List<Item> items);

        public delegate void HoverChangeDelegate(Item item);

        public InventoryChangeDelegate SelectionChanged = i => { };
        public HoverChangeDelegate HoverChanged = i => { };

        public InventoryDialog(List<Item> inventory)
        {
            _inventory = inventory;

            InitializeComponent();

            foreach (var item in _inventory)
            {
                _itemList.Items.Add(item.Name);
            }

            _itemList.SelectionChanged += ItemSelectionChanged;
            _itemList.HoverChange += (i) => HoverChanged.Invoke(_inventory[i]);
        }

        private void ItemSelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged.Invoke(_itemList.SelectedItems.Select(i => _inventory[i]).ToList());
        }
    }
}

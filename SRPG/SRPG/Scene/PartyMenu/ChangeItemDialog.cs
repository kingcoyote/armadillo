using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class ChangeItemDialog
    {
        public delegate void ItemSelectedDelegate(Item item);
        public ItemSelectedDelegate ItemSelected = i => { };
        public Action ItemCancelled = () => { };

        public ChangeItemDialog()
        {
            InitializeComponent();
        }

        public void SetItems(IEnumerable<Item> items)
        {
            _itemList.Items.Clear();
            foreach (var item in items)
                _itemList.Items.Add(item.Name);
        }
    }
}

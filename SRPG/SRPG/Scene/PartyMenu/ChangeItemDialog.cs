using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface;

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
            _itemList.Children.Clear();
            for (var i = 0; i < items.Count(); i++)
            {
                var item = items.ElementAt(i);
                var button = new ButtonControl();
                button.Bounds = new UniRectangle(10, 5 + 50*i, new UniScalar(1.0f, -20.0f), 40);
                button.Pressed += (s, a) => ItemSelected.Invoke(item);
                button.Text = item.Name;
                _itemList.Children.Add(button);
            }
        }
    }
}

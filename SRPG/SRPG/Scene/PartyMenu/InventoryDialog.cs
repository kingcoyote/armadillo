using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;
using Nuclex.UserInterface;

namespace SRPG.Scene.PartyMenu
{
    public partial class InventoryDialog
    {
        public InventoryDialog()
        {
            InitializeComponent();
        }

        public void SetInventory(IEnumerable<Item> items)
        {
            Children.Clear();

            for (var i = 0; i < items.Count(); i++)
            {
                var item = items.ElementAt(i);
                var display = new ItemDisplay(item);
                display.Bounds = new UniRectangle(
                    new UniScalar(10), new UniScalar(10 + 50 * i),
                    new UniScalar(1.0f, -20.0f), new UniScalar(40)
                );
                Children.Add(display);
            }
        }
    }
}

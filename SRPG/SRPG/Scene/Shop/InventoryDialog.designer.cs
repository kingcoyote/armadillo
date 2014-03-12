using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Shop
{
    public partial class InventoryDialog
    {
        private InventoryListControl _itemList;

        private void InitializeComponent()
        {
            _itemList = new InventoryListControl();
            _itemList.Bounds = new UniRectangle(
                new UniScalar(10), new UniScalar(10),
                new UniScalar(1.0f, -20), new UniScalar(1.0f, -20) 
            );
            _itemList.SelectionMode = ListSelectionMode.Multi;
            Children.Add(_itemList);
        }
    }
}

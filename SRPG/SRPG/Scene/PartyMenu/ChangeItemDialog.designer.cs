using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class ChangeItemDialog : WindowControl
    {
        private ListControl _itemList;

        private void InitializeComponent()
        {
            Title = "Select Item";
            Bounds = new Nuclex.UserInterface.UniRectangle(200, 200, 200, 400);

            _itemList = new ListControl();
            _itemList.Bounds = new UniRectangle(
                new UniScalar(0, 10), new UniScalar(0, 10),
                new UniScalar(1, -20), new UniScalar(1, -20)
            );
            Children.Add(_itemList);
        }
    }
}

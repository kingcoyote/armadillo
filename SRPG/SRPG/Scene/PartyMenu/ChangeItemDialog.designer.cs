using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.PartyMenu
{
    public partial class ChangeItemDialog : WindowControl
    {
        private Control _itemList;

        private void InitializeComponent()
        {
            Bounds = new Nuclex.UserInterface.UniRectangle(200, 200, 200, 400);

            _itemList = new ListControl();
            _itemList.Bounds = new UniRectangle(
                new UniScalar(0, 10), new UniScalar(0, 10),
                new UniScalar(1, -20), new UniScalar(1, -65)
            );
            Children.Add(_itemList);

            var cancelButton = new ButtonControl();
            cancelButton.Text = "Cancel";
            cancelButton.Pressed += (s, a) => { ItemCancelled.Invoke(); };
            cancelButton.Bounds = new UniRectangle(
                new UniScalar(0, 10), new UniScalar(1, -45),
                new UniScalar(1, -20), new UniScalar(35)
            );
            Children.Add(cancelButton);
        }
    }
}

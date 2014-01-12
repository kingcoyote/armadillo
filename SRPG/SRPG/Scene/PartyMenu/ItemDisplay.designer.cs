using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.PartyMenu
{
    public partial class ItemDisplay : WindowControl
    {
        private void InitializeComponent()
        {
            EnableDragging = false;

            var label = new LabelControl();
            label.Text = _item.Name;
            label.Bounds = new Nuclex.UserInterface.UniRectangle(10, 5, 80, 30);
            Children.Add(label);

            label = new LabelControl();
            label.Text = _item.ItemType.ToString();
            label.Bounds = new Nuclex.UserInterface.UniRectangle(210, 5, 80, 30);
            Children.Add(label);

            label = new LabelControl();
            label.Text = _item.Ability.Name;
            label.Bounds = new Nuclex.UserInterface.UniRectangle(410, 5, 80, 30);
            Children.Add(label);
        }
    }
}

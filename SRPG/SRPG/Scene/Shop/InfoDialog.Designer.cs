using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Shop
{
    partial class InfoDialog : WindowControl
    {
        private LabelControl _goldAmount;

        private void InitializeComponent()
        {
            var goldLabel = new LabelControl();
            goldLabel.Bounds = new UniRectangle(10, 10, 150, 25);
            goldLabel.Text = "Gold";
            Children.Add(goldLabel);

            _goldAmount = new LabelControl();
            _goldAmount.Bounds = new UniRectangle(20, 35, 150, 25);
            Children.Add(_goldAmount);
        }
    }
}

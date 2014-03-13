using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Shop
{
    partial class InfoDialog : WindowControl
    {
        public InfoDialog()
        {
            InitializeComponent();
        }

        public void UpdateInfo(int gold)
        {
            _goldAmount.Text = gold.ToString() + "g";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.LoadGame
{
    partial class SavedGameDialog : WindowControl
    {
        public delegate void OnSelectDelegate();
        public OnSelectDelegate OnSelect;

        public SavedGameDialog(Data.SaveGame game)
        {
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.PartyMenu
{
    partial class MenuDialog
    {
        public delegate void MenuChangedDelegate(string menu);
        public MenuChangedDelegate MenuChanged;
        public MenuDialog()
        {
            InitializeComponent();

            _settingsButton.Enabled = false;

            _statusButton.Pressed += (s, a) => MenuChanged("party");
            _inventoryButton.Pressed += (s, a) => MenuChanged("inventory");
            _settingsButton.Pressed += (s, a) => MenuChanged("settings");
        }
    }
}

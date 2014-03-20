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

            _number.Text = game.Number.ToString();
            _name.Text = game.Name;
            _gameTime.Text = new TimeSpan(game.GameTime * TimeSpan.TicksPerMillisecond).ToString();

            _loadButton.Pressed += (s, a) => OnSelect.Invoke();
        }
    }
}

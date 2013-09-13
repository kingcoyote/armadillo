using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.MainMenu
{
    public partial class MenuOptionsDialog
    {
        public EventHandler OnNewGamePressed;
        public EventHandler OnContinuePressed;
        public EventHandler OnLoadGamePressed;
        public EventHandler OnOptionsPressed;
        public EventHandler OnExitPressed;

        public MenuOptionsDialog()
        {
            InitializeComponent();

            _newGame.Pressed += (s, a) => OnNewGamePressed.Invoke(s, a);
            _continue.Pressed += (s, a) => OnContinuePressed.Invoke(s, a);
            _loadGame.Pressed += (s, a) => OnLoadGamePressed.Invoke(s, a);
            _options.Pressed += (s, a) => OnOptionsPressed.Invoke(s, a);
            _exit.Pressed += (s, a) => OnExitPressed.Invoke(s, a);
        }
    }
}

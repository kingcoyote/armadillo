using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.MainMenu
{
    public partial class MenuOptionsDialog
    {
        public delegate void MenuOptionDelegate();
        public event MenuOptionDelegate OnNewGamePressed;
        public event MenuOptionDelegate OnContinuePressed;
        public event MenuOptionDelegate OnLoadGamePressed;
        public event MenuOptionDelegate OnOptionsPressed;
        public event MenuOptionDelegate OnExitPressed;

        public MenuOptionsDialog()
        {
            InitializeComponent();

            _continue.Enabled = false;
            _loadGame.Enabled = false;

            _newGame.Pressed += (s, a) => OnNewGamePressed.Invoke();
            _continue.Pressed += (s, a) => OnContinuePressed.Invoke();
            _loadGame.Pressed += (s, a) => OnLoadGamePressed.Invoke();
            _options.Pressed += (s, a) => OnOptionsPressed.Invoke();
            _exit.Pressed += (s, a) => OnExitPressed.Invoke();
        }
    }
}

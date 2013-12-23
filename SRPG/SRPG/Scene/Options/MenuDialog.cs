using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Options
{
    public partial class MenuDialog : WindowControl
    {
        public delegate void MenuOptionDelegate();

        public MenuOptionDelegate OnReturnPressed;
        public MenuOptionDelegate OnMainMenuPressed;
        public MenuOptionDelegate OnExitPressed;

        public MenuDialog()
        {
            InitializeComponent();

            _return.Pressed += (s, a) => OnReturnPressed.Invoke();
            _mainMenu.Pressed += (s, a) => OnMainMenuPressed.Invoke();
            _exit.Pressed += (s, a) => OnExitPressed.Invoke();
        }
    }
}

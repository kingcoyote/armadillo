using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Options
{
    public partial class MenuDialog
    {
        private ButtonControl _return;
        private ButtonControl _mainMenu;
        private ButtonControl _exit;

        private void InitializeComponent()
        {
            EnableDragging = false;

            _return = new ButtonControl();
            _mainMenu = new ButtonControl();
            _exit = new ButtonControl();

            // new game
            _return.Text = "Return";
            _return.Bounds = new UniRectangle(15, 15, 150, 45);

            // continue
            _mainMenu.Text = "Main Menu";
            _mainMenu.Bounds = new UniRectangle(15, 65, 150, 45);

            // load game
            _exit.Text = "Exit";
            _exit.Bounds = new UniRectangle(15, 115, 150, 45);

            Children.Add(_return);
            Children.Add(_mainMenu);
            Children.Add(_exit);

            Bounds = new UniRectangle(10, 10, 180, 275);
        }
    }
}

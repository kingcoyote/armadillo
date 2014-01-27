using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.MainMenu
{
    partial class MenuOptionsDialog : WindowControl
    {
        private ButtonControl _newGame;
        private ButtonControl _continue;
        private ButtonControl _loadGame;
        private ButtonControl _options;
        private ButtonControl _exit;

        private void InitializeComponent()
        {
            EnableDragging = false;

            _newGame = new ButtonControl();
            _continue = new ButtonControl();
            _loadGame = new ButtonControl();
            _options = new ButtonControl();
            _exit = new ButtonControl();

            // new game
            _newGame.Text = "New Game";
            _newGame.Bounds = new UniRectangle(15, 15, 150, 45);

            // continue
            _continue.Text = "Continue";
            _continue.Bounds = new UniRectangle(15, 65, 150, 45);

            // load game
            _loadGame.Text = "Load Game";
            _loadGame.Bounds = new UniRectangle(15, 115, 150, 45);

            // options
            _options.Text = "Options";
            _options.Bounds = new UniRectangle(15, 165, 150, 45);

            // exit
            _exit.Text = "Exit";
            _exit.Bounds = new UniRectangle(15, 215, 150, 45);

            Children.Add(_newGame);
            Children.Add(_continue);
            Children.Add(_loadGame);
            Children.Add(_options);
            Children.Add(_exit);
        }
    }
}

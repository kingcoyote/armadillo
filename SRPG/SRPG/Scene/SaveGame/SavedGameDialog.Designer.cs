using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.SaveGame
{
    partial class SavedGameDialog
    {
        private LabelControl _number;
        private LabelControl _name;
        private LabelControl _gameTime;
        private ButtonControl _loadButton;

        private void InitializeComponent()
        {
            // file number
            _number = new LabelControl();
            _number.Bounds = new UniRectangle(10, 10, 50, 35);
            Children.Add(_number);

            // name
            _name = new LabelControl();
            _name.Bounds = new UniRectangle(60, 10, 300, 35);
            Children.Add(_name);

            // elapsed game time
            _gameTime = new LabelControl();
            _gameTime.Bounds = new UniRectangle(10, 55, 300, 35);

            Children.Add(_gameTime);

            // load button
            _loadButton = new ButtonControl();
            _loadButton.Text = "Overwrite";
            _loadButton.Bounds = new UniRectangle(
                new UniScalar(1.0f, -160), new UniScalar(10),
                new UniScalar(150), new UniScalar(35)
            );
            Children.Add(_loadButton);
        }
    }
}

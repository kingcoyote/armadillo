using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using Torch;
using Game = Torch.Game;

namespace SRPG.Data.Layers
{
    partial class DialogLayer : WindowControl
    {
        private LabelControl _dialogText;
        private ListControl _optionsList;
        private ButtonControl _nextButton;

        private void InitializeComponent()
        {
            EnableDragging = false;

            _dialogText = new LabelControl();
            _dialogText.Bounds = new UniRectangle(
                new UniScalar(0.0f, 15.0f), new UniScalar(0.0f, 15.0f),
                new UniScalar(1.0f, -30.0f), new UniScalar(1.0f, -85.0f)
            );
            Children.Add(_dialogText);

            _optionsList = new ListControl();
            _optionsList.Bounds = new UniRectangle(10, 10, new UniScalar(1, -20), 150);
            _optionsList.SelectionMode = ListSelectionMode.Single;

            _nextButton = new ButtonControl();
            _nextButton.Text = "Continue";
            _nextButton.Bounds = new UniRectangle(
                new UniScalar(1.0f, -110), new UniScalar(1.0f, -55f),
                new UniScalar(100), new UniScalar(45) 
            );
            Children.Add(_nextButton);

            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(1.0f, -225.0f),      
                new UniScalar(1.0f, 0.0f), new UniScalar(0.0f, 225.0f) 
            );
        }
    }
}

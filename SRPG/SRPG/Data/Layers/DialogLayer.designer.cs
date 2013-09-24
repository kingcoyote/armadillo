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

        private void InitializeComponent()
        {
            _dialogText = new LabelControl();
            _dialogText.Bounds = new UniRectangle(
                new UniScalar(0.0f, 15.0f), new UniScalar(0.0f, 15.0f),
                new UniScalar(1.0f, -30.0f), new UniScalar(1.0f, -30.0f)
            );
            Children.Add(_dialogText);

            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(1.0f, -250.0f),      
                new UniScalar(1.0f, 0.0f), new UniScalar(0.0f, 225.0f) 
            );
        }
    }
}

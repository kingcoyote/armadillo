using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Battle
{
    public partial class HUDDialog : WindowControl
    {
        private LabelControl _roundLabel;
        private LabelControl _roundNumber;
        private LabelControl _faction;
        private ButtonControl _endTurn;

        private void InitializeComponent()
        {
            EnableDragging = false;

            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(0.0f, 0.0f),
                new UniScalar(0.0f, 170.0f), new UniScalar(0.0f, 130.0f) 
            );

            _roundLabel = new LabelControl();
            _roundNumber = new LabelControl();
            _faction = new LabelControl();
            _endTurn = new ButtonControl();

            Children.Add(_roundLabel);
            Children.Add(_roundNumber);
            Children.Add(_faction);
            Children.Add(_endTurn);

            _faction.Bounds = new UniRectangle(
                new UniScalar(0.0f, 10.0f), new UniScalar(0.0f, 10.0f),
                new UniScalar(1.0f, -20.0f), new UniScalar(0.0f, 30.0f) 
            );

            _roundLabel.Bounds = new UniRectangle(
                new UniScalar(0.0f, 10.0f), new UniScalar(0.0f, 50.0f),
                new UniScalar(0.5f, -10.0f), new UniScalar(0.0f, 30.0f)
            );
            _roundLabel.Text = "Round: ";

            _roundNumber.Bounds = new UniRectangle(
                new UniScalar(0.5f, 10.0f), new UniScalar(0.0f, 50.0f),
                new UniScalar(0.5f, -10.0f), new UniScalar(0.0f, 30.0f)
            );

            _endTurn.Bounds = new UniRectangle(
                new UniScalar(0.0f, 10.0f), new UniScalar(0.0f, 90.0f),
                new UniScalar(1.0f, -20.0f), new UniScalar(0.0f, 30.0f)
            );
            _endTurn.Text = "End Turn";
        }
    }
}

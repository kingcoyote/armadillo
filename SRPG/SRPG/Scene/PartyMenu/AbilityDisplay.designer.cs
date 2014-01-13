using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class AbilityDisplay : Control
    {
        private void InitializeComponent()
        {
            Bounds = new UniRectangle(
                10, 10, new UniScalar(1.0f, -20), 35   
            );

            var label = new LabelControl();
            label.Text = _ability.Name;
            label.Bounds = new UniRectangle(10, 5, 180, 30);
            Children.Add(label);

            label = new LabelControl();
            label.Text = ((int)(_exp / 100)).ToString();
            label.Bounds = new UniRectangle(190, 5, 30, 30);
            Children.Add(label);

            var progress = new ProgressControl();
            progress.Progress = (_exp % 100) / 100.0f;
            progress.Bounds = new UniRectangle(225, 5, new UniScalar(1.0f, -270), new UniScalar(1.0f, -5.0f));
            Children.Add(progress);

            label = new LabelControl();
            label.Text = ((int)(_exp % 100)).ToString();
            label.Bounds = new UniRectangle(new UniScalar(1.0f, -35), 5, 30, 30);
            Children.Add(label);
        }
    }
}

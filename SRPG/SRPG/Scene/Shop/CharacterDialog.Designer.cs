using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Shop
{
    partial class CharacterDialog : WindowControl
    {
        private LabelControl _nameLabel;
        private LabelControl _classLabel;

        private LabelControl _defLabel;
        private LabelControl _attLabel;
        private LabelControl _wisLabel;
        private LabelControl _intLabel;
        private LabelControl _spdLabel;
        private LabelControl _hitLabel;

        private void InitializeComponent()
        {
            LabelControl label;
            
            // name
            _nameLabel = new LabelControl();
            _nameLabel.Bounds = new UniRectangle(10, 5, new UniScalar(1, -10), 25);
            Children.Add(_nameLabel);

            // class
            _classLabel = new LabelControl();
            _classLabel.Bounds = new UniRectangle(10, 35, new UniScalar(1, -10), 25);
            Children.Add(_classLabel);

            // dawish
            label = new LabelControl();
            label.Bounds = new UniRectangle(10, 75, new UniScalar(0.4f, -5), 25);
            label.Text = "DEF";
            Children.Add(label);

            _defLabel = new LabelControl();
            _defLabel.Bounds = new UniRectangle(new UniScalar(0.4f, 5), 75, new UniScalar(0.6f, -5), 25);
            Children.Add(_defLabel);

            label = new LabelControl();
            label.Bounds = new UniRectangle(10, 105, new UniScalar(0.4f, -5), 25);
            label.Text = "ATT";
            Children.Add(label);

            _attLabel = new LabelControl();
            _attLabel.Bounds = new UniRectangle(new UniScalar(0.4f, 5), 105, new UniScalar(0.6f, -5), 25);
            Children.Add(_attLabel);

            label = new LabelControl();
            label.Bounds = new UniRectangle(10, 135, new UniScalar(0.4f, -5), 25);
            label.Text = "WIS";
            Children.Add(label);

            _wisLabel = new LabelControl();
            _wisLabel.Bounds = new UniRectangle(new UniScalar(0.4f, 5), 135, new UniScalar(0.6f, -5), 25);
            Children.Add(_wisLabel);

            label = new LabelControl();
            label.Bounds = new UniRectangle(10, 165, new UniScalar(0.4f, -5), 25);
            label.Text = "INT";
            Children.Add(label);

            _intLabel = new LabelControl();
            _intLabel.Bounds = new UniRectangle(new UniScalar(0.4f, 5), 165, new UniScalar(0.6f, -5), 25);
            Children.Add(_intLabel);

            label = new LabelControl();
            label.Bounds = new UniRectangle(10, 195, new UniScalar(0.4f, -5), 25);
            label.Text = "SPD";
            Children.Add(label);

            _spdLabel = new LabelControl();
            _spdLabel.Bounds = new UniRectangle(new UniScalar(0.4f, 5), 195, new UniScalar(0.6f, -5), 25);
            Children.Add(_spdLabel);

            label = new LabelControl();
            label.Bounds = new UniRectangle(10, 225, new UniScalar(0.4f, -5), 25);
            label.Text = "HIT";
            Children.Add(label);

            _hitLabel = new LabelControl();
            _hitLabel.Bounds = new UniRectangle(new UniScalar(0.4f, 5), 225, new UniScalar(0.6f, -5), 25);
            Children.Add(_hitLabel);
        }
    }
}

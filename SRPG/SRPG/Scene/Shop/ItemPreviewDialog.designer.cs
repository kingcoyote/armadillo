using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.Shop
{
    partial class ItemPreviewDialog
    {
        private LabelControl _nameLabel;
        private LabelControl _typeLabel;

        private LabelControl _defLabel;
        private LabelControl _defText;
        private LabelControl _attLabel;
        private LabelControl _attText;
        private LabelControl _wisLabel;
        private LabelControl _wisText;
        private LabelControl _intLabel;
        private LabelControl _intText;
        private LabelControl _spdLabel;
        private LabelControl _spdText;
        private LabelControl _hitLabel;
        private LabelControl _hitText;

        private LabelControl _abilityLabel;
        private LabelControl _priceLabel;
        private LabelControl _rangeLabel;

        private void InitializeComponent()
        {
            // name
            _nameLabel = new LabelControl();
            _nameLabel.Bounds = new UniRectangle(10, 10, 150, 25);
            Children.Add(_nameLabel);

            // type
            _typeLabel = new LabelControl();
            _typeLabel.Bounds = new UniRectangle(10, 45, 150, 25);
            Children.Add(_typeLabel);

            // def
            _defLabel = new LabelControl();
            _defLabel.Bounds = new UniRectangle(10, 80, 75, 25);
            _defLabel.Text = "DEF";
            Children.Add(_defLabel);

            _defText = new LabelControl();
            _defText.Bounds = new UniRectangle(100, 80, 75, 25);
            Children.Add(_defText);

            // att
            _attLabel = new LabelControl();
            _attLabel.Bounds = new UniRectangle(10, 115, 75, 25);
            _attLabel.Text = "ATT";
            Children.Add(_attLabel);

            _attText = new LabelControl();
            _attText.Bounds = new UniRectangle(100, 115, 75, 25);
            Children.Add(_attText);

            // wis
            _wisLabel = new LabelControl();
            _wisLabel.Bounds = new UniRectangle(10, 150, 75, 25);
            _wisLabel.Text = "WIS";
            Children.Add(_wisLabel);

            _wisText = new LabelControl();
            _wisText.Bounds = new UniRectangle(100, 150, 75, 25);
            Children.Add(_wisText);

            // int
            _intLabel = new LabelControl();
            _intLabel.Bounds = new UniRectangle(10, 185, 75, 25);
            _intLabel.Text = "INT";
            Children.Add(_intLabel);

            _intText = new LabelControl();
            _intText.Bounds = new UniRectangle(100, 185, 75, 25);
            Children.Add(_intText);

            // spd
            _spdLabel = new LabelControl();
            _spdLabel.Bounds = new UniRectangle(10, 220, 75, 25);
            _spdLabel.Text = "SPD";
            Children.Add(_spdLabel);

            _spdText = new LabelControl();
            _spdText.Bounds = new UniRectangle(100, 220, 75, 25);
            Children.Add(_spdText);

            // hit
            _hitLabel = new LabelControl();
            _hitLabel.Bounds = new UniRectangle(10, 255, 75, 25);
            _hitLabel.Text = "HIT";
            Children.Add(_hitLabel);

            _hitText = new LabelControl();
            _hitText.Bounds = new UniRectangle(100, 255, 75, 25);
            Children.Add(_hitText);

            // ability
            _abilityLabel = new LabelControl();
            _abilityLabel.Bounds = new UniRectangle(10, 300, 150, 25);
            Children.Add(_abilityLabel);

            // price
            _priceLabel = new LabelControl();
            _priceLabel.Bounds = new UniRectangle(new UniScalar(1, -50), 10, 50, 25);
            Children.Add(_priceLabel);

            // range
            _hitLabel = new LabelControl();
            _hitLabel.Bounds = new UniRectangle(0, 0, 0, 0);
            Children.Add(_hitLabel);
        }
    }
}

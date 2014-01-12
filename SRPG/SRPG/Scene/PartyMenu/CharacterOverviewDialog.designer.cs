﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterOverviewDialog : WindowControl
    {
        private LabelControl _nameText;
        private LabelControl _classText;

        private LabelControl _healthLabel;
        private LabelControl _manaLabel;
        private LabelControl _healthText;
        private LabelControl _manaText;

        private void InitializeComponent()
        {
            EnableDragging = false;

            // todo portrait (top right)

            // name (top left)
            _nameText = new LabelControl();
            _nameText.Bounds = new UniRectangle(10, 10, 150, 25);
            Children.Add(_nameText);

            // class (top left)
            _classText = new LabelControl();
            _classText.Bounds = new UniRectangle(10, 45, 150, 25);
            Children.Add(_classText);

            // health
            _healthLabel = new LabelControl();
            _healthLabel.Bounds = new UniRectangle(10, 105, 150, 25);
            _healthLabel.Text = "Health";
            Children.Add(_healthLabel);

            _healthText = new LabelControl();
            _healthText.Bounds = new UniRectangle(170, 105, 150, 25);
            Children.Add(_healthText);

            // mana
            _manaLabel = new LabelControl();
            _manaLabel.Bounds = new UniRectangle(10, 140, 150, 25);
            _manaLabel.Text = "Mana";
            Children.Add(_manaLabel);

            _manaText = new LabelControl();
            _manaText.Bounds = new UniRectangle(170, 140, 150, 25);
            Children.Add(_manaText);
        }
    }
}

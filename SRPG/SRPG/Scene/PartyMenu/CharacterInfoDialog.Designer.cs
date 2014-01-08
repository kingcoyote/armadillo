using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterInfoDialog
    {
        private LabelControl _nameText;
        private LabelControl _classText;

        private LabelControl _healthLabel;
        private LabelControl _manaLabel;
        private LabelControl _healthText;
        private LabelControl _manaText;

        private CharacterStatsDialog _statsControl;
        private CharacterWeaponDialog _weaponDialog;

        private void InitializeComponent()
        {
            Bounds = new UniRectangle(
                new UniScalar(0.2f, 15.0f), new UniScalar(0.0f, 105.0f),
                new UniScalar(0.8f, -15.0f), new UniScalar(1.0f, -105.0f)
            );

            // portrait (top right)

            // name (top left)
            _nameText = new LabelControl();
            _nameText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 20.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_nameText);

            // class (top left)
            _classText = new LabelControl();
            _classText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 55.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_classText);

            // health
            _healthLabel = new LabelControl();
            _healthLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 125.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _healthLabel.Text = "Health";
            Children.Add(_healthLabel);

            _healthText = new LabelControl();
            _healthText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 180.0F), new UniScalar(0.0F, 125.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_healthText);

            // mana
            _manaLabel = new LabelControl();
            _manaLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 160.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _manaLabel.Text = "Mana";
            Children.Add(_manaLabel);

            _manaText = new LabelControl();
            _manaText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 180.0F), new UniScalar(0.0F, 160.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_manaText);

            _statsControl = new CharacterStatsDialog();
            _statsControl.Bounds = new UniRectangle(
                new UniScalar(0.6F, 0F), new UniScalar(0.0F, 20.0F),
                new UniScalar(0.4F, -30.0F), new UniScalar(0.0f, 315.0f)
            );
            Children.Add(_statsControl);

            _weaponDialog = new CharacterWeaponDialog();
            _weaponDialog.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(0.5F, 0.0F), new UniScalar(0.0f, 95.0f)
            );
            Children.Add(_weaponDialog);

            // abilities
        }

        public void Hide()
        {
            if (Bounds.Location.X.Offset < -50000) return;

            Bounds.Location.X.Offset -= 50000;
        }

        public void Show()
        {
            if (Bounds.Location.X.Offset > -40000) return;

            Bounds.Location.X.Offset += 50000;
        }
    }
}

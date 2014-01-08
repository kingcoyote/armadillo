using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterStatsDialog
    {
        private LabelControl _nameText;
        private LabelControl _classText;

        private LabelControl _healthLabel;
        private LabelControl _manaLabel;
        private LabelControl _healthText;
        private LabelControl _manaText;

        private CharacterAbilityDialog _abilityControl;

        private LabelControl _weaponLabel;
        private LabelControl _weaponText;
        private LabelControl _armorLabel;
        private LabelControl _armorText;
        private LabelControl _accLabel;
        private LabelControl _accText;

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

            _abilityControl = new CharacterAbilityDialog();
            _abilityControl.Bounds = new UniRectangle(
                new UniScalar(0.6F, 0F), new UniScalar(0.0F, 20.0F),
                new UniScalar(0.4F, -30.0F), new UniScalar(0.0f, 315.0f)
            );
            Children.Add(_abilityControl);

            // 
            // weaponLabel
            //
            _weaponLabel = new LabelControl();
            _weaponLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _weaponLabel.Text = "Weapon";
            Children.Add(_weaponLabel);

            //
            // weaponText
            //
            _weaponText = new LabelControl();
            _weaponText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 175.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_weaponText);

            //
            // armorLabel
            //
            _armorLabel = new LabelControl();
            _armorLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 265.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _armorLabel.Text = "Armor";
            Children.Add(_armorLabel);

            // 
            // armorText
            //
            _armorText = new LabelControl();
            _armorText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 175.0F), new UniScalar(0.0F, 265.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_armorText);

            //
            // accLabel
            //
            _accLabel = new LabelControl();
            _accLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 300.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _accLabel.Text = "Accessory";
            Children.Add(_accLabel);

            //
            // accText
            //
            _accText = new LabelControl();
            _accText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 175.0F), new UniScalar(0.0F, 300.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_accText);

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

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

            //
            // defLabel
            //
            _defLabel = new LabelControl();
            _defLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _defLabel.Text = "DEF";
            Children.Add(_defLabel);

            //
            // defText
            //
            _defText = new LabelControl();
            _defText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 75.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_defText);

            //
            // attLabel
            //
            _attLabel = new LabelControl();
            _attLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 130.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _attLabel.Text = "ATT";
            Children.Add(_attLabel);

            //
            // attText
            //
            _attText = new LabelControl();
            _attText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 185.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_attText);

            //
            // wisLabel
            //
            _wisLabel = new LabelControl();
            _wisLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 240.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _wisLabel.Text = "WIS";
            Children.Add(_wisLabel);

            //
            // wisText
            //
            _wisText = new LabelControl();
            _wisText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 295.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_wisText);

            //
            // intLabel
            //
            _intLabel = new LabelControl();
            _intLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 350.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _intLabel.Text = "INT";
            Children.Add(_intLabel);

            //
            // intText
            //
            _intText = new LabelControl();
            _intText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 405.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_intText);

            //
            // spdLabel
            //
            _spdLabel = new LabelControl();
            _spdLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 460.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _spdLabel.Text = "SPD";
            Children.Add(_spdLabel);

            //
            // spdText
            //
            _spdText = new LabelControl();
            _spdText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 515.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_spdText);

            //
            // hitLabel
            //
            _hitLabel = new LabelControl();
            _hitLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 570.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _hitLabel.Text = "HIT";
            Children.Add(_hitLabel);

            //
            // hitText
            //
            _hitText = new LabelControl();
            _hitText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 625.0F), new UniScalar(0.0F, 230.0F),
                new UniScalar(50.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_hitText);

            // 
            // weaponLabel
            //
            _weaponLabel = new LabelControl();
            _weaponLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 300.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _weaponLabel.Text = "Weapon";
            Children.Add(_weaponLabel);

            //
            // weaponText
            //
            _weaponText = new LabelControl();
            _weaponText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 175.0F), new UniScalar(0.0F, 300.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_weaponText);

            //
            // armorLabel
            //
            _armorLabel = new LabelControl();
            _armorLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 335.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _armorLabel.Text = "Armor";
            Children.Add(_armorLabel);

            // 
            // armorText
            //
            _armorText = new LabelControl();
            _armorText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 175.0F), new UniScalar(0.0F, 335.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_armorText);

            //
            // accLabel
            //
            _accLabel = new LabelControl();
            _accLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 370.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _accLabel.Text = "Accessory";
            Children.Add(_accLabel);

            //
            // accText
            //
            _accText = new LabelControl();
            _accText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 175.0F), new UniScalar(0.0F, 370.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_accText);

            // abilities
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterWeaponDialog : Control
    {
        private LabelControl _weaponLabel;
        private LabelControl _weaponText;
        private LabelControl _armorLabel;
        private LabelControl _armorText;
        private LabelControl _accLabel;
        private LabelControl _accText;

        private void InitializeComponent()
        {
            // 
            // weaponLabel
            //
            _weaponLabel = new LabelControl();
            _weaponLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 0.0F), new UniScalar(0.0F, 0.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _weaponLabel.Text = "Weapon";
            Children.Add(_weaponLabel);

            //
            // weaponText
            //
            _weaponText = new LabelControl();
            _weaponText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 155.0F), new UniScalar(0.0F, 0.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_weaponText);

            //
            // armorLabel
            //
            _armorLabel = new LabelControl();
            _armorLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 0.0F), new UniScalar(0.0F, 35.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _armorLabel.Text = "Armor";
            Children.Add(_armorLabel);

            // 
            // armorText
            //
            _armorText = new LabelControl();
            _armorText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 155.0F), new UniScalar(0.0F, 35.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_armorText);

            //
            // accLabel
            //
            _accLabel = new LabelControl();
            _accLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 0.0F), new UniScalar(0.0F, 70.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _accLabel.Text = "Accessory";
            Children.Add(_accLabel);

            //
            // accText
            //
            _accText = new LabelControl();
            _accText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 155.0F), new UniScalar(0.0F, 70.0F),
                new UniScalar(450.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_accText);
        }
    }
}

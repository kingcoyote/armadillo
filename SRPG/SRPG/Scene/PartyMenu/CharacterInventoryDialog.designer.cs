using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterInventoryDialog : InventoryWindowControl
    {
        private LabelControl _weaponLabel;
        private LabelControl _weaponText;
        private ButtonControl _weaponButton;
        private LabelControl _armorLabel;
        private LabelControl _armorText;
        private ButtonControl _armorButton;
        private LabelControl _accLabel;
        private LabelControl _accText;
        private ButtonControl _accButton;

        public Action WeaponChange = () => { };
        public Action ArmorChange = () => { };
        public Action AccessoryChange = () => { };

        private void InitializeComponent()
        {
            EnableDragging = false;

            // 
            // weaponLabel
            //
            _weaponLabel = new LabelControl();
            _weaponLabel.Bounds = new UniRectangle(10, 13, 150, 25);
            _weaponLabel.Text = "Weapon";
            Children.Add(_weaponLabel);

            //
            // weaponText
            //
            _weaponText = new LabelControl();
            _weaponText.Bounds = new UniRectangle(130, 13, 170, 25);
            Children.Add(_weaponText);

            //
            // weaponButton
            //
            _weaponButton = new ButtonControl();
            _weaponButton.Bounds = new UniRectangle(new UniScalar(1.0f, -110), 10, 100, 30);
            _weaponButton.Text = "Change";
            _weaponButton.Pressed += (s, a) => WeaponChange.Invoke();
            Children.Add(_weaponButton);

            //
            // armorLabel
            //
            _armorLabel = new LabelControl();
            _armorLabel.Bounds = new UniRectangle(10, 48, 150, 25);
            _armorLabel.Text = "Armor";
            Children.Add(_armorLabel);

            // 
            // armorText
            //
            _armorText = new LabelControl();
            _armorText.Bounds = new UniRectangle(130, 48, 170, 25);
            Children.Add(_armorText);

            //
            // armorButton
            //
            _armorButton = new ButtonControl();
            _armorButton.Bounds = new UniRectangle(new UniScalar(1.0f, -110), 45, 100, 30);
            _armorButton.Text = "Change";
            _armorButton.Pressed += (s, a) => ArmorChange.Invoke();
            Children.Add(_armorButton);

            //
            // accLabel
            //
            _accLabel = new LabelControl();
            _accLabel.Bounds = new UniRectangle(10, 83, 150, 25);
            _accLabel.Text = "Accessory";
            Children.Add(_accLabel);

            //
            // accText
            //
            _accText = new LabelControl();
            _accText.Bounds = new UniRectangle(130, 83, 170, 25);
            Children.Add(_accText);

            //
            // accButton
            //
            _accButton = new ButtonControl();
            _accButton.Bounds = new UniRectangle(new UniScalar(1.0f, -110), 80, 100, 30);
            _accButton.Text = "Change";
            _accButton.Pressed += (s, a) => AccessoryChange.Invoke();
            Children.Add(_accButton);
        }
    }
}

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

        private void InitializeComponent()
        {
            // 
            // weaponLabel
            //
            _weaponLabel = new LabelControl();
            _weaponLabel.Bounds = new UniRectangle(0, 3, 150, 25);
            _weaponLabel.Text = "Weapon";
            Children.Add(_weaponLabel);

            //
            // weaponText
            //
            _weaponText = new LabelControl();
            _weaponText.Bounds = new UniRectangle(130, 3, 170, 25);
            Children.Add(_weaponText);

            //
            // weaponButton
            //
            _weaponButton = new ButtonControl();
            _weaponButton.Bounds = new UniRectangle(300, 0, 200, 30);
            _weaponButton.Text = "Change Weapon";
            Children.Add(_weaponButton);

            //
            // armorLabel
            //
            _armorLabel = new LabelControl();
            _armorLabel.Bounds = new UniRectangle(0, 38, 150, 25);
            _armorLabel.Text = "Armor";
            Children.Add(_armorLabel);

            // 
            // armorText
            //
            _armorText = new LabelControl();
            _armorText.Bounds = new UniRectangle(130, 38, 170, 25);
            Children.Add(_armorText);

            //
            // armorButton
            //
            _armorButton = new ButtonControl();
            _armorButton.Bounds = new UniRectangle(300, 35, 200, 30);
            _armorButton.Text = "Change Armor";
            Children.Add(_armorButton);

            //
            // accLabel
            //
            _accLabel = new LabelControl();
            _accLabel.Bounds = new UniRectangle(0, 73, 150, 25);
            _accLabel.Text = "Accessory";
            Children.Add(_accLabel);

            //
            // accText
            //
            _accText = new LabelControl();
            _accText.Bounds = new UniRectangle(130, 73, 170, 25);
            Children.Add(_accText);

            //
            // weaponButton
            //
            _accButton = new ButtonControl();
            _accButton.Bounds = new UniRectangle(300, 70, 200, 30);
            _accButton.Text = "Change Accessory";
            Children.Add(_accButton);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterWeaponDialog
    {
        public CharacterWeaponDialog()
        {
            InitializeComponent();
        }

        public void UpdateCharacter(Combatant character)
        {
            var weapon = character.GetEquippedWeapon();
            var armor = character.GetEquippedArmor();
            var acc = character.GetEquippedAccessory();

            _weaponText.Text = weapon == null ? "---" : weapon.Name;
            _armorText.Text = armor == null ? "---" : armor.Name;
            _accText.Text = acc == null ? "---" : acc.Name;
        }
    }
}

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
            _weaponText.Text = character.GetEquippedWeapon().Name;
            _armorText.Text = character.GetEquippedArmor().Name;
        }
    }
}

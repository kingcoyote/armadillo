using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterOverviewDialog
    {
        public CharacterOverviewDialog()
        {
            InitializeComponent();
        }

        public void UpdateCharacter(Combatant character)
        {
            _nameText.Text = character.Name;
            _classText.Text = character.Class;

            _healthText.Text = character.MaxHealth.ToString();
            _manaText.Text = character.MaxMana.ToString();
        }
    }
}

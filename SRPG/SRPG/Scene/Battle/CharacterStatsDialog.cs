using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.Battle
{
    public partial class CharacterStatsDialog
    {
        public bool Visible;

        public CharacterStatsDialog()
        {
            InitializeComponent();
        }

        public void SetCharacter(Combatant character)
        {
            _name.Text = character.Name;
            _class.Text = character.Class;
            _health.Text = character.CurrentHealth.ToString();
            _mana.Text = character.CurrentMana.ToString();
            _healthBar.Progress = (float)character.CurrentHealth/(float)character.MaxHealth;
            _manaBar.Progress = (float)character.CurrentMana/(float)character.MaxMana;
        }
    }
}

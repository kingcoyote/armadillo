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
        private readonly float _locX;

        public CharacterStatsDialog()
        {
            InitializeComponent();
            _locX = Bounds.Location.X.Offset;
        }

        public void SetCharacter(Combatant character)
        {
            _name.Text = character.Name;
            _class.Text = character.Class;
            _health.Text = character.CurrentHealth.ToString();
            _mana.Text = character.CurrentMana.ToString();
            _healthBar.Progress = character.CurrentHealth/character.MaxHealth;
            _manaBar.Progress = character.CurrentMana/character.MaxMana;
        }

        public void SetVisibility(bool visible)
        {
            Bounds.Location.X.Offset = visible ? _locX : -10000;
        }
    }
}

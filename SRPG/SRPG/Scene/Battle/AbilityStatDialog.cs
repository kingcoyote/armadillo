using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.Battle
{
    public partial class AbilityStatDialog
    {
        private Ability _ability;

        public AbilityStatDialog()
        {
            InitializeComponent();
        }

        public void SetAbility(Ability ability)
        {
            _ability = ability;
            UpdateText();
        }

        private void UpdateText()
        {
            _name.Text = _ability.Name;
            _mana.Text = _ability.ManaCost.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterAbilityDialog
    {
        public CharacterAbilityDialog()
        {
            InitializeComponent();
        }

        public void UpdateCharacter(Combatant character)
        {
            Children.Clear();
            var abilities = character.GetAbilities();
            for (var i = 0; i < abilities.Count(); i++)
            {
                var ability = abilities.ElementAt(i);
                var exp = character.AbilityExperienceLevels.ContainsKey(ability) ? character.AbilityExperienceLevels[ability] : 0;
                var display = new AbilityDisplay(ability, exp);
                display.Bounds.Location.Y.Offset = 10 + 30 * i;
                Children.Add(display);
            }
        }
    }
}

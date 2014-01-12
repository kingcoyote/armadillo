using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    public partial class AbilityDisplay
    {
        private Ability _ability;
        private int _exp;

        public AbilityDisplay(Ability ability, int exp)
        {
            _ability = ability;
            _exp = exp;

            InitializeComponent();
        }
    }
}

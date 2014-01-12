using System;
using System.Collections.Generic;
using System.Linq;
using Torch.UserInterface;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class PartyMenuDialog : WindowControl
    {
        private List<Combatant> _party;

        public delegate void PartyMenuDelegate(Combatant combatant);
        public event PartyMenuDelegate OnCharacterChange;

        public PartyMenuDialog(List<Combatant> party)
        {
            _party = party;

            InitializeComponent();
        }

        private void ChangeCharacter(Combatant combatant)
        {
            OnCharacterChange.Invoke(combatant);
        }
    }
}

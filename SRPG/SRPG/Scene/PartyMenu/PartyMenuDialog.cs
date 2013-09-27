using System.Collections.Generic;
using System.Linq;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class PartyMenuDialog : WindowControl
    {
        private List<Combatant> _party;

        public PartyMenuDialog(List<Combatant> party) : base()
        {
            _party = party;

            InitializeComponent();
        }
    }
}

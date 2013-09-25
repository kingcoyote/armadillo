using System.Collections.Generic;
using System.Linq;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class StatusMenuDialog : WindowControl
    {
        private List<Combatant> _party;

        public StatusMenuDialog(List<Combatant> party) : base()
        {
            _party = party;

            InitializeComponent();
        }
    }
}

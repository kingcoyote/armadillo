using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class PartyMenuDialog
    {
        private Dictionary<Combatant, ButtonControl> _partyButtons = new Dictionary<Combatant, ButtonControl>();

        private void InitializeComponent()
        {
            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(0.0f, 105.0f),
                new UniScalar(0.2f, 0.0f), new UniScalar(1.0f, -105.0f)
            );
            EnableDragging = false;

            for (var i = 0; i < _party.Count; i++)
            {
                var combatant = _party[i];
                var partyButton = new ButtonControl();
                partyButton.Text = combatant.Name;
                partyButton.Bounds = new UniRectangle(
                    new UniScalar(0.0f, 15.0f), new UniScalar(0.0f, (75.0f * i) + 15.0f),
                    new UniScalar(1.0f, -30.0f), new UniScalar(0.0f, 65.0f)
                );
                partyButton.Pressed += (s, a) => ChangeCharacter(combatant);
                Children.Add(partyButton);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.Shop
{
    partial class PartyDialog : WindowControl
    {
        private List<Combatant> _party;
        private List<CharacterDialog> _characterDialogs;

        public PartyDialog(List<Combatant> party)
        {
            InitializeComponent();

            _party = party;

            _characterDialogs = new List<CharacterDialog>();

            for (var i = 0; i < _party.Count; i++)
            {
                var character = _party[i];
                var dialog = new CharacterDialog(character);
                dialog.Bounds = new UniRectangle(
                    new UniScalar(0.2f * i, 10), new UniScalar(10),
                    new UniScalar(0.2f, -20), new UniScalar(1.0f, -20)
                );
                Children.Add(dialog);

                _characterDialogs.Add(dialog);
            }
        }

        public void ResetParty()
        {
            foreach (var d in _characterDialogs)
            {
                d.ResetCharacter();
            }
        }

        public void PreviewItem(Item item)
        {
            foreach (var d in _characterDialogs)
            {
                d.PreviewItem(item);
            }
        }
    }
}

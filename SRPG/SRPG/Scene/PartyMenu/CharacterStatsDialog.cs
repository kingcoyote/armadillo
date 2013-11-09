using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterStatsDialog : WindowControl
    {
        private Combatant _character;

        public CharacterStatsDialog()
        {
            InitializeComponent();
        }

        public void SetCharacter(Combatant character)
        {
            _character = character;

            RefreshComponents();
        }

        private void RefreshComponents()
        {
            _nameText.Text = _character.Name;
            _classText.Text = _character.Class;
        }
    }
}

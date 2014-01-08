using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterInfoDialog : WindowControl
    {
        private Combatant _character;

        public CharacterInfoDialog()
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

            _healthText.Text = _character.MaxHealth.ToString();
            _manaText.Text = _character.MaxMana.ToString();

            _statsControl.UpdateCharacter(_character);
            _inventoryDialog.UpdateCharacter(_character);
        }
    }
}

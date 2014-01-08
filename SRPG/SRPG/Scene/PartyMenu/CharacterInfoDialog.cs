using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterInfoDialog : WindowControl
    {
        public CharacterInfoDialog()
        {
            InitializeComponent();
        }

        public void SetCharacter(Combatant character)
        {
            _overviewDialog.UpdateCharacter(character);
            _statsDialog.UpdateCharacter(character);
            _inventoryDialog.UpdateCharacter(character);
        }
    }
}

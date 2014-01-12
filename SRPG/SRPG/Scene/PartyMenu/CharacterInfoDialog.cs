using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterInfoDialog : WindowControl
    {
        private Combatant _character;

        public delegate void ChangeItemDelegate(Combatant character, ItemEquipType type);
        public ChangeItemDelegate ChangeItem = (c, t) => { };

        public CharacterInfoDialog()
        {
            InitializeComponent();

            _inventoryDialog.WeaponChange += () => ChangeItem.Invoke(_character, ItemEquipType.Weapon);
            _inventoryDialog.ArmorChange += () => ChangeItem.Invoke(_character, ItemEquipType.Armor);
            _inventoryDialog.AccessoryChange += () => ChangeItem.Invoke(_character, ItemEquipType.Accessory);
        }

        public void SetCharacter(Combatant character)
        {
            _character = character;

            _overviewDialog.UpdateCharacter(_character);
            _statsDialog.UpdateCharacter(_character);
            _inventoryDialog.UpdateCharacter(_character);
            _abilityDialog.UpdateCharacter(_character);
        }
    }
}

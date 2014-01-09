using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface.Visuals.Flat;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class PartyMenuScene : Torch.Scene
    {
        private string _currentMenu = "";

        private MenuDialog _menu;
        private PartyMenuDialog _partyMenuDialog;
        private CharacterInfoDialog _characterInfoDialog;

        private bool _changingItem = false;

        public PartyMenuScene(Game game) : base(game)
        {
            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());
            Components.Add(keyboard);

            _menu = new MenuDialog();
            Gui.Screen.Desktop.Children.Add(_menu);

            _partyMenuDialog = new PartyMenuDialog(((SRPGGame)Game).Party);
            _partyMenuDialog.OnCharacterChange += SetCharacter;
            Gui.Screen.Desktop.Children.Add(_partyMenuDialog);

            _characterInfoDialog = new CharacterInfoDialog();
            _characterInfoDialog.ChangeItem += ChangeItem;
            _characterInfoDialog.Hide();
            Gui.Screen.Desktop.Children.Add(_characterInfoDialog);

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");
        }

        protected override void OnEntered()
        {
            base.OnEntered();

            Game.IsMouseVisible = true;
        }

        protected override void OnLeaving()
        {
            base.OnLeaving();

            Game.IsMouseVisible = false;
        }

        public void ReturnToGame()
        {
            Game.PopScene();
        }

        public void ChangeMenu(string menu)
        {

        }

        public void SetCharacter(Combatant character)
        {
            _characterInfoDialog.SetCharacter(character);
            _characterInfoDialog.Show();
        }

        private void ChangeItem(Combatant character, ItemEquipType type)
        {
            if (_changingItem) return;
            _changingItem = true;

            var dialog = new ChangeItemDialog();
            var items = new List<Item>();
            var inventory = ((SRPGGame) Game).Inventory;
            foreach(var item in inventory)
            {
                switch(type)
                {
                    case ItemEquipType.Armor:
                        if (character.CanEquipArmor(item)) items.Add(item);
                        break;
                    case ItemEquipType.Weapon:
                        if (character.CanEquipWeapon(item)) items.Add(item);
                        break;
                    case ItemEquipType.Accessory:
                        items.Add(item);
                        break;
                }
            }
            dialog.SetItems(items);
            Gui.Screen.Desktop.Children.Add(dialog);
            
            dialog.ItemSelected += i =>
                {
                    inventory.Add(character.EquipItem(i));
                    dialog.Close();
                    _changingItem = false;
                };

            dialog.ItemCancelled += () =>
                {
                    dialog.Close();
                    _changingItem = false;
                };
        }
    }
}

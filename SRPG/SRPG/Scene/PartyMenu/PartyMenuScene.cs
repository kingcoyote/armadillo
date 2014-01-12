using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface;
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
        private InventoryDialog _inventoryDialog;

        private bool _changingItem = false;

        public PartyMenuScene(Game game) : base(game)
        {
            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());
            Components.Add(keyboard);

            _menu = new MenuDialog();
            _menu.MenuChanged += ChangeMenu;
            Gui.Screen.Desktop.Children.Add(_menu);

            _partyMenuDialog = new PartyMenuDialog(((SRPGGame)Game).Party);
            _partyMenuDialog.OnCharacterChange += SetCharacter;
            Gui.Screen.Desktop.Children.Add(_partyMenuDialog);

            _characterInfoDialog = new CharacterInfoDialog();
            _characterInfoDialog.ChangeItem += ChangeItem;
            Gui.Screen.Desktop.Children.Add(_characterInfoDialog);

            _inventoryDialog = new InventoryDialog();
            Gui.Screen.Desktop.Children.Add(_inventoryDialog);

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_gui.xml");
        }

        protected override void OnEntered()
        {
            base.OnEntered();

            Game.IsMouseVisible = true;

            ChangeMenu("party");
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
            if (menu == _currentMenu) return;

            _currentMenu = menu;

            HideGui(_partyMenuDialog);
            HideGui(_characterInfoDialog);

            HideGui(_inventoryDialog);

            switch (menu)
            {
                case "party":
                    ShowGui(_partyMenuDialog);
                    break;
                case "inventory":
                    _inventoryDialog.SetInventory(((SRPGGame)Game).Inventory);
                    ShowGui(_inventoryDialog);
                    break;
                case "settings":
                    break;
            }
        }

        public void SetCharacter(Combatant character)
        {
            _characterInfoDialog.SetCharacter(character);
            ShowGui(_characterInfoDialog);
        }

        private void ChangeItem(Combatant character, ItemEquipType type)
        {
            if (_changingItem) return;
            _changingItem = true;

            var dialog = new ChangeItemDialog();
            dialog.Bounds = new UniRectangle(
                new UniScalar(0.5f, 0 - 100), new UniScalar(0.5f, 0 - 200),
                200, 400
            );
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
            _characterInfoDialog.Children.Add(dialog);
            dialog.BringToFront();
            
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

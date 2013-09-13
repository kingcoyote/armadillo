using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class PartyMenuScene : Torch.Scene
    {
        public PartyMenuScene(Game game) : base(game) { }

        private string _currentMenu = "";

        private Menu _menu;
        private StatusMenu _statusMenu;
        private InventoryMenu _inventoryMenu;
        private SettingsMenu _settingsMenu;

        public override void Initialize()
        {
            base.Initialize();

            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Components.Add(keyboard);
            _menu = new Menu(this);
            _statusMenu = new StatusMenu(this);
            _inventoryMenu = new InventoryMenu(this);
            _settingsMenu = new SettingsMenu(this);

            Components.Add(_menu);
            Components.Add(_statusMenu);
            Components.Add(_inventoryMenu);
            Components.Add(_settingsMenu);
        }

        public override void Start()
        {
            base.Start();

            _statusMenu.Visible = false;
            _inventoryMenu.Visible = false;
            _settingsMenu.Visible = false;
        }

        public void ReturnToGame()
        {
            Game.ChangeScenes("overworld");
        }

        public void ChangeMenu(string menu)
        {
            _statusMenu.Visible = false;
            _inventoryMenu.Visible = false;
            _settingsMenu.Visible = false;

            _currentMenu = menu;

            switch(_currentMenu)
            {
                case "status":
                    _statusMenu.Visible = true;
                    _statusMenu.Reset();
                    break;
                case "inventory":
                    _inventoryMenu.Visible = true;
                    _inventoryMenu.Reset();
                    break;
                case "settings":
                    _settingsMenu.Visible = true;
                    _settingsMenu.Reset();
                    break;
            }
        }

        public void SetCharacter(Combatant character)
        {
            _statusMenu.SetCharacter(character);
        }
    }
}

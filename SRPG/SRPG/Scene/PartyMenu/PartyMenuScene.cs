using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface.Visuals.Flat;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class PartyMenuScene : Torch.Scene
    {
        public PartyMenuScene(Game game) : base(game) { }

        private string _currentMenu = "";

        private MenuDialog _menu;
        private StatusMenu _statusMenu;
        private InventoryMenu _inventoryMenu;
        private SettingsMenu _settingsMenu;

        public override void Initialize()
        {
            base.Initialize();

            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Components.Add(keyboard);
            _menu = new MenuDialog();
            _statusMenu = new StatusMenu(this, null);
            _inventoryMenu = new InventoryMenu(this, null);
            _settingsMenu = new SettingsMenu(this, null);

            Gui.Screen.Desktop.Children.Add(_menu);
            Components.Add(_statusMenu);
            Components.Add(_inventoryMenu);
            Components.Add(_settingsMenu);

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_menu.xml");

            Game.IsMouseVisible = true;
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

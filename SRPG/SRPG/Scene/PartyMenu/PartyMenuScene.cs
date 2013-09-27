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
        private PartyMenuDialog _partyMenuDialog;
        private CharacterStatsDialog _characterStatsDialog;

        private InventoryMenu _inventoryMenu;
        private SettingsMenu _settingsMenu;

        public override void Initialize()
        {
            base.Initialize();

            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Components.Add(keyboard);
            
            _menu = new MenuDialog();
            Gui.Screen.Desktop.Children.Add(_menu);

            _partyMenuDialog = new PartyMenuDialog(((SRPGGame)Game).Party);
            _partyMenuDialog.OnCharacterChange += SetCharacter;
            Gui.Screen.Desktop.Children.Add(_partyMenuDialog);

            _characterStatsDialog = new CharacterStatsDialog();
            Gui.Screen.Desktop.Children.Add(_characterStatsDialog);

            _inventoryMenu = new InventoryMenu(this, null);
            _settingsMenu = new SettingsMenu(this, null);
            Components.Add(_inventoryMenu);
            Components.Add(_settingsMenu);

            Gui.DrawOrder = 1000;
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_menu.xml");

            Game.IsMouseVisible = true;
        }

        public void ReturnToGame()
        {
            Game.ChangeScenes("overworld");
        }

        public void ChangeMenu(string menu)
        {

        }

        public void SetCharacter(Combatant character)
        {
           
        }
    }
}

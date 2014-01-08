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
        private CharacterStatsDialog _characterStatsDialog;

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

            _characterStatsDialog = new CharacterStatsDialog();
            _characterStatsDialog.Hide();
            Gui.Screen.Desktop.Children.Add(_characterStatsDialog);

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
            _characterStatsDialog.SetCharacter(character);
            _characterStatsDialog.Show();
        }
    }
}

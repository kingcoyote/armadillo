using Torch;

namespace SRPG.Scene.PartyMenu
{
    class PartyMenuScene : Torch.Scene
    {
        public PartyMenuScene(Game game) : base(game) { }

        private string _currentMenu = "";

        public override void Initialize()
        {
            base.Initialize();

            Layers.Add("keyboard input", new KeyboardInput(this));
            Layers.Add("main menu", new Menu(this));
            Layers.Add("status menu", new StatusMenu(this) { X = -10000 });
            Layers.Add("inventory menu", new InventoryMenu(this) { X = -1000 });
            Layers.Add("settings menu", new SettingsMenu(this) { X = -1000 });
        }

        public void ReturnToGame()
        {
            Game.ChangeScenes("overworld");
        }

        public void ChangeMenu(string menu)
        {
            if(Layers.ContainsKey(_currentMenu)) Layers[_currentMenu].X = -10000;
            _currentMenu = menu;
            Layers[_currentMenu].X = 350;
        }
    }
}

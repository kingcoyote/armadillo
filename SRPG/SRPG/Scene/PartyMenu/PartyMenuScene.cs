using Microsoft.Xna.Framework.Input;
using SRPG.Data;
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

            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));
            Layers.Add("keyboard input", keyboard);
            Layers.Add("main menu", new Menu(this));
            Layers.Add("status menu", new StatusMenu(this));
            Layers.Add("inventory menu", new InventoryMenu(this));
            Layers.Add("settings menu", new SettingsMenu(this));
        }

        public override void Start()
        {
            base.Start();

            Layers["status menu"].Visible = false;
            Layers["inventory menu"].Visible = false;
            Layers["settings menu"].Visible = false;
        }

        public void ReturnToGame()
        {
            Game.ChangeScenes("overworld");
        }

        public void ChangeMenu(string menu)
        {
            if(Layers.ContainsKey(_currentMenu)) Layers[_currentMenu].Visible = false;
            _currentMenu = menu;
            Layers[_currentMenu].Visible = true;
            ((SubmenuLayer)Layers[_currentMenu]).Reset();
        }

        public void SetCharacter(Character character)
        {
            ((StatusMenu)Layers["status menu"]).SetCharacter(character);
        }
    }
}

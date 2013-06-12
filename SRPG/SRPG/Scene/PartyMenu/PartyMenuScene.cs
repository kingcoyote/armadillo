using Torch;

namespace SRPG.Scene.PartyMenu
{
    class PartyMenuScene : Torch.Scene
    {
        public PartyMenuScene(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            Layers.Add("keyboard input", new KeyboardInput(this));
            Layers.Add("menu", new Menu(this));
            
        }

        public void ReturnToGame()
        {
            Game.ChangeScenes("overworld");
        }
    }
}

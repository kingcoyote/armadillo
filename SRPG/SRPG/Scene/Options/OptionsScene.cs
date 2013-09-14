using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.Options
{
    class OptionsScene : Torch.Scene
    {
        public OptionsScene(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.ChangeScenes("overworld"));

            Components.Add(keyboard);
            Components.Add(new Menu(this));
        }

        public override void Start()
        {
            Game.IsMouseVisible = true;
        }

        public override void Stop()
        {
            Game.IsMouseVisible = false;
        }
    }
}

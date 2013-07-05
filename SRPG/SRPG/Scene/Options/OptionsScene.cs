using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.Options
{
    class OptionsScene : Torch.Scene
    {
        public OptionsScene(Game game) : base(game)
        {
            var keyboard = new KeyboardInputLayer(this);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.GetInstance().ChangeScenes("overworld"));

            Layers.Add("keyboard", keyboard);
            Layers.Add("menu", new Menu(this));
        }

        public override void Initialize()
        {
            
        }
    }
}

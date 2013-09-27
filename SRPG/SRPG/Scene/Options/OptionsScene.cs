using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.Options
{
    class OptionsScene : Torch.Scene
    {
        public OptionsScene(Game game) : base(game)
        {
            
        }

        protected override void OnEntered()
        {
            base.OnEntered();

            var keyboard = new KeyboardInputLayer(this, null);
            keyboard.AddKeyDownBinding(Keys.Escape, () => Game.PopScene());

            Components.Add(keyboard);
            Components.Add(new Menu(this, null));
        }

        protected override void OnResume()
        {
            Game.IsMouseVisible = true;
        }

        protected override void OnPause()
        {
            Game.IsMouseVisible = false;
        }
    }
}

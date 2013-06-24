using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch;

namespace SRPG.Scene.MainMenu
{
    class MainMenu : Torch.Scene
    {
        public MainMenu(Game game) : base(game) { }
    
        public override void Initialize()
        {
            Layers.Add("bg", new BackgroundLayer(this, "MainMenu/bg"));
            Layers.Add("menuoptions", new MenuOptions(this));
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

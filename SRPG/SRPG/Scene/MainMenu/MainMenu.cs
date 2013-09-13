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
            base.Initialize();

            Components.Add(new BackgroundLayer(this, "MainMenu/bg"));
            Components.Add(new MenuOptions(this));
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

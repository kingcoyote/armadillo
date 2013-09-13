using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Visuals.Flat;
using Torch;

namespace SRPG.Scene.MainMenu
{
    class MainMenu : Torch.Scene
    {
        public MainMenu(Game game) : base(game) { }
    
        public override void Initialize()
        {
            base.Initialize();


            Components.Add(new BackgroundLayer(this, "MainMenu/bg") { DrawOrder = -10000 });
            Gui.Screen.Desktop.Children.Add(new MenuOptionsDialog());
            Gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, "Content/Gui/main_menu.xml");
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

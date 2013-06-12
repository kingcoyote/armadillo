using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class KeyboardInput : Layer
    {
        public KeyboardInput(Torch.Scene scene) : base(scene)
        {
            KeyDown += HandleKeyDown;
            KeyUp += HandleKeyUp;

            Objects.Add("background", new TextureObject() { Color = Color.Blue, Width = Torch.Game.GetInstance().Window.ClientBounds.Width, Height = Torch.Game.GetInstance().Window.ClientBounds.Height });
        }

        public void HandleKeyDown(object sender, KeyboardEventArgs args)
        {
            switch (args.WhichKey)
            {
                case Keys.Tab:
                case Keys.Escape:
                    ((PartyMenuScene) Scene).ReturnToGame();
                    break;
            }
        }

        public void HandleKeyUp(object sender, KeyboardEventArgs args)
        {
            
        }
    }
}

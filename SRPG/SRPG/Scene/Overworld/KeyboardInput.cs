using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Overworld
{
    class KeyboardInput : Layer
    {
        private readonly Dictionary<string, Keys> _controls = new Dictionary<string, Keys>(); 

        public KeyboardInput(Torch.Scene scene, Torch.Object parent) : base(scene, parent)
        {
            var keyboard = ((InputManager) scene.Game.Services.GetService(typeof (IInputService))).GetKeyboard();
            keyboard.KeyPressed += HandleKeyDown;

            _controls.Add("left", Keys.A);
            _controls.Add("right", Keys.D);
            _controls.Add("up", Keys.W);
            _controls.Add("down", Keys.S);
            _controls.Add("party menu", Keys.Tab);
            _controls.Add("config menu", Keys.Escape);
            _controls.Add("interact", Keys.E);
        }

        public void HandleKeyDown(Keys key)
        {
            var scene = ((OverworldScene) Scene);

            if (key == _controls["party menu"])
            {
                scene.OpenPartyMenu();
            }
            else if (key == _controls["config menu"])
            {
                ((SRPGGame)Game).ChangeScenes("options");
            }
            else if (key == _controls["interact"])
            {
                ((OverworldScene) Scene).Interact();
            }

        }
    }
}

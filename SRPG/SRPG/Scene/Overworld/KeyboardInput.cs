using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Overworld
{
    class KeyboardInput : Layer
    {
        private readonly Dictionary<string, Keys> _controls = new Dictionary<string, Keys>(); 

        public KeyboardInput(Torch.Scene scene) : base(scene)
        {
            KeyDown += HandleKeyDown;
            KeyUp += HandleKeyUp;

            _controls.Add("left", Keys.A);
            _controls.Add("right", Keys.D);
            _controls.Add("up", Keys.W);
            _controls.Add("down", Keys.S);
            _controls.Add("party menu", Keys.Tab);
            _controls.Add("config menu", Keys.Escape);
            _controls.Add("interact", Keys.E);
        }

        public void HandleKeyDown(object sender, KeyboardEventArgs args)
        {
            if (args.WhichKey == _controls["left"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Left, true);
            }
            else if (args.WhichKey == _controls["right"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Right, true);
            }
            else if (args.WhichKey == _controls["up"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Up, true);
            }
            else if (args.WhichKey == _controls["down"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Down, true);
            }
            else if (args.WhichKey == _controls["party menu"])
            {

            }
            else if (args.WhichKey == _controls["config menu"])
            {
                Game.GetInstance().Exit();
            }
            else if (args.WhichKey == _controls["interact"])
            {
                ((OverworldScene) Scene).Interact();
            }
        }

        public void HandleKeyUp(object sender, KeyboardEventArgs args)
        {
            if (args.WhichKey == _controls["left"])
            {
                ((OverworldScene) Scene).ChangeDirection(Direction.Left, false);
            }
            else if (args.WhichKey == _controls["right"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Right, false);
            }
            else if (args.WhichKey == _controls["up"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Up, false);
            }
            else if (args.WhichKey == _controls["down"])
            {
                ((OverworldScene)Scene).ChangeDirection(Direction.Down, false);
            }
        }
    }
}

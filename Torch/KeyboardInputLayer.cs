using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;

namespace Torch
{
    public class KeyboardInputLayer : Layer
    {
        private readonly Dictionary<Keys, List<Action>> _keyDownBindings = new Dictionary<Keys, List<Action>>();
        private readonly Dictionary<Keys, List<Action>> _keyUpBindings = new Dictionary<Keys, List<Action>>();
        
        public KeyboardInputLayer(Scene scene, Torch.Object parent) : base(scene, parent)
        {
            var keyboard = ((InputManager) scene.Game.Services.GetService(typeof (IInputService))).GetKeyboard();
            keyboard.KeyPressed += HandleKeyDown;
            keyboard.KeyReleased += HandleKeyUp;
        }

        public void AddKeyDownBinding(Keys key, Action callback)
        {
            if(!_keyDownBindings.Keys.Contains(key))
            {
                _keyDownBindings.Add(key, new List<Action>());
            }

            _keyDownBindings[key].Add(callback);
        }

        public void AddKeyUpBinding(Keys key, Action callback)
        {
            if (!_keyUpBindings.Keys.Contains(key))
            {
                _keyUpBindings.Add(key, new List<Action>());
            }

            _keyUpBindings[key].Add(callback);
        }

        private void HandleKeyDown(Keys key)
        {
            if (Scene.IsRunning == false) return;

            if(!_keyDownBindings.Keys.Contains(key)) return;

            _keyDownBindings[key].ForEach(e => e.Invoke());
        }

        private void HandleKeyUp(Keys key)
        {
            if (Scene.IsRunning == false) return;

            if (!_keyUpBindings.Keys.Contains(key)) return;

            _keyUpBindings[key].ForEach(e => e.Invoke());
        }
    }
}

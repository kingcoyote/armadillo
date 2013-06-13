using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Torch
{
    public class KeyboardInputLayer : Layer
    {
        private readonly Dictionary<Keys, List<Action>> _keyDownBindings = new Dictionary<Keys, List<Action>>();
        private readonly Dictionary<Keys, List<Action>> _keyUpBindings = new Dictionary<Keys, List<Action>>();
        
        public KeyboardInputLayer(Scene scene) : base(scene)
        {
            KeyDown += HandleKeyDown;
            KeyUp += HandleKeyUp;
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

        private void HandleKeyDown(object sender, KeyboardEventArgs args)
        {
            if(!_keyDownBindings.Keys.Contains(args.WhichKey)) return;

            _keyDownBindings[args.WhichKey].ForEach(e => e.Invoke());
        }

        private void HandleKeyUp(object sender, KeyboardEventArgs args)
        {
            if (!_keyUpBindings.Keys.Contains(args.WhichKey)) return;

            _keyUpBindings[args.WhichKey].ForEach(e => e.Invoke());
        }
    }
}

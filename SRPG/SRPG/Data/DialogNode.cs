using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch;

namespace SRPG.Data
{
    public class DialogNode
    {
        public string Text;
        public int Identifier;
        public EventHandler<DialogNodeEventArgs> OnEnter;
        public EventHandler<DialogNodeEventArgs> OnExit;
        public Dictionary<string, int> Options;
        public SpriteObject Sprite;
    }

    public class DialogNodeEventArgs : EventArgs { }
}

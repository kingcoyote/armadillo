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
        public EventHandler<DialogNodeEventArgs> OnEnter = ( (sender, args) => { });
        public EventHandler<DialogNodeEventArgs> OnExit = ((sender, args) => { });
        public Dictionary<string, int> Options = new Dictionary<string, int>();
        public SpriteObject Sprite;
    }

    public class DialogNodeEventArgs : EventArgs { }
}

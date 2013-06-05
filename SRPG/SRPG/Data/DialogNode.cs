using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch;

namespace SRPG.Data
{
    public struct DialogNode
    {
        public string Text;
        public EventHandler OnEntry;
        public Dictionary<string, int> Options;
        public SpriteObject Sprite;
    }
}

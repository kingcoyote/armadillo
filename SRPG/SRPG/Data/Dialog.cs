using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRPG.Data
{
    public class Dialog
    {
        public Dictionary<int, DialogNode> Nodes;
        public DialogNode CurrentNode { get; private set; }

        public void SetOption(int optionNumber)
        {
            throw new NotImplementedException();
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }
    }
}

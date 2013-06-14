using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    abstract class SubmenuLayer : Layer
    {
        protected SubmenuLayer(Torch.Scene scene) : base(scene) { }

        public virtual void Reset()
        {
            
        }
    }
}

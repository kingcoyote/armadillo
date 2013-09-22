using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    abstract class SubmenuLayer : Layer
    {
        protected SubmenuLayer(Torch.Scene scene, Torch.Object parent) : base(scene, parent) { }

        public virtual void Reset()
        {
            
        }
    }
}

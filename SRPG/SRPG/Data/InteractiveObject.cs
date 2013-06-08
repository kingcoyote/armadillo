using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SRPG.Data
{
    public class InteractiveObject
    {
        public Rectangle Location;
        public EventHandler<InteractEventArgs> Interact;
    }
}

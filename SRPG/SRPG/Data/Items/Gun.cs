using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRPG.Data.Items
{
    class Revolver : Item
    {
        public Revolver()
        {
            Name = "Revolver";
            ItemType = ItemType.Gun;
        }
    }
}

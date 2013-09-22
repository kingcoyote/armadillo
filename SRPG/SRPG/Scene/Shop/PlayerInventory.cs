using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.Shop
{
    class PlayerInventory : Inventory
    {
        public PlayerInventory(Torch.Scene scene, Torch.Object parent, List<Item> inventory) : base(scene, parent, inventory)
        {
        }
    }
}

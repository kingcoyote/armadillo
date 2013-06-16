using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SRPG.Data;

namespace SRPG.Scene.Shop
{
    class ShopInventory : Inventory
    {
        public ShopInventory(Torch.Scene scene, List<Item> inventory) : base(scene, inventory)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class InventoryMenu : SubmenuLayer
    {
        public InventoryMenu(Torch.Scene scene) : base(scene) { }

        public override void Reset()
        {
            var inventory = ((SRPGGame)Game).Inventory;
            var font = FontManager.Get("Menu");

            var x = 0;
            int y = 0;

            
        }
    }
}

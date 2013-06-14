using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;

namespace SRPG.Scene.PartyMenu
{
    class InventoryMenu : SubmenuLayer
    {
        public InventoryMenu(Torch.Scene scene) : base(scene) { }

        public override void Reset()
        {
            Objects.Clear();
            
            var inventory = ((SRPGGame)Torch.Game.GetInstance()).Inventory;
            var font = Torch.Game.GetInstance().Content.Load<SpriteFont>("menu");

            for (var i = 0; i < inventory.Count; i++)
            {
                var item = inventory[i];
                Objects.Add("inventory/" + i, new TextObject
                {
                    Color = Color.White,
                    X = 275,
                    Y = (i + 1) * 50,
                    Font = font,
                    Value = item.Name
                });
            }
        }
    }
}

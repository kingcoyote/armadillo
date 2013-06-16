using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Data;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class Inventory : Layer
    {
        private List<Item> _inventory;

        public Inventory(Torch.Scene scene, List<Item> inventory) : base(scene)
        {
            _inventory = inventory;

            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            for (var i = 0; i < inventory.Count; i++)
            {
                var item = inventory[i];

                Objects.Add("inventory/" + i, new TextObject
                    {
                        Font = font,
                        Color = Color.White,
                        Value = item.Name,
                        Y = i * 50
                    });
            }
        }
    }
}

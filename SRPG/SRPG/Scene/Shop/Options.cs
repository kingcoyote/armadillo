using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class Options : Layer
    {
        public Options(Torch.Scene scene) : base(scene)
        {
            var font = FontManager.Get("Menu");

            Objects.Add("buy", new TextObject()
                {
                    Font = font,
                    Color = Color.LightGreen,
                    Value = "Buy",
                    Y = 200,
                    X = Game.GraphicsDevice.Viewport.Width / 2 - 75,
                    Alignment = TextObject.AlignTypes.Center
                });

            Objects["buy"].MouseClick += BuyItems;

            Objects.Add("sell", new TextObject()
                {
                    Font = font,
                    Color = Color.LightGreen,
                    Value = "Sell",
                    Y = 200,
                    X = Game.GraphicsDevice.Viewport.Width / 2 + 75,
                    Alignment = TextObject.AlignTypes.Center
                });
            Objects["sell"].MouseClick += SellItems;
        }

        private void BuyItems(object sender, MouseEventArgs args)
        {
            ((ShopScene)Scene).BuySelectedItems();
        }

        private void SellItems(object sender, MouseEventArgs args)
        {
            ((ShopScene) Scene).SellSelectedItems();
        }
    }
}

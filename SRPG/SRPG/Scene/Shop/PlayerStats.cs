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
    class PlayerStats : Layer
    {
        public PlayerStats(Torch.Scene scene) : base(scene)
        {
            var font = FontManager.Get("Menu");

            Objects.Add("money", new TextObject()
                {
                    Font = font,
                    Color = Color.White,
                    Y = 75,
                    X = Game.GetInstance().GraphicsDevice.Viewport.Width - 425,
                    Alignment = TextObject.AlignTypes.Right,
                    Value = string.Format("{0}g", ((SRPGGame)Game.GetInstance()).Money)
                });
        }
    }
}

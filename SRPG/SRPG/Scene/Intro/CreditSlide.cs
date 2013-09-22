using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Intro
{
    class CreditSlide : Layer
    {
        public CreditSlide(Torch.Scene scene, Torch.Object parent) : base(scene, parent)
        {
            var font = FontManager.Get("Menu");

            Components.Add(new TextObject(Game, this)
            {
                Font = font,
                Color = Color.White,
                Value = "Steve Phillips",
                Alignment = TextObject.AlignTypes.Center,
                X = Game.GraphicsDevice.Viewport.Width / 2,
                Y = Game.GraphicsDevice.Viewport.Height / 2 - 25
            });

            Components.Add(new TextObject(Game, this)
                {
                    Font = font, 
                    Color = Color.White, 
                    Value = "George Emond", 
                    Alignment = TextObject.AlignTypes.Center, 
                    X = Game.GraphicsDevice.Viewport.Width / 2, 
                    Y = Game.GraphicsDevice.Viewport.Height / 2 + 25
                });
        }
    }
}

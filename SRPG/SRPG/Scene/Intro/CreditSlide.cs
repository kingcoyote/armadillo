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
        public CreditSlide(Torch.Scene scene) : base(scene)
        {
            var font = FontManager.Get("Menu");
            
            Objects.Add("steve", new TextObject
            {
                Font = font,
                Color = Color.White,
                Value = "Steve Phillips",
                Alignment = TextObject.AlignTypes.Center,
                X = Game.GetInstance().GraphicsDevice.Viewport.Width / 2,
                Y = Game.GetInstance().GraphicsDevice.Viewport.Height / 2 - 25
            });
            
            Objects.Add("george", new TextObject
                {
                    Font = font, 
                    Color = Color.White, 
                    Value = "George Emond", 
                    Alignment = TextObject.AlignTypes.Center, 
                    X = Game.GetInstance().GraphicsDevice.Viewport.Width / 2, 
                    Y = Game.GetInstance().GraphicsDevice.Viewport.Height / 2 + 25
                });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Shop
{
    class Background : Layer
    {
        public Background(Torch.Scene scene) : base(scene)
        {
            Objects.Add("background", new TextureObject()
            {
                Color = Color.Blue,
                Width = Game.GetInstance().GraphicsDevice.Viewport.Width,
                Height = Game.GetInstance().GraphicsDevice.Viewport.Height
            });
        }
    }
}

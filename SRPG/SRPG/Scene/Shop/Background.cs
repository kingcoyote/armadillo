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
            Components.Add(new TextureObject(Game)
            {
                Color = Color.Blue,
                Width = Game.GraphicsDevice.Viewport.Width,
                Height = Game.GraphicsDevice.Viewport.Height
            });
        }
    }
}

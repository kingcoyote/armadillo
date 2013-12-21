using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public class BackgroundLayer : Layer
    {
        public BackgroundLayer(Scene scene, Torch.Object parent, string image) : base(scene, parent)
        {
            Components.Add(new ImageObject(Game, this, image));
        }

        public override void Update(GameTime gametime) { }
    }
}

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
        private readonly Texture2D _backgroundImage;

        public BackgroundLayer(Scene scene, string image) : base(scene)
        {
            Components.Add(new ImageObject(Game, image));
        }

        public override void Update(GameTime gametime) { }
    }
}

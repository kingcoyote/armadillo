using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public class TextureObject : Object
    {
        public Color Color;
        public Color Alpha = Color.White;

        private readonly Texture2D _texture;

        public TextureObject()
        {
            _texture = new Texture2D(Game.GetInstance().GraphicsDevice, 1, 1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _texture.SetData(new [] {Color});
            spriteBatch.Draw(_texture, Rectangle, Alpha);
        }
    }
}

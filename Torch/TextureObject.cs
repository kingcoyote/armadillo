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

        public TextureObject(Microsoft.Xna.Framework.Game game, Torch.Object parent) : base(game, parent)
        {
            _texture = new Texture2D(GraphicsDevice, 1, 1);
        }

        public override void Draw(GameTime gametime)
        {
            _texture.SetData(new [] {Color});
            var spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, Rectangle, Alpha);
            spriteBatch.End();
        }
    }
}

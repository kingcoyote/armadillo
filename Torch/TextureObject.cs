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
            float offsetx = 0;
            float offsety = 0;

            if (Parent != null)
            {
                offsetx = Parent.OffsetX();
                offsety = Parent.OffsetY();
            }

            _texture.SetData(new [] {Color});
            _spriteBatch.Draw(_texture, new Rectangle((int)X + (int)offsetx, (int)Y + (int)offsety, Width, Height), Alpha);
        }
    }
}

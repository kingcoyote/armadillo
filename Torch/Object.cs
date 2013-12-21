using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public abstract class Object : DrawableGameComponent
    {
        protected SpriteBatch _spriteBatch;

        protected Object(Microsoft.Xna.Framework.Game game, Object parent) : base(game)
        {
            Parent = parent;
            _spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            Initialize();
        }

        public float OffsetX()
        {
            if (Parent == null) return X;

            return X + Parent.OffsetX();
        }

        public float OffsetY()
        {
            if (Parent == null) return Y;

            return Y + Parent.OffsetY();
        }

        public virtual float X { get; set; }
        public virtual float Y { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public Object Parent { get; set; }

        public virtual Rectangle Rectangle { get { return new Rectangle((int)X, (int)Y, Width, Height); }}
    }
}

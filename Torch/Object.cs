using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public abstract class Object : DrawableGameComponent
    {
        protected Object(Microsoft.Xna.Framework.Game game, Object parent) : base(game)
        {
            Parent = parent;
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
        public virtual Object Parent { get; private set; }

        public virtual Rectangle Rectangle { get { return new Rectangle((int)X, (int)Y, Width, Height); }}
    }
}

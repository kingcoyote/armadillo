using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public abstract class Object : DrawableGameComponent
    {
        protected Object(Microsoft.Xna.Framework.Game game) : base(game) { }

        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public int Z = 1;
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        
        public virtual Rectangle Rectangle { get { return new Rectangle(X, Y, Width, Height); }}
    }
}

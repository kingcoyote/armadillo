using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public abstract class Object
    {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public int Z = 1;
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        
        public virtual Rectangle Rectangle { get { return new Rectangle(X, Y, Width, Height); }}

        public EventHandler<MouseEventArgs> MouseClick = (sender, args) => { };
        public EventHandler<MouseEventArgs> MouseRelease = (sender, args) => { };
        public EventHandler<MouseEventArgs> MouseOver = (sender, args) => { };
        public EventHandler<MouseEventArgs> MouseOut = (sender, args) => { };

        public virtual void Update(GameTime gameTime) { }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;

namespace Torch
{
    public abstract class Layer : DrawableGameComponent
    {
        public int ZIndex;
        public float X;
        public float Y;
        public Scene Scene { get; private set; }
        protected readonly Dictionary<string, Object> Objects = new Dictionary<string, Object>();
        protected int _oldMouseX = -1;
        protected int _oldMouseY = -1;
        public bool Visible = true;

        protected Layer(Scene scene) : base(scene.Game)
        {
            Scene = scene;
        }

        // draw
        public override void Draw(GameTime gametime)
        {
            var spriteBatch = new SpriteBatch(((GraphicsDeviceManager)Scene.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice);
            spriteBatch.Begin();
            foreach(var obj in (from obj in Objects.Values orderby obj.Z ascending select obj))
            {
                obj.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        // update
        public override void Update(GameTime gameTime)
        {

        }

        protected void ClearByName(string name)
        {
            var oldKeys =
                (from key in Objects.Keys where key.Length > name.Length && key.Substring(0, name.Length) == name select key);

            foreach (var key in oldKeys.ToList())
            {
                Objects.Remove(key);
            }
        }
    }
}

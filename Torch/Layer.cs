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

        public event EventHandler<MouseEventArgs> MouseClick = (sender, args) => { };
        public event EventHandler<MouseEventArgs> MouseRelease = (sender, args) => { };
        public event EventHandler<MouseEventArgs> MouseOver = (sender, args) => { };
        public event EventHandler<MouseEventArgs> MouseOut = (sender, args) => { };
        public event EventHandler<KeyboardEventArgs> KeyDown = (sender, args) => { };
        public event EventHandler<KeyboardEventArgs> KeyUp = (sender, args) => { };

        protected Layer(Scene scene) : base(scene.Game)
        {
            Scene = scene;

            MouseClick += HandleMouseClick;
            MouseRelease += HandleMouseRelease;
        }

        // draw
        public override void Draw(GameTime gametime)
        {
            var spriteBatch = new SpriteBatch((GraphicsDevice)Scene.Game.Services.GetService(typeof(IGraphicsDeviceService)));
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
            var mouse = ((IInputService)Game.Services.GetService(typeof (IInputService))).GetMouse().GetState();
            var cursor = new Rectangle(mouse.X, mouse.Y, 1, 1);

            foreach (var obj in Objects.Values.ToList())
            {
                obj.Update(gameTime);

                var rect = obj.Rectangle;
                rect.X += (int)X;
                rect.Y += (int)Y;

                // check for mouse over
                if (rect.Contains(cursor) && !rect.Contains(_oldMouseX, _oldMouseY))
                {
                    obj.MouseOver.Invoke(obj, new MouseEventArgs { X = cursor.X, Y = cursor.Y, Target = obj });
                }

                // check for mouseout
                if (!rect.Contains(cursor) && rect.Contains(_oldMouseX, _oldMouseY))
                {
                    obj.MouseOut.Invoke(obj, new MouseEventArgs { X = cursor.X, Y = cursor.Y, Target = obj });
                }
            }

            _oldMouseX = cursor.X;
            _oldMouseY = cursor.Y;
        }

        public void InvokeMouseClick(Scene scene, MouseEventArgs mouseEventArgs)
        {
            MouseClick.Invoke(scene, mouseEventArgs);
        }

        public void InvokeMouseRelease(Scene scene, MouseEventArgs mouseEventArgs)
        {
            MouseRelease.Invoke(scene, mouseEventArgs);
        }

        public void InvokeKeyDown(Scene scene, KeyboardEventArgs keyboardEventArgs)
        {
            KeyDown.Invoke(scene, keyboardEventArgs);
        }

        public void InvokeKeyUp(Scene scene, KeyboardEventArgs keyboardEventArgs)
        {
            KeyUp.Invoke(scene, keyboardEventArgs);
        }

        public void InvokeMouseOver(Scene scene, MouseEventArgs mouseEventArgs)
        {
            MouseOver.Invoke(scene, mouseEventArgs);
        }

        public void InvokeMouseOut(Scene scene, MouseEventArgs mouseEventArgs)
        {
            MouseOut.Invoke(scene, mouseEventArgs);
        }

        public void HandleMouseClick(object sender, MouseEventArgs args)
        {
            foreach (var obj in Objects.Values.ToList())
            {
                if (obj.Rectangle.Contains(args.X - (int)X, args.Y - (int)Y))
                {
                    args.Target = obj;
                    obj.MouseClick.Invoke(obj, args);
                }
            }
        }

        public void HandleMouseRelease(object sender, MouseEventArgs args)
        {
            foreach (var obj in Objects.Values.ToList())
            {
                if (obj.Rectangle.Contains(args.X - (int)X, args.Y - (int)Y))
                {
                    args.Target = obj;
                    obj.MouseRelease.Invoke(obj, args);
                }
            }
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

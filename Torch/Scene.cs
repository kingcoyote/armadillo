using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// the scene needs access to the game

namespace Torch
{
    public abstract class Scene
    {
        protected readonly Dictionary<string, Layer> Layers = new Dictionary<string, Layer>();
        public Game Game { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsRunning { get; private set; }

        // new
        protected Scene(Game game)
        {
            Game = game;
            IsInitialized = false;
            IsRunning = true;
        }
        
        // initialize
        public virtual void Initialize()
        {
            IsInitialized = true;
        }
        // shutdown
        public virtual void ShutDown()
        {
            IsInitialized = false;
        }
        
        // pause
        public virtual void Start()
        {
            IsRunning = true;
        }
        // unpause
        public virtual void Stop()
        {
            IsRunning = false;
        }

        // draw
        public void Draw()
        {
            var spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            foreach(var layer in from layer in Layers.Values where layer.Visible orderby layer.ZIndex select layer)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Immediate, 
                    BlendState.AlphaBlend, 
                    SamplerState.PointWrap, 
                    DepthStencilState.Default, 
                    RasterizerState.CullNone, 
                    null,
                    Matrix.CreateTranslation(layer.X, layer.Y, 0)
                );
                layer.Draw(spriteBatch);
                spriteBatch.End();
            }
        }
        // update
        public virtual void Update(GameTime gameTime, Input input)
        {
            foreach(var layer in Layers.Values.ToArray())
            {
                layer.Update(gameTime, input);
            }
        }

        public void UpdateEvents(List<InputEventArgs> events)
        {
            foreach(InputEventArgs e in events)
            {
                e.Handled = false;

                foreach(var layer in from layer in Layers.Values.ToList() where layer.Visible select layer)
                {
                    if (e.Handled) continue;

                    if (e is MouseEventArgs)
                    {
                        if (((MouseEventArgs)e).Press)
                        {
                            layer.InvokeMouseClick(this, (MouseEventArgs) e);
                        }
                        else
                        {
                            layer.InvokeMouseRelease(this, (MouseEventArgs)e);
                        }
                    }
                    else if (e is KeyboardEventArgs)
                    {
                        if(((KeyboardEventArgs)e).Press)
                        {
                            layer.InvokeKeyDown(this, (KeyboardEventArgs) e);
                        }
                        else
                        {
                            layer.InvokeKeyUp(this, (KeyboardEventArgs) e);
                        }
                    }
                }
            }
        }
    }
}

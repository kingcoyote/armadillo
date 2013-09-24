using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using Nuclex.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

// the scene needs access to the game

namespace Torch
{
    public abstract class Scene : DrawableGameState
    {
        protected readonly GameComponentCollection Components = new GameComponentCollection();
        protected GuiManager Gui;
        public Game Game { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsRunning { get; private set; }

        // new
        protected Scene(Game game)
        {
            Game = game;
            IsInitialized = false;
            IsRunning = true;

            Gui = new GuiManager(
                (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager)), 
                (IInputService)Game.Services.GetService(typeof(IInputService)))
            {
                Screen = new Screen(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height)
            };

            Gui.Screen.Desktop.Bounds = new UniRectangle(
              new UniScalar(0.01F, 0.0F), new UniScalar(0.01F, 0.0F),
              new UniScalar(0.98F, 0.0F), new UniScalar(0.98F, 0.0F)
            );

            Gui.UpdateOrder = 1000;

            Components.Add(Gui);
        }
        
        // initialize
        public virtual void Initialize()
        {
            IsInitialized = true;

            Gui.Initialize();
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
        public override void Update(GameTime gametime)
        {
            foreach (var component in (from IUpdateable c in Components orderby c.UpdateOrder select c).ToArray())
            {
                component.Update(gametime);
            }
        }
        // update
        public override void Draw(GameTime gametime)
        {
            foreach (var component in (from IDrawable c in Components orderby c.DrawOrder select c).ToArray())
            {
                component.Draw(gametime);
            }
        }
    }
}

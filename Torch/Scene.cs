using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using Nuclex.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Input;

namespace Torch
{
    public abstract class Scene : DrawableGameState
    {
        protected readonly GameComponentCollection Components = new GameComponentCollection();
        protected GuiManager Gui;
        public Game Game { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsRunning { get; private set; }

        private IInputCapturer _capturer;
        private SpriteBatch _spriteBatch;

        // new
        protected Scene(Game game)
        {
            Game = game;

            _spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));

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

            Gui.DrawOrder = 100;
        }
        
        // initialize
        protected override void OnEntered()
        {
            IsInitialized = true;

            Gui.Initialize();
        }
        // shutdown
        protected override void OnLeaving()
        {
            IsInitialized = false;
        }
        
        // pause
        protected override void OnPause()
        {
            IsRunning = true;

            Gui.Visible = false;
            _capturer = Gui.InputCapturer;
            Gui.InputCapturer = null;
        }

        // unpause
        protected override void OnResume()
        {
            IsRunning = false;

            Gui.Visible = true;
            Gui.InputCapturer = _capturer;
            _capturer = null;
        }

        // update
        public override void Update(GameTime gametime)
        {
            foreach (var component in (from IUpdateable c in Components orderby c.UpdateOrder select c).ToArray())
            {
                component.Update(gametime);
            }
        }

        // draw
        public override void Draw(GameTime gametime)
        {
            _spriteBatch.Begin();
            foreach (var component in (from IDrawable c in Components orderby c.DrawOrder select c).ToArray())
            {
                component.Draw(gametime);
            }
            _spriteBatch.End();
            Gui.Draw(gametime);
        }
    }
}

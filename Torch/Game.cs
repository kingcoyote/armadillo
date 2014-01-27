using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using Nuclex.Input;

namespace Torch
{
    abstract public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;

        protected GameStateManager GameStates;
        protected string CurrentScene;
        protected string SettingsFile;

        protected bool IsFullScreen = true;
        protected int ScreenWidth = 1280;
        protected int ScreenHeight = 960;

        public AppSettings Settings;

        protected InputManager Input;

        private SpriteBatch _spriteBatch;

        protected Game()
        {
            
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);
        }

        protected void InitializeGraphics()
        {
            _graphics.PreferMultiSampling = false;

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.IsFullScreen = IsFullScreen;

            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // load settings file
            LoadSettings();

            InitializeGraphics();

            GameStates = new GameStateManager(Services);
            Input = new InputManager(Services);
            Services.AddService(typeof(ContentManager), Content);

            GameStates.Initialize();

            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            SaveSettings();
        }

        public virtual void SaveSettings()
        {
            SettingsManager.SaveSettings();
        }

        public virtual void LoadSettings()
        {
            SettingsManager.LoadSettings();
            Settings = SettingsManager.Settings;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // initialize current scene
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), _spriteBatch);
            Services.AddService(typeof(GraphicsDeviceManager), _graphics);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            // update current scene
            GameStates.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gametime)
        {
            GraphicsDevice.Clear(Color.Black);

            // draw current scene
            GameStates.Draw(gametime);
        }

        public void PushScene(Scene scene)
        {
            if (GameStates.ActiveState != null)
            {
                GameStates.ActiveState.Pause();
            }
            GameStates.Push(scene);
        }

        public void PopScene()
        {
            GameStates.Pop();
            if (GameStates.ActiveState != null)
            {
                GameStates.ActiveState.Resume();
            } else
            {
                Exit();
            }
        }

        public virtual void ChangeResolution(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            InitializeGraphics();
        }

        public virtual void SetFullScreen(bool enabled)
        {
            IsFullScreen = enabled;
            InitializeGraphics();
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    abstract public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Input _input;

        protected Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        protected string CurrentScene;
        protected string SettingsFile;

        protected bool IsFullScreen = true;
        protected int ScreenWidth = 1280;
        protected int ScreenHeight = 960;

        public AppSettings Settings;

        protected static Game _instance;

        protected Game()
        {
            
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);

            _instance = this;
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

            _input = new Input();

            InitializeGraphics();

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
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // initialize current scene
            Scenes[CurrentScene].Initialize();

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
            // update input
            _input.Update(gameTime);

            if(_input.Events.Count > 0)
            {
                Scenes[CurrentScene].UpdateEvents(_input.Events);
            }
            // update current scene
            Scenes[CurrentScene].Update(gameTime, _input);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // draw current scene
            Scenes[CurrentScene].Draw();

            base.Draw(gameTime);
        }

        public void ChangeScenes(string scene)
        {
            Scenes[CurrentScene].Stop();

            if(Scenes[scene].IsInitialized == false)
            {
                Scenes[scene].Initialize();
            }

            Scenes[scene].Start();

            CurrentScene = scene;
        }

        public void ChangeResolution(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            InitializeGraphics();
        }

        public void SetFullScreen(bool enabled)
        {
            IsFullScreen = enabled;
            InitializeGraphics();
        }

        public static Game GetInstance()
        {
            return _instance;
        }
    }
}

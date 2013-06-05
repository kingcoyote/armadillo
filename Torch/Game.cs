using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    abstract public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Input _input;

        protected Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        protected string CurrentScene;
        protected string SettingsFile;

        abstract protected int ScreenWidth { get; set; }
        abstract protected int ScreenHeight { get; set; }

        public AppSettings Settings;

        private static GraphicsDevice _graphicsDevice ;
        protected static Game _instance;

        protected Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphicsDevice = GraphicsDevice;

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            _instance = this;
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

        public static Game GetInstance()
        {
            return _instance;
        }
    }
}

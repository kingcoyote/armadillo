using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SRPG.Data;
using SRPG.Scene.Battle;
using SRPG.Scene.Intro;
using SRPG.Scene.Menu;
using SRPG.Scene.Options;
using SRPG.Scene.Overworld;
using SRPG.Scene.Shop;
using Torch;

namespace SRPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SRPGGame : Torch.Game
    {
        private int _screenWidth = 1280;
        private int _screenHeight = 1024;
        protected override int ScreenWidth { get { return _screenWidth; } set { _screenWidth = value; } }
        protected override int ScreenHeight { get { return _screenHeight; } set { _screenHeight = value; } }

        public List<Character> Party;
        public List<Item> Inventory;
        public int Money;

        public const int TileSize = 64;

        public SRPGGame()
        {
            Scenes.Add("intro", new IntroScene(this));
            Scenes.Add("menu", new MenuScene(this));
            Scenes.Add("overworld", new OverworldScene(this));
            Scenes.Add("battle", new BattleScene(this));
            Scenes.Add("shop", new ShopScene(this));
            Scenes.Add("options", new OptionsScene(this));

            CurrentScene = "overworld";
        }

        protected override void Initialize()
        {
            base.Initialize();

            ((OverworldScene) Scenes["overworld"]).SetZone(Zone.Factory("kakariko"));
        }
    }
}

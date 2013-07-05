using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Options
{
    class Menu : Layer
    {
        public Menu(Torch.Scene scene) : base(scene)
        {

            var font = FontManager.Get("Menu");

            Objects.Add("return", new TextObject() { Font = font, Color = Color.White, Value = "Return to Game", X = 50, Y = 50 });
            Objects["return"].MouseClick += (sender, args) => Game.GetInstance().ChangeScenes("overworld");
            
            Objects.Add("menu", new TextObject() { Font = font, Color = Color.White, Value = "Exit to Main Menu", X = 50, Y = (int)(50 + font.LineSpacing * 1.5) });
            Objects["menu"].MouseClick += (sender, args) => Game.GetInstance().ChangeScenes("main menu");
            
            Objects.Add("exit", new TextObject() { Font = font, Color = Color.White, Value = "Exit to Desktop", X = 50, Y = 50 + font.LineSpacing * 3 });
            Objects["exit"].MouseClick += (sender, args) => Game.GetInstance().Exit();
        }
    }
}

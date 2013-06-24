using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.MainMenu
{
    class MenuOptions : Layer
    {
        public MenuOptions(Torch.Scene scene) : base(scene)
        {
            var font = Game.GetInstance().Content.Load<SpriteFont>("Menu");

            Objects.Add("New Game", new TextObject { Font = font, Color = Color.White, Value = "New Game", X = 50, Y = 50 });
            Objects["New Game"].MouseClick = (sender, args) => ((SRPGGame) Scene.Game).StartGame();

            Objects.Add("Continue", new TextObject { Font = font, Color = Color.White, Value = "Continue", X = 50, Y = 100 });


            Objects.Add("Load Game", new TextObject { Font = font, Color = Color.White, Value = "Load Game", X = 50, Y = 150 });


            Objects.Add("Options", new TextObject { Font = font, Color = Color.White, Value = "Options", X = 50, Y = 200 });


            Objects.Add("Exit", new TextObject { Font = font, Color = Color.White, Value = "Exit", X = 50, Y = 250 });
            Objects["Exit"].MouseClick = (sender, args) => Game.GetInstance().Exit();
        }
    }
}

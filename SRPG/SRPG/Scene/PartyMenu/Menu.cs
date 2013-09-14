using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.PartyMenu
{
    class Menu : Layer
    {

        public Menu(Torch.Scene scene) : base(scene)
        {
            var cm = (ContentManager)Game.Services.GetService(typeof(ContentManager));

            var font = FontManager.Get("Menu");
            Components.Add(new ImageObject(Game, "PartyMenu/cursor"));

            var screenWidth = Game.GraphicsDevice.Viewport.Width;

            Components.Add(new TextObject(Game) { Color = Color.White, Font = font, Value = "Status", X = screenWidth / 5, Y = 50, Alignment = TextObject.AlignTypes.Center });
            Components.Add(new TextObject(Game) { Color = Color.White, Font = font, Value = "Inventory", X = screenWidth / 2, Y = 50, Alignment = TextObject.AlignTypes.Center });
            Components.Add(new TextObject(Game) { Color = Color.White, Font = font, Value = "Settings", X = screenWidth / 5 * 4, Y = 50, Alignment = TextObject.AlignTypes.Center });

            //Objects["status"].MouseClick += ChangeMenu("status menu");
            //Objects["inventory"].MouseClick += ChangeMenu("inventory menu");
            //Objects["settings"].MouseClick += ChangeMenu("settings menu");

            Components.Add(new TextureObject(Game) { Color = Color.Blue, Z = -1, Width = Game.GraphicsDevice.Viewport.Width, Height = Game.GraphicsDevice.Viewport.Height });
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            var cursor = ((IInputService) Game.Services.GetService(typeof (IInputService))).GetMouse().GetState();
        }

        private bool MouseInWindow(int x, int y)
        {
            return x > 0 && 
                x < Game.Window.ClientBounds.Width && 
                y > 0 && 
                y < Game.Window.ClientBounds.Height;
        }

        //private EventHandler<MouseEventArgs> ChangeMenu(string menu)
        //{
        //    return (sender, args) => ((PartyMenuScene) Scene).ChangeMenu(menu);
        //}
    }
}

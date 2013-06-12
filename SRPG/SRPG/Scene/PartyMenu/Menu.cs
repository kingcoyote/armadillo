using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.PartyMenu
{
    class Menu : Layer
    {

        public Menu(Torch.Scene scene) : base(scene)
        {
            ContentManager cm = Game.GetInstance().Content;

            var font = cm.Load<SpriteFont>("Menu");
            Objects.Add("cursor", new ImageObject(cm.Load<Texture2D>("PartyMenu/cursor")));
            Objects.Add("status", new TextObject() { Color = Color.White, Font = font, Value = "Status", X = 50, Y = 50 });
            Objects.Add("inventory", new TextObject() { Color = Color.White, Font = font, Value = "Inventory", X = 50, Y = 100 });
            Objects.Add("settings", new TextObject() { Color = Color.White, Font = font, Value = "Settings", X = 50, Y = 150 });

            Objects["status"].MouseClick += ChangeMenu("status menu");
            Objects["inventory"].MouseClick += ChangeMenu("inventory menu");
            Objects["settings"].MouseClick += ChangeMenu("settings menu");
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            if (MouseInWindow(input))
            {
                Objects["cursor"].X = input.Cursor.X;
                Objects["cursor"].Y = input.Cursor.Y;
            }
            else
            {
                Objects["cursor"].X = -100;
                Objects["cursor"].Y = -100;
            }
        }

        private static bool MouseInWindow(Input input)
        {
            return input.Cursor.X > 0 && 
                input.Cursor.X < Game.GetInstance().Window.ClientBounds.Width && 
                input.Cursor.Y > 0 && 
                input.Cursor.Y < Game.GetInstance().Window.ClientBounds.Height;
        }

        private EventHandler<MouseEventArgs> ChangeMenu(string menu)
        {
            return (sender, args) => ((PartyMenuScene) Scene).ChangeMenu(menu);
        }
    }
}

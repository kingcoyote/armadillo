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

            var screenWidth = Game.GetInstance().GraphicsDevice.Viewport.Width;

            Objects.Add("status", new TextObject { Color = Color.White, Font = font, Value = "Status", X = screenWidth / 5, Y = 50, Alignment = TextObject.AlignTypes.Center });
            Objects.Add("inventory", new TextObject { Color = Color.White, Font = font, Value = "Inventory", X = screenWidth / 2, Y = 50, Alignment = TextObject.AlignTypes.Center });
            Objects.Add("settings", new TextObject { Color = Color.White, Font = font, Value = "Settings", X = screenWidth / 5 * 4, Y = 50, Alignment = TextObject.AlignTypes.Center });

            Objects["status"].MouseClick += ChangeMenu("status menu");
            Objects["inventory"].MouseClick += ChangeMenu("inventory menu");
            Objects["settings"].MouseClick += ChangeMenu("settings menu");

            Objects.Add("background", new TextureObject { Color = Color.Blue, Z = -1, Width = Game.GetInstance().GraphicsDevice.Viewport.Width, Height = Game.GetInstance().GraphicsDevice.Viewport.Height });
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

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
            Objects.Add("party", new TextObject() { Color = Color.White, Font = font, Value = "Party", X = 50, Y = 50 });
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
    }
}

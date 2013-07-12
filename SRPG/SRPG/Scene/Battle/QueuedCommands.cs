using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Torch;
using Game = Torch.Game;

namespace SRPG.Scene.Battle
{
    class QueuedCommands : Layer
    {
        public QueuedCommands(Torch.Scene scene) : base(scene)
        {
            
            Objects.Add("box", new TextureObject { Color = Color.Blue, X = 25, Y = 0, Width = 250, Height = 0 });
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            var commands = ((BattleScene)Scene).QueuedCommands;
            var font = FontManager.Get("Menu");
            var viewport = Game.GetInstance().GraphicsDevice.Viewport;

            Objects["box"].Height = (int)((commands.Count + 1)*font.LineSpacing*1.5);
            Objects["box"].Y = viewport.Height - 25 - Objects["box"].Height;

            ClearByName("command");

            var i = 0;

            foreach(var command in commands)
            {
                //Objects.Add("command/" + i + "/portrait", command.Character.Portrait);
                //Objects["command/" + i + "/portrait"].X = 35;
                //Objects["command/" + i + "/portrait"].Y = Objects["box"].Y + 10 + (int) (i*font.LineSpacing*1.5);

                Objects.Add("command/" + i + "/name", new TextObject
                    {
                        Font = font, 
                        Color = Color.White, 
                        X = 70, 
                        Y = Objects["box"].Y + 10 + (int)(i * font.LineSpacing * 1.5), 
                        Value = command.Ability.Name
                    });

                i++;
            }
        }
    }
}

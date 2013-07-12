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

            Objects["box"].Height = (commands.Count + 1)*font.LineSpacing*2;
            Objects["box"].Y = viewport.Height - 25 - Objects["box"].Height;
        }
    }
}

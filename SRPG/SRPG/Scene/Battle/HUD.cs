using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.Battle
{
    class HUD : Layer
    {
        public HUD(Torch.Scene scene) : base(scene)
        {
            var font = FontManager.Get("Menu");

            // background box
            Objects.Add("box", new TextureObject { X = 25, Y = 25, Width = 300, Height = 125, Color = Color.Blue });

            // round number
            Objects.Add("round label", new TextObject { Font = font, X = 35, Y = 35, Value = "Round", Color = Color.White });
            Objects.Add("round number", new TextObject { Font = font, X = 315, Y = 35, Alignment = TextObject.AlignTypes.Right, Color = Color.White, Value = "1" });

            // faction
            Objects.Add("faction label", new TextObject { Font = font, X = 35, Y = (int)(35 + font.LineSpacing * 1.5), Color = Color.White, Value = ""});

            Objects.Add("end turn", new TextObject { Font = font, X = 315, Y = (int)(35 + font.LineSpacing * 1.5), Alignment = TextObject.AlignTypes.Right, Color = Color.White, Value = "End" });

            Objects["end turn"].MouseRelease += EndPlayerTurn;

            KeyDown += CheckCancel;
        }

        private void EndPlayerTurn(object sender, MouseEventArgs e)
        {
            ((BattleScene) Scene).EndPlayerTurn();
        }

        public override void Update(GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            ((TextObject)Objects["round number"]).Value = ((BattleScene) Scene).RoundNumber.ToString();
            ((TextObject)Objects["faction label"]).Value = ((BattleScene) Scene).FactionTurn == 0 ? "Player" : "Enemy Turn";

            ((TextObject)Objects["end turn"]).Value = ((BattleScene)Scene).FactionTurn == 0 ? "End Turn" : "";
        }

        private void CheckCancel(object sender, KeyboardEventArgs args)
        {
            if (args.WhichKey != Keys.Escape) return;

            ((BattleScene) sender).Cancel();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.Input;
using Torch;

namespace SRPG
{
    class RadialMenu : Layer
    {
        public int Distance = 75;
        public int CenterX = 0;
        public int CenterY = 0;
        public int ExitDistance = 125;

        public Action OnExit;

        public RadialMenu(Torch.Scene scene) : base(scene) { }

        public void AddOption(string name, SpriteObject icon)
        {
            Objects.Add(name, icon);
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            var degs = Math.PI * 2 /Objects.Count;

            for(var i = 0; i < Objects.Count; i++)
            {
                var key = Objects.Keys.ElementAt(i);

                Objects[key].X = (int)(CenterX + Math.Sin(degs * (i)) * Distance) - Objects[key].Width / 2;
                Objects[key].Y = (int)(CenterY - Math.Cos(degs * (i)) * Distance) - Objects[key].Height / 2;
            }
        }

        public void ClearOptions()
        {
            Objects.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var cursor = new Rectangle
                {
                    X = ((IInputService) Game.Services.GetService(typeof (IInputService))).GetMouse().GetState().X,
                    Y = ((IInputService) Game.Services.GetService(typeof (IInputService))).GetMouse().GetState().Y
                };

            var xDistance = cursor.X - (CenterX + X);
            var yDistance = cursor.Y - (CenterY + Y);

            if(Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2)) > ExitDistance)
            {
                OnExit.Invoke();
            }
        }
    }
}

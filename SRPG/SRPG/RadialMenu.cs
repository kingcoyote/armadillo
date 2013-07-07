using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Torch;

namespace SRPG
{
    class RadialMenu : Layer
    {
        public int Distance = 75;

        public int CenterX = 0;
        public int CenterY = 0;

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

                Objects[key].X = (int)(CenterX + Math.Sin(degs * (i - 1)) * Distance) - Objects[key].Width / 2;
                Objects[key].Y = (int)(CenterY - Math.Cos(degs * (i - 1)) * Distance) - Objects[key].Height / 2;
            }
        }
    }
}

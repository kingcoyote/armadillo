using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.Input;
using Nuclex.Input.Devices;
using Nuclex.UserInterface.Controls;
using Torch;

namespace SRPG
{
    class RadialMenu : Control
    {
        public int Distance = 75;
        public int CenterX = 0;
        public int CenterY = 0;
        public int ExitDistance = 125;

        public Action OnExit;

        private IMouse _mouse;

        public RadialMenu(IMouse mouse)
        {
            _mouse = mouse;
        }

        public void AddOption(string name, ImageButtonControl button)
        {
            Children.Add(button);
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            var degs = Math.PI * 2 / Children.Count;

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Bounds.Location.X.Offset = (int)(CenterX + Math.Sin(degs * (i)) * Distance) - Children[i].Bounds.Size.X.Offset / 2;
                Children[i].Bounds.Location.Y.Offset = (int)(CenterY - Math.Cos(degs * (i)) * Distance) - Children[i].Bounds.Size.Y.Offset / 2;
            }
        }

        public void ClearOptions()
        {
            Children.Clear();
        }

        public void Render(GameTime gameTime)
        {
            // if the cursor strays too far from the center, close the radial menu
            var cursor = new Rectangle
                {
                    X = _mouse.GetState().X,
                    Y = _mouse.GetState().Y
                };

            var xDistance = cursor.X - (CenterX);
            var yDistance = cursor.Y - (CenterY);

            if(Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2)) > ExitDistance)
            {
                OnExit.Invoke();
            }
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Nuclex.Input.Devices;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace Torch.UserInterface
{
    public class RadialMenuControl : Control
    {
        public int Distance = 75;
        public int CenterX = 0;
        public int CenterY = 0;
        public int ExitDistance = 125;

        public Action OnExit;

        private readonly IMouse _mouse;

        public RadialMenuControl(IMouse mouse)
        {
            _mouse = mouse;
            Bounds = new UniRectangle(0, 0, (UniScalar)(ExitDistance * 2), (UniScalar)(ExitDistance * 2));
        }

        public void AddOption(string name, ImageButtonControl button)
        {
            Children.Add(button);
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            var degs = Math.PI * 2 / Children.Count;

            Bounds.Location.X.Offset = CenterX - ExitDistance;
            Bounds.Location.Y.Offset = CenterY - ExitDistance;

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Bounds.Location.X.Offset = (int)(ExitDistance + Math.Sin(degs * (i)) * Distance) - Children[i].Bounds.Size.X.Offset / 2;
                Children[i].Bounds.Location.Y.Offset = (int)(ExitDistance - Math.Cos(degs * (i)) * Distance) - Children[i].Bounds.Size.Y.Offset / 2;
            }
        }

        public void ClearOptions()
        {
            Children.Clear();
        }

        public void Update(GameTime gameTime)
        {
            // if the cursor strays too far from the center, close the radial menu
            var cursor = new Rectangle
            {
                X = _mouse.GetState().X,
                Y = _mouse.GetState().Y
            };

            var xDistance = cursor.X - (CenterX);
            var yDistance = cursor.Y - (CenterY);

            if (Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2)) > ExitDistance)
            {
                OnExit.Invoke();
            }
        }
    }
}

﻿using System;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace Torch.UserInterface
{
    public class FlatRadialButtonControlRenderer : IFlatControlRenderer<RadialButtonControl>
    {
        public void Render(RadialButtonControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            // Determine the style to use for the button
            int stateIndex = 0;
            if (control.Enabled)
            {
                if (control.Depressed)
                {
                    stateIndex = 3;
                }
                else if (control.MouseHovering)
                {
                    stateIndex = 2;
                }
                else
                {
                    stateIndex = 1;
                }
            }

            // Draw the button's frame
            graphics.DrawElement("radialbuttoncontrol" + states[stateIndex], controlBounds);
            graphics.DrawElement("radialcontrol.images." + control.ImageFrame + states[stateIndex], controlBounds);
        }

        /// <summary>Names of the states the button control can be in</summary>
        /// <remarks>
        ///   Storing this as full strings instead of building them dynamically prevents
        ///   any garbage from forming during rendering.
        /// </remarks>
        private static readonly string[] states = new string[] {
          ".disabled",
          ".normal",
          ".highlighted",
          ".depressed"
        };
    }
}

using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;
using Nuclex.UserInterface.Controls.Desktop;

namespace FusionC
{
    public class FlatImageButtonControlRenderer : IFlatControlRenderer<ImageButtonControl>
    {
        public void Render(ImageButtonControl control, IFlatGuiGraphics graphics)
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
                else if (control.MouseHovering || control.HasFocus)
                {
                    stateIndex = 2;
                }
                else
                {
                    stateIndex = 1;
                }
            }

            // Draw the button's frame
            graphics.DrawElement("imagebutton" + states[stateIndex], controlBounds);
            graphics.DrawElement(control.ImageFrame + states[stateIndex], controlBounds);
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

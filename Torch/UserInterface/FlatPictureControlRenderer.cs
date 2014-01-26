using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace Torch.UserInterface
{
    public class FlatPictureControlRenderer : IFlatControlRenderer<PictureControl>
    {
        public void Render(PictureControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            // Draw the button's frame
            graphics.DrawElement(control.Frame, controlBounds);
        }
    }
}

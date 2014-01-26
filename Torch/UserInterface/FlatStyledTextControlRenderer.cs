using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace Torch.UserInterface
{
    public class FlatStyledTextControlRenderer : IFlatControlRenderer<StyledTextControl>
    {
        public void Render(StyledTextControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            // Draw the button's frame
            graphics.DrawString(control.Font, controlBounds, control.Text);
        }
    }
}

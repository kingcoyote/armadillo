using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace SRPG.Scene.Battle
{
    public class FlatQueuedCommandControlRenderer : IFlatControlRenderer<QueuedCommandControl>
    {
        public void Render(QueuedCommandControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            // Draw the button's frame
            graphics.DrawElement("imagebutton.normal", controlBounds);
            graphics.DrawString("imagebutton.normal", controlBounds, control.Command.Ability.Name);
        }
    }
}

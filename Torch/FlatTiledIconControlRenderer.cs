using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace FusionC
{
    public class FlatTiledIconControlRenderer : IFlatControlRenderer<TiledIconControl>
    {
        public void Render(TiledIconControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            for (var i = 0; i < control.Height; i++)
            {
                for (var j = 0; j < control.Width; j++)
                {
                    var tileWidth = controlBounds.Width/control.Width;
                    var tileHeight = controlBounds.Height/control.Height;
                    var tileX = controlBounds.X + tileWidth*j;
                    var tileY = controlBounds.Y + tileHeight*i;

                    var tileBounds = new RectangleF(tileX, tileY, tileWidth, tileHeight);

                    // Draw the button's frame
                    graphics.DrawElement(control.ImageFrame, tileBounds);

                    if(i * control.Width + j >= control.Count - 1)
                    {
                        return;
                    }
                }
            }
        }
    }
}

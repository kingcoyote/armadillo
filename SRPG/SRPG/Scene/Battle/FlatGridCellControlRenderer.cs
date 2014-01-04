using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace SRPG.Scene.Battle
{
    public class FlatGridCellControlRenderer : IFlatControlRenderer<GridCellControl>
    {
        public void Render(GridCellControl control, IFlatGuiGraphics graphics)
        {
            graphics.DrawElement(_highlights[control.Highlight], control.GetAbsoluteBounds());
        }

        private readonly Dictionary<GridHighlight, string> _highlights = new Dictionary<GridHighlight, string>()
            {
                {GridHighlight.Normal, "gridcell.normal"},
                {GridHighlight.Selectable, "gridcell.selectable"},
                {GridHighlight.Targetted, "gridcell.targetted"},
                {GridHighlight.Splashed, "gridcell.splashed"}
            };
    }
}

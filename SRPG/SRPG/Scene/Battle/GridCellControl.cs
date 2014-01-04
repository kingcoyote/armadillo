using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using Torch.UserInterface;

namespace SRPG.Scene.Battle
{
    public class GridCellControl : PressableControl
    {
        public GridHighlight Highlight;
        public Action GridClicked = () => { };

        protected override void OnPressed()
        {
            base.OnPressed();
            GridClicked.Invoke();
        }
    }
}

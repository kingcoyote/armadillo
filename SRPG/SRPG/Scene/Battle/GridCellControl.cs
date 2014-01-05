using System;
using Nuclex.UserInterface.Controls;

namespace SRPG.Scene.Battle
{
    public class GridCellControl : PressableControl
    {
        public GridHighlight Highlight;
        public Action GridClicked = () => { };

        protected override void OnPressed()
        {
            base.OnPressed();

            if (Highlight != GridHighlight.Targetted) return; 

            GridClicked.Invoke();
        }
    }
}

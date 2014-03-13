using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Shop
{
    class InventoryListControl : ListControl
    {
        public delegate void HoverChangeDelegate(int i);

        public delegate void HoverClearedDelegate();

        public HoverChangeDelegate HoverChange;
        public HoverClearedDelegate HoverCleared;

        private int _hover = -1;

        protected override void OnMouseMoved(float x, float y)
        {
            var row = ListRowLocator.GetRow(
                GetAbsoluteBounds(),
                Slider.ThumbPosition,
                Items.Count,
                y
            );

            if (row < 0 || row >= Items.Count)
            {
                if (_hover != -1)
                {
                    _hover = -1;
                    HoverCleared.Invoke();
                }
                return;
            }

            var newHover = row;

            if (_hover != newHover)
            {
                _hover = newHover;
                HoverChange.Invoke(row);
            }

            base.OnMouseMoved(x, y);
        }

        protected override void OnMouseLeft()
        {
            base.OnMouseLeft();
            _hover = -1;
            HoverCleared.Invoke();
        }
    }
}

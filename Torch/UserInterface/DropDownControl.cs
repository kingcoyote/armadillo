using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Torch.UserInterface
{
    public class DropDownControl : WindowControl
    {
        public delegate void SelectDelegate(string item);

        public SelectDelegate ItemSelected = i => { };

        private int _items;

        public void AddItem(string item)
        {
            var button = new ButtonControl
                {
                    Bounds = new UniRectangle(
                        new UniScalar(0), new UniScalar(_items*30),
                        new UniScalar(1, 0), new UniScalar(30)
                        ),
                    Text = item
                };
            button.Pressed += (s, a) => ItemSelected.Invoke(item);
            Children.Add(button);
            _items++;
        }
    }
}

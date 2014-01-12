using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Torch.UserInterface
{
    public class WindowControl : Nuclex.UserInterface.Controls.Desktop.WindowControl
    {
        private bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set {
                _visible = value;
                if (_visible == true) { 
                    Show();
                } else {
                    Hide();
                }
            }
        }

        public void Hide()
        {
            if (Bounds.Location.X.Offset < -50000) return;

            Bounds.Location.X.Offset -= 50000;
        }

        public void Show()
        {
            if (Bounds.Location.X.Offset > -40000) return;

            Bounds.Location.X.Offset += 50000;
        }
    }
}

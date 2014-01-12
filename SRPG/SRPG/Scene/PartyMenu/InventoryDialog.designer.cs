using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch.UserInterface;
using Nuclex.UserInterface;

namespace SRPG.Scene.PartyMenu
{
    public partial class InventoryDialog : WindowControl
    {
        private void InitializeComponent()
        {
            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(0.0f, 105.0f),
                new UniScalar(0.2f, 0.0f), new UniScalar(1.0f, -105.0f)
            );
            EnableDragging = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torch;

namespace SRPG.Scene.Overworld
{
    class HUD : Layer
    {
        private readonly ImageObject _interactIcon;
        
        public HUD(Torch.Scene scene, Torch.Object parent) : base(scene, parent)
        {
            _interactIcon = new ImageObject(Game, this, "HUD/interacticon");
        }

        public void ShowInteractIcon(int x, int y)
        {
            _interactIcon.X = x - (_interactIcon.Width/2);
            _interactIcon.Y = y - (_interactIcon.Height/2);
            Components.Add(_interactIcon);
        }

        public void HideInteractIcon()
        {
            Components.Remove(_interactIcon);
        }
    }
}

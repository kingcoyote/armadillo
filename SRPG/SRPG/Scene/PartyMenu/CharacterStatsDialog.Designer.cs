using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;

namespace SRPG.Scene.PartyMenu
{
    partial class CharacterStatsDialog
    {
        private void InitializeComponent()
        {
            Bounds = new UniRectangle(
                new UniScalar(0.2f, 15.0f), new UniScalar(0.0f, 105.0f),
                new UniScalar(0.8f, -15.0f), new UniScalar(1.0f, -105.0f)
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.Battle
{
    public partial class AbilityStatDialog : WindowControl
    {
        private LabelControl _name;
        private LabelControl _mana;

        private void InitializeComponent()
        {
            Bounds =  new UniRectangle(
                new UniScalar(1.0f, -200.0f), new UniScalar(-1.0f, -38.0f), 
                new UniScalar(0.0f, 200.0f), new UniScalar(0.0f, 38.0f)
            );

            _name = new LabelControl();
            _name.Bounds = new UniRectangle(
                new UniScalar(0.0f, 5.0f), new UniScalar(0.0f, 5.0f),
                new UniScalar(1.0f, -35.0f), new UniScalar(0.0f, 20.0f)
            );
            Children.Add(_name);

            _mana = new LabelControl();
            _mana.Bounds = new UniRectangle(
                new UniScalar(1.0f, -30.0f), new UniScalar(0.0f, 5.0f),
                new UniScalar(0.0f, 25.0f), new UniScalar(0.0f, 20.0f)
            );
            Children.Add(_mana);
        }

        public void Hide()
        {
            Bounds.Location.Y.Fraction = -1.0f;
        }

        public void Show()
        {
            Bounds.Location.Y.Fraction = 1.0f;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SRPG.Data;

namespace SRPG.Scene.Battle
{
    public partial class CharacterStatsDialog : WindowControl
    {
        private LabelControl _name;
        private LabelControl _class;
        private LabelControl _health;
        private LabelControl _mana;
        private ProgressControl _healthBar;
        private ProgressControl _manaBar;

        private void InitializeComponent()
        {
            EnableDragging = false;
            Bounds = new UniRectangle
                (
                new UniScalar(1.0f, -400.0f), new UniScalar(0.0f, 0.0f),
                new UniScalar(0.0f, 400.0f), new UniScalar(0.0f, 130.0f)
                );

            _name = new LabelControl();
            _class = new LabelControl();
            _health = new LabelControl();
            _mana = new LabelControl();
            _healthBar = new ProgressControl();
            _manaBar = new ProgressControl();

            _name.Bounds = new UniRectangle
                (
                new UniScalar(0.0f, 10.0f), new UniScalar(0.0f, 10.0f),
                new UniScalar(0.5f, -15.0f), new UniScalar(0.0f, 30.0f)
                );
            _name.Text = "Name";
            Children.Add(_name);

            _class.Bounds = new UniRectangle
                (
                new UniScalar(0.5f, 10.0f), new UniScalar(0.0f, 10.0f),
                new UniScalar(0.5f, -15.0f), new UniScalar(0.0f, 30.0f)
                );
            _class.Text = "Class";
            Children.Add(_class);

            _health.Bounds = new UniRectangle
                (
                new UniScalar(1.0f, -70f), new UniScalar(1.0f, -80.0f),
                new UniScalar(0.0f, 60f), new UniScalar(0.0f, 30.0f)
                );
            _health.Text = "50 HP";
            Children.Add(_health);

            _mana.Bounds = new UniRectangle
                (
                new UniScalar(1.0f, -70f), new UniScalar(1.0f, -40.0f),
                new UniScalar(0.0f, 60f), new UniScalar(0.0f, 30.0f)
                );
            _mana.Text = "20 SP";
            Children.Add(_mana);

            _healthBar.Bounds = new UniRectangle
                (
                new UniScalar(0.0f, 10.0f), new UniScalar(1.0f, -80.0f),
                new UniScalar(1.0f, -100.0f), new UniScalar(0.0f, 30.0f)
                );
            _healthBar.Progress = 1.0f;
            Children.Add(_healthBar);

            _manaBar.Bounds = new UniRectangle
                (
                new UniScalar(0.0f, 10.0f), new UniScalar(1.0f, -40.0f),
                new UniScalar(1.0f, -100.0f), new UniScalar(0.0f, 30.0f)
                );
            _manaBar.Progress = 1.0f;
            Children.Add(_manaBar);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class CharacterOverviewDialog : InventoryWindowControl
    {
        private LabelControl _nameText;
        private LabelControl _classText;

        private LabelControl _healthLabel;
        private LabelControl _manaLabel;
        private LabelControl _healthText;
        private LabelControl _manaText;

        private void InitializeComponent()
        {
            // portrait (top right)

            // name (top left)
            _nameText = new LabelControl();
            _nameText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 20.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_nameText);

            // class (top left)
            _classText = new LabelControl();
            _classText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 55.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_classText);

            // health
            _healthLabel = new LabelControl();
            _healthLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 125.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _healthLabel.Text = "Health";
            Children.Add(_healthLabel);

            _healthText = new LabelControl();
            _healthText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 180.0F), new UniScalar(0.0F, 125.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_healthText);

            // mana
            _manaLabel = new LabelControl();
            _manaLabel.Bounds = new UniRectangle(
                new UniScalar(0.0F, 20.0F), new UniScalar(0.0F, 160.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            _manaLabel.Text = "Mana";
            Children.Add(_manaLabel);

            _manaText = new LabelControl();
            _manaText.Bounds = new UniRectangle(
                new UniScalar(0.0F, 180.0F), new UniScalar(0.0F, 160.0F),
                new UniScalar(150.0F, 0.0F), new UniScalar(25.0F, 0.0F)
            );
            Children.Add(_manaText);
        }
    }
}

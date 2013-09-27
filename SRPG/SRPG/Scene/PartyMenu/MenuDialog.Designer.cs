using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.PartyMenu
{
    public partial class MenuDialog : WindowControl
    {
        private ButtonControl _statusButton;
        private ButtonControl _inventoryButton;
        private ButtonControl _settingsButton;

        private void InitializeComponent()
        {
            EnableDragging = false;

            Bounds = new UniRectangle(
                new UniScalar(0.0f, 0.0f), new UniScalar(0.0f, 0.0f),
                new UniScalar(1.0f, 0.0f), new UniScalar(0.0f, 95.0f)
            );

            _statusButton = new ButtonControl();
            _statusButton.Text = "Status";
            _statusButton.Bounds = new UniRectangle(
                new UniScalar(0.025f, 0.0f), new UniScalar(0.0f, 15.0f),
                new UniScalar(0.30f, 0.0f), new UniScalar(0.0f, 65.0f)
            );
            Children.Add(_statusButton);

            _inventoryButton = new ButtonControl();
            _inventoryButton.Text = "Inventory";
            _inventoryButton.Bounds = new UniRectangle(
                new UniScalar(0.35f, 0.0f), new UniScalar(0.0f, 15.0f),
                new UniScalar(0.30f, 0.0f), new UniScalar(0.0f, 65.0f)
            );
            Children.Add(_inventoryButton);

            _settingsButton = new ButtonControl();
            _settingsButton.Text = "Settings";
            _settingsButton.Bounds = new UniRectangle(
                new UniScalar(0.675f, 0.0f), new UniScalar(0.0f, 15.0f),
                new UniScalar(0.30f, 0.0f), new UniScalar(0.0f, 65.0f)
            );
            Children.Add(_settingsButton);
        }
    }
}

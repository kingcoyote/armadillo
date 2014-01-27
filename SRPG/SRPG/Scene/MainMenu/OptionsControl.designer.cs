using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace SRPG.Scene.MainMenu
{
    public partial class OptionsControl : WindowControl
    {
        private ButtonControl _fullScreenButton;
        private ButtonControl _resolutionButton;
        private ButtonControl _saveButton;
        private ButtonControl _cancelButton;
        
        private void InitializeComponent()
        {
            // full screen label
            var label = new LabelControl();
            label.Text = "Full Screen";
            label.Bounds = new UniRectangle(10, 12, 100, 20);
            Children.Add(label);

            // full screen checkbox
            _fullScreenButton = new ButtonControl();
            _fullScreenButton.Bounds = new UniRectangle(150, 10, 150, 30);
            _fullScreenButton.Text = "No";
            Children.Add(_fullScreenButton);

            // resolution label
            label = new LabelControl();
            label.Text = "Resolution";
            label.Bounds = new UniRectangle(10, 52, 100, 20);
            Children.Add(label);

            // resolution selection
            _resolutionButton = new ButtonControl();
            _resolutionButton.Bounds = new UniRectangle(150, 50, 150, 30);
            _resolutionButton.Text = "1024x768";
            Children.Add(_resolutionButton);

            // save button
            _saveButton = new ButtonControl();
            _saveButton.Bounds = new UniRectangle(
                new UniScalar(0.5f, 10), new UniScalar(1, -40), 
                new UniScalar(0.5f, -20), new UniScalar(30)
            );
            _saveButton.Text = "Save";
            Children.Add(_saveButton);

            // cancel button
            _cancelButton = new ButtonControl();
            _cancelButton.Bounds = new UniRectangle(
                new UniScalar(0, 10), new UniScalar(1, -40), 
                new UniScalar(0.5f, -20), new UniScalar(30)
            );
            _cancelButton.Text = "Cancel";
            Children.Add(_cancelButton);
        }
    }
}

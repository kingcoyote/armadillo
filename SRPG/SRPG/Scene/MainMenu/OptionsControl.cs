using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Torch.UserInterface;
using Game = Torch.Game;

namespace SRPG.Scene.MainMenu
{
    public partial class OptionsControl
    {
        public delegate void OptionsCanceledDelegate();
        // todo - this is such a bad idea. i need a better container for the options
        public delegate void OptionsSavedDelegate(bool fullScreen, int width, int height);

        public OptionsCanceledDelegate OptionsCanceled = () => { };
        public OptionsSavedDelegate OptionsSaved = (f, w, h) => { };

        public OptionsControl()
        {
            InitializeComponent();
            _fullScreenButton.Pressed +=
                (s, a) => _fullScreenButton.Text = _fullScreenButton.Text == "Yes" ? "No" : "Yes";
            _resolutionButton.Pressed += ShowResolutions;

            _cancelButton.Pressed += (s, a) => OptionsCanceled.Invoke();
            _saveButton.Pressed += (s, a) =>
                {
                    var resolution = _resolutionButton.Text.Split('x');
                    OptionsSaved.Invoke
                        (_fullScreenButton.Text == "Yes", int.Parse(resolution[0]), int.Parse(resolution[1]));
                };
        }

        public void UpdateSettings(Game game)
        {
            var graphics = (GraphicsDeviceManager)(game.Services.GetService(typeof (GraphicsDeviceManager)));
            _fullScreenButton.Text = graphics.IsFullScreen ? "Yes" : "No";
            _resolutionButton.Text = graphics.PreferredBackBufferWidth + "x" + graphics.PreferredBackBufferHeight;
        }

        private void ShowResolutions(object sender, EventArgs args)
        {
            var dropdown = new DropDownControl();
            dropdown.Bounds = new UniRectangle(
                new UniScalar(0), new UniScalar(0),
                new UniScalar(1, 0), new UniScalar(1, 0)
            );
            Children.Add(dropdown);
            dropdown.BringToFront();

            foreach (var d in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if ((d.Format != SurfaceFormat.Color)) continue;
                if (!(Math.Abs(d.AspectRatio - 1.33) < 0.03 || Math.Abs(d.AspectRatio - 1.6) < 0.03 || Math.Abs(d.AspectRatio - 1.77) < 0.03)) continue;
                if (d.Height < 768) continue;

                dropdown.AddItem(d.Width + "x" + d.Height);
            }

            dropdown.ItemSelected += i =>
                {
                    _resolutionButton.Text = i;
                    Children.Remove(dropdown);
                };
        }
    }
}

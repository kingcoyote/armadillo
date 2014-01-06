using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Nuclex.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Torch;

namespace SRPG.Scene.Battle
{
    class HitLayer : Layer
    {
        private readonly Dictionary<string, int> _displayedHits = new Dictionary<string, int>();
        private readonly Dictionary<string, float> _hitLocations = new Dictionary<string, float>();
        private GuiManager _gui;
        private readonly Dictionary<string, LabelControl> _hitLabels = new Dictionary<string, LabelControl>(); 
        
        public HitLayer(Torch.Scene scene, Torch.Object parent) : base(scene, parent)
        {
            _gui = new GuiManager(
                (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager)),
                (IInputService)Game.Services.GetService(typeof(IInputService))
            );
            _gui.Screen = new Screen(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            _gui.Screen.Desktop.Bounds = new UniRectangle(
              new UniScalar(0.01F, 0.0F), new UniScalar(0.01F, 0.0F),
              new UniScalar(0.98F, 0.0F), new UniScalar(0.98F, 0.0F)
            );
            _gui.DrawOrder = 0;
            _gui.Initialize();
            Components.Add(_gui);
        }

        public void DisplayHit(int amount, Color color, Point target)
        {
            var key = string.Format(
                "hit/{0}/{1}/{2},{3}/{4}",
                amount,
                color,
                target.X,
                target.Y,
                new Random().Next()
            );

            var label = new LabelControl {Text = amount.ToString(), Bounds = new UniRectangle(OffsetX() + target.X*50, OffsetY() + target.Y*50 + 25, 75, 30)};
            _gui.Screen.Desktop.Children.Add(label);
            _hitLabels.Add(key, label);
            _hitLocations.Add(key, target.Y * 50 + 25);
            _displayedHits.Add(key, 0);
        }

        public override void Update(GameTime gametime)
        {
            for (var i = _displayedHits.Count; i > 0; i--)
            {
                var key = _displayedHits.Keys.ElementAt(i - 1);

                _displayedHits[key] += gametime.ElapsedGameTime.Milliseconds;
                if (_displayedHits[key] > 1200)
                {
                    _gui.Screen.Desktop.Children.Remove(_hitLabels[key]);
                    _hitLabels.Remove(key);
                    _displayedHits.Remove(key);
                }
                else
                {
                   
                    _hitLocations[key] -= 75F*gametime.ElapsedGameTime.Milliseconds/1000F;
                    _hitLabels[key].Bounds.Location.Y.Offset = (int) _hitLocations[key];
                }
            }
        }
    }
}

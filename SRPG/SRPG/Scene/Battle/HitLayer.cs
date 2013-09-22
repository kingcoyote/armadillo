using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Torch;

namespace SRPG.Scene.Battle
{
    class HitLayer : Layer
    {
        private readonly Dictionary<string, int> _displayedHits = new Dictionary<string, int>();
        private readonly Dictionary<string, float> _hitLocations = new Dictionary<string, float>();
        
        public HitLayer(Torch.Scene scene, Torch.Object parent) : base(scene, parent) { }

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

            //Objects.Add(key, new TextObject{ Font = FontManager.Get("Menu"), Color = color, Value = amount.ToString(), X = target.X * 50 + 25, Y = target.Y * 50, Z = 1000, Alignment = TextObject.AlignTypes.Center});
            _hitLocations.Add(key, target.Y * 50);
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
                    //Objects.Remove(key);
                    _displayedHits.Remove(key);
                }
                else
                {
                   
                    _hitLocations[key] -= 75F*gametime.ElapsedGameTime.Milliseconds/1000F;
                    //Objects[key].Y = (int)_hitLocations[key];
                }
            }
        }
    }
}

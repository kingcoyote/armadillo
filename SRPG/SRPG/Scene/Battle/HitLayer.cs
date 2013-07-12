﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SRPG.Data;
using Torch;

namespace SRPG.Scene.Battle
{
    class HitLayer : Layer
    {
        private Dictionary<string, int> _displayedHits = new Dictionary<string, int>();
        private Dictionary<string, float> _hitLocations = new Dictionary<string, float>();
        
        public HitLayer(Torch.Scene scene) : base(scene) { }

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

            Objects.Add(key, new TextObject{ Font = FontManager.Get("Menu"), Color = color, Value = amount.ToString(), X = target.X * 50 + 25, Y = target.Y * 50 - 25, Z = 1000, Alignment = TextObject.AlignTypes.Center});
            _hitLocations.Add(key, target.Y * 50);
            _displayedHits.Add(key, 0);
        }

        public override void Update(GameTime gameTime, Input input)
        {
            for (var i = _displayedHits.Count; i > 0; i--)
            {
                var key = _displayedHits.Keys.ElementAt(i - 1);

                _displayedHits[key] += gameTime.ElapsedGameTime.Milliseconds;
                if (_displayedHits[key] > 1200)
                {
                    Objects.Remove(key);
                    _displayedHits.Remove(key);
                }
                else
                {
                   
                    _hitLocations[key] -= 75F*gameTime.ElapsedGameTime.Milliseconds/1000F;
                    Objects[key].Y = (int)_hitLocations[key];
                }
            }
        }
    }
}
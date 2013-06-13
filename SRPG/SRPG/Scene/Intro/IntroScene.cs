using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.Intro
{
    class IntroScene : Torch.Scene
    {
        public IntroScene(Game game) : base(game) { }
        private const int SlideDuration = 3000;
        private int _slideTimer = SlideDuration;
        private int _currentSlide = 0;
        private List<string> _slides = new List<string>()
            {
                "credit slide", 
                "xna slide"
            };

        public override void Initialize()
        {
            base.Initialize();

            var keyboardLayer = new KeyboardInputLayer(this);

            keyboardLayer.AddKeyDownBinding(Keys.Escape, Skip);
            keyboardLayer.AddKeyDownBinding(Keys.Space, NextSlide);

            Layers.Add("keyboard input", keyboardLayer);

            Layers.Add("credit slide", new CreditSlide(this) { X = -50000 });
            Layers.Add("xna slide", new XnaSlide(this) { X = -50000 });

            Layers[_slides[_currentSlide]].X = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Input input)
        {
            base.Update(gameTime, input);

            _slideTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (_slideTimer <= 0)
            {
                NextSlide();
            }
        }

        /// <summary>
        /// Proceed to the next slide in the intro
        /// </summary>
        public void NextSlide()
        {
            _currentSlide++;

            if(_slides.Count == _currentSlide)
            {
                Skip();
                return;
            }
            
            _slideTimer = SlideDuration;
            Layers[_slides[_currentSlide - 1]].X = -50000;
            Layers[_slides[_currentSlide]].X = 0;
        }

        /// <summary>
        /// Skip the intro and go straight to the main menu
        /// </summary>
        public void Skip()
        {
            ((SRPGGame)Game).StartGame();
        }
    }
}

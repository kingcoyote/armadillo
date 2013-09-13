using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Torch;

namespace SRPG.Scene.Intro
{
    class IntroScene : Torch.Scene
    {
        public IntroScene(Game game) : base(game) { }
        private const int SlideDuration = 3000;
        private int _slideTimer = SlideDuration;
        private int _currentSlide;
        private readonly List<Layer> _slides = new List<Layer>();

        public override void Initialize()
        {
            base.Initialize();

            var keyboardLayer = new KeyboardInputLayer(this);

            keyboardLayer.AddKeyDownBinding(Keys.Escape, Skip);
            keyboardLayer.AddKeyDownBinding(Keys.Space, NextSlide);

            Components.Add(keyboardLayer);

            _slides.Add(new CreditSlide(this) { Visible = false });
            Components.Add(_slides[0]);

            _slides.Add(new XnaSlide(this) { Visible = false });
            Components.Add(_slides[1]);

            _slides[_currentSlide].X = 0;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gametime)
        {
            base.Update(gametime);

            _slideTimer -= gametime.ElapsedGameTime.Milliseconds;

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
            _slides[_currentSlide - 1].Visible = false;
            _slides[_currentSlide].Visible = true;
        }

        /// <summary>
        /// Skip the intro and go straight to the main menu
        /// </summary>
        public void Skip()
        {
            Game.ChangeScenes("main menu");
        }
    }
}

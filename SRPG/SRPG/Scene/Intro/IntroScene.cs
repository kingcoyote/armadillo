using System;
using System.Timers;
using Torch;

namespace SRPG.Scene.Intro
{
    class IntroScene : Torch.Scene
    {
        public IntroScene(Game game) : base(game) { }
        private Timer _timer;

        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// process a tick of the intro timer, either going to the next slide or the main menu.
        /// </summary>
        public void TimerTick()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Proceed to the next slide in the intro
        /// </summary>
        public void NextSlide()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Skip the intro and go straight to the main menu
        /// </summary>
        public void Skip()
        {
            throw new NotImplementedException();
        }
    }
}

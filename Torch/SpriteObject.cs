using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public class SpriteObject : ImageObject
    {
        public SpriteObject(Texture2D image) : base(image) { }
        private readonly Dictionary<string, SpriteAnimation> _animations = new Dictionary<string, SpriteAnimation>();
        private string _currentAnimation;
        private int _currentFrame;
        private int _frameTimer;

        public new int Width
        {
            get { return _animations[_currentAnimation].Size.Width; }
        }

        public new int Height
        {
            get { return _animations[_currentAnimation].Size.Height; }
        }


        public void AddAnimation(string name, SpriteAnimation animation)
        {
            _animations.Add(name, animation);
        }

        public void SetAnimation(string animation)
        {
            if(_animations.Keys.Contains(animation))
            {
                _currentAnimation = animation;
            }

            _frameTimer = CalculateNextFrameChange();
            _currentFrame = 0;
        }

        public string GetAnimation()
        {
            return _currentAnimation;
        }

        public override void Update(GameTime gameTime)
        {
            _frameTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if(_frameTimer <= 0)
            {
                ChangeFrame();
                _frameTimer = CalculateNextFrameChange();
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            var a = _animations[_currentAnimation];
            spriteBatch.Draw(
                _image, 
                new Vector2(X, Y),
                new Rectangle(_currentFrame*a.Size.Width + 1, a.StartRow , a.Size.Width, a.Size.Height),
                Color.White
            );
        }

        private void ChangeFrame()
        {
            _currentFrame++;
            if(_currentFrame == _animations[_currentAnimation].FrameCount)
            {
                _currentFrame = 0;
            }
        }

        private int CalculateNextFrameChange()
        {
            return 1000/_animations[_currentAnimation].FrameRate;
        }
    }

    public struct SpriteAnimation
    {
        public int FrameCount;
        public int FrameRate;
        public Rectangle Size;
        public int StartRow;
    }
}

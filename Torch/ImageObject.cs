using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public class ImageObject : Object
    {
        protected readonly Texture2D _image;
        public float XScale = 1.0F;
        public float YScale = 1.0F;
        public Rectangle SourceRectangle;

        private int _width = -1;
        private int _height = -1;

        protected ContentManager Content;

        public override int Width 
        {
            get { return _width == -1 ? _image.Width : _width; }
            set { _width = value; }
        }

        public override int Height
        {
            get { return _height == -1 ? _image.Height : _height; }
            set { _height = value; }
        }

        public ImageObject(Microsoft.Xna.Framework.Game game, Torch.Object parent, string imageName) : base(game, parent)
        {
            Content = (ContentManager)(Game.Services.GetService(typeof (ContentManager)));
            _image = Content.Load<Texture2D>(imageName);
            SourceRectangle = new Rectangle(0, 0, _image.Width, _image.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = new SpriteBatch(GraphicsDevice);
            
            spriteBatch.Begin();
            spriteBatch.Draw(_image, new Rectangle(X, Y, Width, Height), SourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}

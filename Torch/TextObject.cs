using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Torch
{
    public class TextObject : Object
    {
        public SpriteFont Font;
        public Color Color;
        public string Value;
        public AlignTypes Alignment;

        public enum AlignTypes
        {
            Left,
            Center,
            Right
        }

        private int _width = -1;
        public override int Width
        {
            get { return _width == -1 ? (int) (Font.MeasureString(Value).X) : _width; }
            set { _width = value; }
        }
        public override int Height
        {
            get { return Font.LineSpacing; }
        }

        public override Rectangle Rectangle
        {
            get
            {
                var rect = base.Rectangle;
                if (Alignment == AlignTypes.Center) rect.X -= Width/2;
                if (Alignment == AlignTypes.Right) rect.X -= Width;
                return rect;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_width == -1)
            {
                PrintString(spriteBatch, Value, Alignment, X, Y);
            }
            else
            {
                // split Value by spaces
                var words = Value.Split(" ".ToCharArray());

                // set string as null
                var printedString = "";

                int lines = 0;

                // loop through split value
                for (int i = 0; i < words.Length; i++)
                {
                    // append current chunk to string
                    printedString += words[i];

                    // if attaching next chunk would go greater than width
                    if ((i == words.Length - 1) || (i + 1 < words.Length && Font.MeasureString(printedString + " " + words[i + 1]).X > _width))
                    {
                        // print (including alignment offset)
                        PrintString(spriteBatch, printedString, Alignment, X, Y + Font.LineSpacing * lines);
                        // clear string
                        printedString = "";
                        lines++;
                    }
                    else
                    {
                        printedString += " ";
                    }
                }
            }
        }

        private void PrintString(SpriteBatch spriteBatch, string str, AlignTypes align, int x, int y)
        {
            switch (align)
            {
                case AlignTypes.Right:
                    x -= (int)Font.MeasureString(str).X;
                    break;
                case AlignTypes.Center:
                    x -= (int)(Font.MeasureString(str).X / 2);
                    break;
            }
            spriteBatch.DrawString(Font, str, new Vector2(x, y), Color);
        }
    }
}

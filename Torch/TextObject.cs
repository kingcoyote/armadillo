using System.Collections.Generic;
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

        public TextObject(Microsoft.Xna.Framework.Game game) : base(game) { } 

        public override int Width
        {
            get { return _width == -1 ? (int) (Font.MeasureString(Value).X) : _width; }
            set { _width = value; }
        }
        public override int Height
        {
            get { return Font.LineSpacing * GetPrintedLines().Count; }
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

        public override void Draw(GameTime gameTime)
        {
            var printedLines = GetPrintedLines();

            var spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();

            for(var i = 0; i < printedLines.Count; i++)
            {
                PrintString(spriteBatch, printedLines[i], Alignment, X, Y + Font.LineSpacing * i);
            }

            spriteBatch.End();
        }

        private List<string> GetPrintedLines()
        {
            if (_width == -1)
            {
                return new List<string>() { Value };
            }
            else
            {
                // split Value by spaces
                var words = Value.Split(" ".ToCharArray());

                // set string as null
                var printedString = "";

                var lines = new List<string>();

                // loop through split value
                for (int i = 0; i < words.Length; i++)
                {
                    // append current chunk to string
                    printedString += words[i];

                    // if attaching next chunk would go greater than width, or a newline is specified
                    if ((i == words.Length - 1) || (words[i + 1] == "\n") || (i + 1 < words.Length && Font.MeasureString(printedString + " " + words[i + 1]).X > _width))
                    {
                        // print (including alignment offset)
                        lines.Add(printedString);

                        // clear string
                        printedString = "";
                    }
                    else
                    {
                        printedString += " ";
                    }
                }

                return lines;
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

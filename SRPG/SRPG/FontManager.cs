using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SRPG
{
    class FontManager
    {
        private static Dictionary<string, SpriteFont> _fonts;
        private static FontSize _size;

        public static bool Add(string key, FontSize size, SpriteFont font)
        {
            if (_fonts.Keys.Contains(key + size)) return false;

            _fonts.Add(key + size, font);

            return true;
        }

        public static void Initialize(FontSize size)
        {
            _fonts = new Dictionary<string, SpriteFont>();
            _size = size;
        }

        public static SpriteFont Get(string key)
        {
            return _fonts[key + _size];
        }
    }

    public enum FontSize
    {
        Small,
        Normal
    }
}

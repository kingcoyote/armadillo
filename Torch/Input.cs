using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Torch
{
    public class Input
    {
        private MouseState _mouseState;
        private KeyboardState _keyboardState;
        private MouseState _oldMouseState;
        private KeyboardState _oldKeyboardState;

        public bool KeyDown;
        public bool KeyUp;
        public bool MouseDown;
        public bool MouseUp;

        public ButtonState LeftButton { get { return _mouseState.LeftButton;  } }
        public ButtonState MiddleButton { get { return _mouseState.MiddleButton; } }
        public ButtonState RightButton { get { return _mouseState.RightButton; } }
        public Point Cursor { get { return new Point(_mouseState.X, _mouseState.Y); } }

        public List<InputEventArgs> Events;

        public void Update(GameTime gameTime)
        {
            KeyDown = KeyUp = MouseDown = MouseUp = false;
            Events = new List<InputEventArgs>();

            _oldMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            _oldKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            foreach(Keys key in Enum.GetValues(typeof(Keys)))
            {
                bool shift = IsKeyDown(Keys.LeftShift) || IsKeyDown(Keys.RightShift);

                if(_keyboardState.IsKeyDown(key) && ! _oldKeyboardState.IsKeyDown(key))
                {
                    KeyDown = true;
                    Events.Add(new KeyboardEventArgs { Press = true, WhichKey = key, Character = GetChar(key, shift)} );
                }

                if (!_keyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyDown(key))
                {
                    KeyUp = true;
                    Events.Add(new KeyboardEventArgs { Press = false, WhichKey = key, Character = GetChar(key, shift) });
                }
            }

            if ((_mouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed))
            {
                Events.Add(new MouseEventArgs { Press = false, WhichButton = MouseButtons.Left, X = _mouseState.X, Y = _mouseState.Y });
            }

            if ((_mouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released))
            {
                Events.Add(new MouseEventArgs { Press = true, WhichButton = MouseButtons.Left, X = _mouseState.X, Y = _mouseState.Y });
            }

            if ((_mouseState.MiddleButton == ButtonState.Released && _oldMouseState.MiddleButton == ButtonState.Pressed))
            {
                Events.Add(new MouseEventArgs { Press = true, WhichButton = MouseButtons.Middle, X = _mouseState.X, Y = _mouseState.Y });
            }

            if ((_mouseState.MiddleButton == ButtonState.Pressed && _oldMouseState.MiddleButton == ButtonState.Released))
            {
                Events.Add(new MouseEventArgs { Press = false, WhichButton = MouseButtons.Middle, X = _mouseState.X, Y = _mouseState.Y });
            }

            if ((_mouseState.RightButton == ButtonState.Released && _oldMouseState.RightButton == ButtonState.Pressed))
            {
                Events.Add(new MouseEventArgs { Press = true, WhichButton = MouseButtons.Right, X = _mouseState.X, Y = _mouseState.Y });
            }

            if ((_mouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released))
            {
                Events.Add(new MouseEventArgs { Press = false, WhichButton = MouseButtons.Right, X = _mouseState.X, Y = _mouseState.Y });
            }
        }

        public bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _keyboardState.IsKeyUp(key);
        }

        public char GetChar(Keys key, bool shift)
        {
            var c = (char)0;

            switch (key)
            {
                // Backspace
                case Keys.Back: c = '\b'; break;

                // Enter
                case Keys.Enter: c = '\n'; break;

                // Alphabet keys
                case Keys.A: c = shift ? 'A' : 'a'; break;
                case Keys.B: c = shift ? 'B' : 'b'; break;
                case Keys.C: c = shift ? 'C' : 'c'; break;
                case Keys.D: c = shift ? 'D' : 'd'; break;
                case Keys.E: c = shift ? 'E' : 'e'; break;
                case Keys.F: c = shift ? 'F' : 'f'; break;
                case Keys.G: c = shift ? 'G' : 'g'; break;
                case Keys.H: c = shift ? 'H' : 'h'; break;
                case Keys.I: c = shift ? 'I' : 'i'; break;
                case Keys.J: c = shift ? 'J' : 'j'; break;
                case Keys.K: c = shift ? 'K' : 'k'; break;
                case Keys.L: c = shift ? 'L' : 'l'; break;
                case Keys.M: c = shift ? 'M' : 'm'; break;
                case Keys.N: c = shift ? 'N' : 'n'; break;
                case Keys.O: c = shift ? 'O' : 'o'; break;
                case Keys.P: c = shift ? 'P' : 'p'; break;
                case Keys.Q: c = shift ? 'Q' : 'q'; break;
                case Keys.R: c = shift ? 'R' : 'r'; break;
                case Keys.S: c = shift ? 'S' : 's'; break;
                case Keys.T: c = shift ? 'T' : 't'; break;
                case Keys.U: c = shift ? 'U' : 'u'; break;
                case Keys.V: c = shift ? 'V' : 'v'; break;
                case Keys.W: c = shift ? 'W' : 'w'; break;
                case Keys.X: c = shift ? 'X' : 'x'; break;
                case Keys.Y: c = shift ? 'Y' : 'y'; break;
                case Keys.Z: c = shift ? 'Z' : 'z'; break;

                // Decimal keys
                case Keys.D0: c = shift ? ')' : '0'; break;
                case Keys.D1: c = shift ? '!' : '1'; break;
                case Keys.D2: c = shift ? '@' : '2'; break;
                case Keys.D3: c = shift ? '#' : '3'; break;
                case Keys.D4: c = shift ? '$' : '4'; break;
                case Keys.D5: c = shift ? '%' : '5'; break;
                case Keys.D6: c = shift ? '^' : '6'; break;
                case Keys.D7: c = shift ? '&' : '7'; break;
                case Keys.D8: c = shift ? '*' : '8'; break;
                case Keys.D9: c = shift ? '(' : '9'; break;

                // Decimal numpad keys
                case Keys.NumPad0: c = '0'; break;
                case Keys.NumPad1: c = '1'; break;
                case Keys.NumPad2: c = '2'; break;
                case Keys.NumPad3: c = '3'; break;
                case Keys.NumPad4: c = '4'; break;
                case Keys.NumPad5: c = '5'; break;
                case Keys.NumPad6: c = '6'; break;
                case Keys.NumPad7: c = '7'; break;
                case Keys.NumPad8: c = '8'; break;
                case Keys.NumPad9: c = '9'; break;

                // Special keys
                case Keys.OemTilde: c = shift ? '~' : '`'; break;
                case Keys.OemSemicolon: c = shift ? ':' : ';'; break;
                case Keys.OemQuotes: c = shift ? '"' : '\''; break;
                case Keys.OemQuestion: c = shift ? '?' : '/'; break;
                case Keys.OemPlus: c = shift ? '+' : '='; break;
                case Keys.OemPipe: c = shift ? '|' : '\\'; break;
                case Keys.OemPeriod: c = shift ? '>' : '.'; break;
                case Keys.OemOpenBrackets: c = shift ? '{' : '['; break;
                case Keys.OemCloseBrackets: c = shift ? '}' : ']'; break;
                case Keys.OemMinus: c = shift ? '_' : '-'; break;
                case Keys.OemComma: c = shift ? '<' : ','; break;
                case Keys.Space: c = ' '; break;
            }

            return c;
        }
    }

    [Flags]
    public enum MouseButtons
    {
        None = 0x00,
        Left = 0x01,
        Middle = 0x02,
        Right = 0x04
    }

    public class InputEventArgs : EventArgs
    {
        public bool Handled;
    }

    public class KeyboardEventArgs : InputEventArgs
    {
        public Keys WhichKey;
        public bool Press;
        public char Character;
    }

    public class MouseEventArgs : InputEventArgs
    {
        public MouseButtons WhichButton;
        public int X;
        public int Y;
        public bool Press;
    }
}

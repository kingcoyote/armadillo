using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Torch
{
    public class TorchHelper
    {
        public static Point Vector2ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
    }
}

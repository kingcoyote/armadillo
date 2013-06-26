using System;
using SRPG.Scene.Overworld;

namespace SRPG.Data
{
    public class InteractEventArgs : EventArgs
    {
        public OverworldScene Scene;
        public Avatar Character;
    }
}
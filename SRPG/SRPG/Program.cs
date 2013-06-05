using System;

namespace SRPG
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SRPGGame game = new SRPGGame())
            {
                game.Run();
            }
        }
    }
#endif
}


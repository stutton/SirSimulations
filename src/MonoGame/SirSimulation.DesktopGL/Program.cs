using System;

namespace SirSimulation.DesktopGL
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new global::SirSimulation.GameOld())
                game.Run();
        }
    }
}

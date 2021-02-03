using System;

namespace Air_Evade
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AirEvadeGame())
                game.Run();
        }
    }
}

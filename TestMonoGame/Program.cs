using System;

namespace TestMonoGame
{
    public static class Program
    {
        static void Main(string[] args)
        {
            using (var game = new TestGame())
            {
                game.Run();
            }
        }
    }
}
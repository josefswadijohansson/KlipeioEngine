using OpenTK;
using OpenTK.Mathematics;

namespace KlipeioEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(Game game = new Game("Klipeio Engine", 1024, 720))
            {
                game.Run();
            }
        }
    }
}
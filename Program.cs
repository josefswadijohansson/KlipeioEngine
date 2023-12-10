using OpenTK;

namespace KlipeioEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(Game game = new Game("Klipeio Engine", 500, 500))
            {
                game.Run();
            }
        }
    }
}
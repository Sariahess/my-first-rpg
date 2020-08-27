using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sariah_assign2_RPG_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Game newGame = new Game();
            newGame.Start();
            Console.WriteLine("");
            Console.WriteLine("Press any key to exit game.");
            Console.ReadKey();
        }
    }
}

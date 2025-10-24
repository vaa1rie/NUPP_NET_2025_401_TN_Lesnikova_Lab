using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Common
{
    public class PhysicalGame : Game
    {
        public string Platform { get; set; }

        public PhysicalGame(string title, string genre, double price, string platform)
            : base(title, genre, price)
        {
            Platform = platform;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Title} [Phisical Game] - {Platform} - ${Price}");
        }
    }
}

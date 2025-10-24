using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Common
{
    public class DigitalGame : Game
    {
        public double FileSizeGB { get; set; }

        public DigitalGame(string title, string genre, double price, double fileSizeGB)
            : base(title, genre, price)
        {
            FileSizeGB = fileSizeGB;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"{Title} [Digital Game] - {FileSizeGB}GB - ${Price}");
        }
    }
}

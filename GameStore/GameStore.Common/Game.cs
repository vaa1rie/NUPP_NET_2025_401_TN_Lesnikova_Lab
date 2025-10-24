using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Common
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }

        public Game(string title, string genre, double price)
        {
            Id = Guid.NewGuid();
            Title = title;
            Genre = genre;
            Price = price;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"{Title} ({Genre}) - ${Price}");
        }
    }
}

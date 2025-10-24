using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Common
{
    public static class GameExtensions
    {
        public static void ApplyDiscount(this Game game, double percent)
        {
            game.Price -= game.Price * percent / 100;
        }
    }
}

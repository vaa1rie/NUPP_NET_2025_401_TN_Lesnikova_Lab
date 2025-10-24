using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Common
{
    public delegate void GamePurchasedHandler(string message);

    public class PurchaseService
    {
        public event GamePurchasedHandler OnGamePurchased;

        public void PurchaseGame(Customer customer, Game game)
        {
            customer.Purchase(game);
            OnGamePurchased?.Invoke($"{customer.Name} bought {game.Title}");
        }
    }
}

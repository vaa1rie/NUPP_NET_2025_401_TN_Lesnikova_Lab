using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Common;

namespace GameStore.main
{
     public class Program
    {
        static void Main(string[] args)
        {
            GameHelper.PrintWelcome();

            var gameService = new CrudService<Game>();

            var gta = new PhysicalGame("GTA V", "OutLast", 29.99, "PS5");
            var minecraft = new DigitalGame("Minecraft", "StardewWalley", 19.99, 1.2);

            gameService.Create(gta);
            gameService.Create(minecraft);

            foreach (var game in gameService.ReadAll())
            {
                game.DisplayInfo();
            }

            minecraft.ApplyDiscount(10);

            var customer = new Customer("Andriy", "andriy@gmail.com");
            var purchaseService = new PurchaseService();
            purchaseService.OnGamePurchased += msg => Console.WriteLine("EVENT: " + msg);
            purchaseService.PurchaseGame(customer, minecraft);
        }
    }
}
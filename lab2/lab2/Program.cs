using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var filePath = "buses.dat";
        var service = new InMemoryCrudServiceAsync<Bus>(filePath);

        int count = 1000;
        var buses = new List<Bus>();
        var tasks = new List<Task>();

        var semaphore = new SemaphoreSlim(10);
        var autoResetEvent = new AutoResetEvent(true);
        object lockObj = new object();


        Parallel.For(0, count, i =>
        {
            semaphore.Wait();
            autoResetEvent.WaitOne();
            var bus = Bus.CreateNew();
            lock (lockObj)
            {
                buses.Add(bus);
            }
            autoResetEvent.Set();
            semaphore.Release();
        });

        foreach (var bus in buses)
        {
            await service.CreateAsync(bus);
        }

        var allBuses = (await service.ReadAllAsync()).ToList();
        var minSeats = allBuses.Min(b => b.Seats);
        var maxSeats = allBuses.Max(b => b.Seats);
        var avgSeats = allBuses.Average(b => b.Seats);

        Console.WriteLine($"Min Seats: {minSeats}");
        Console.WriteLine($"Max Seats: {maxSeats}");
        Console.WriteLine($"Avg Seats: {avgSeats:F2}");

        await service.SaveAsync();
        Console.WriteLine("Collection saved to file.");
    }
}

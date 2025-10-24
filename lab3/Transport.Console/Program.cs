using Microsoft.EntityFrameworkCore;
using Transport.Infrastructure;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Repositories;
using Transport.Infrastructure.Services;
using System;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        var optionsBuilder = new DbContextOptionsBuilder<TransportContext>();
        optionsBuilder.UseSqlite("Data Source=transport.db");

        using (var context = new TransportContext(optionsBuilder.Options))
        {
            await context.Database.MigrateAsync();
            await InitializeTestData(context);
        }

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Система управління транспортом ===");
            Console.WriteLine("1. Показати всі транспортні засоби");
            Console.WriteLine("2. Додати новий транспорт");
            Console.WriteLine("3. Видалити транспорт");
            Console.WriteLine("0. Вихід");
            Console.Write("\nВиберіть опцію: ");

            var choice = Console.ReadLine();
            if (choice == "0")
            {
                exit = true;
                continue;
            }

            await ProcessChoice(choice, optionsBuilder.Options);

            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();

        using (var context = new TransportContext(optionsBuilder.Options))
        {
            var busRepository = new BusRepository(context);
            var tramRepository = new TramRepository(context);
            var trolleybusRepository = new TrolleybusRepository(context);
            
            var busService = new BusService(busRepository);
            var tramService = new TramService(tramRepository);
            var trolleybusService = new TrolleybusService(trolleybusRepository);

            await context.Database.MigrateAsync();

            
            if (!await context.Vehicles.AnyAsync())
            {
                var bus = new Bus { Model = "Bogdan A70132", Year = 2020, Seats = 45 };
                var tram = new Tram { Model = "Tatra T3", Year = 2018, PowerSupply = "Electric" };
                var trolleybus = new Trolleybus { Model = "LAZ E301", Year = 2019, PowerSupply = "Electric" };

                bus.TechnicalPassport = new TechnicalPassport 
                { 
                    SerialNumber = "BP001", 
                    IssueDate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(3)
                };
                
                tram.TechnicalPassport = new TechnicalPassport 
                { 
                    SerialNumber = "TP002", 
                    IssueDate = DateTime.Now.AddMonths(-6),
                    ExpiryDate = DateTime.Now.AddYears(2)
                };
                
                trolleybus.TechnicalPassport = new TechnicalPassport 
                { 
                    SerialNumber = "TB003", 
                    IssueDate = DateTime.Now.AddMonths(-2),
                    ExpiryDate = DateTime.Now.AddYears(3)
                };

                bus.MaintenanceRecords = new List<MaintenanceRecord>
                {
                    new MaintenanceRecord 
                    { 
                        Date = DateTime.Now.AddMonths(-1),
                        Description = "Oil change",
                        Cost = 1500
                    },
                    new MaintenanceRecord 
                    { 
                        Date = DateTime.Now,
                        Description = "Brake check",
                        Cost = 2000
                    }
                };

                tram.MaintenanceRecords = new List<MaintenanceRecord>
                {
                    new MaintenanceRecord 
                    { 
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "Wheel replacement",
                        Cost = 3000
                    }
                };

                trolleybus.MaintenanceRecords = new List<MaintenanceRecord>
                {
                    new MaintenanceRecord 
                    { 
                        Date = DateTime.Now.AddDays(-15),
                        Description = "Electric system check",
                        Cost = 1800
                    },
                    new MaintenanceRecord 
                    { 
                        Date = DateTime.Now.AddDays(-5),
                        Description = "Battery replacement",
                        Cost = 5000
                    }
                };

                var route1 = new Route { Number = "1A", StartPoint = "Central Station", EndPoint = "Airport" };
                var route2 = new Route { Number = "2B", StartPoint = "Mall", EndPoint = "University" };
                var route3 = new Route { Number = "3C", StartPoint = "Railway Station", EndPoint = "Shopping Center" };
                var route4 = new Route { Number = "4D", StartPoint = "Hospital", EndPoint = "Park" };

                await context.Routes.AddRangeAsync(route1, route2, route3, route4);
                await context.SaveChangesAsync();

                bus.RouteVehicles = new List<RouteVehicle>
                {
                    new RouteVehicle { Route = route1 },
                    new RouteVehicle { Route = route2 }
                };

                tram.RouteVehicles = new List<RouteVehicle>
                {
                    new RouteVehicle { Route = route2 },
                    new RouteVehicle { Route = route3 }
                };

                trolleybus.RouteVehicles = new List<RouteVehicle>
                {
                    new RouteVehicle { Route = route3 },
                    new RouteVehicle { Route = route4 }
                };

                await busService.CreateAsync(bus);
                await tramService.CreateAsync(tram);
                await trolleybusService.CreateAsync(trolleybus);

                Console.WriteLine("Тестові дані додано успішно!");
            }

            Console.WriteLine("\nСписок всіх транспортних засобів:");
            
            var vehicles = await context.Vehicles
                .Include(v => v.TechnicalPassport)
                .Include(v => v.MaintenanceRecords)
                .Include(v => v.RouteVehicles)
                    .ThenInclude(rv => rv.Route)
                .ToListAsync();

            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"\nТранспорт: {vehicle.Model} ({vehicle.GetType().Name})");
                
                if (vehicle.TechnicalPassport != null)
                    Console.WriteLine($"Тех. паспорт: {vehicle.TechnicalPassport.SerialNumber}");
                
                Console.WriteLine("Записи про обслуговування:");
                foreach (var record in vehicle.MaintenanceRecords)
                    Console.WriteLine($"- {record.Date:d}: {record.Description} (${record.Cost})");
                
                Console.WriteLine("Маршрути:");
                foreach (var rv in vehicle.RouteVehicles)
                    Console.WriteLine($"- {rv.Route.Number}: {rv.Route.StartPoint} - {rv.Route.EndPoint}");
            }
        }

        }
    }

    private static async Task InitializeTestData(TransportContext context)
    {
        if (!await context.Vehicles.AnyAsync())
        {
            var bus = new Bus { Model = "Bogdan A70132", Year = 2020, Seats = 45 };
            var tram = new Tram { Model = "Tatra T3", Year = 2018, PowerSupply = "Electric" };
            var trolleybus = new Trolleybus { Model = "LAZ E301", Year = 2019, PowerSupply = "Electric" };

            bus.TechnicalPassport = new TechnicalPassport 
            { 
                SerialNumber = "BP001", 
                IssueDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddYears(3)
            };
            
            tram.TechnicalPassport = new TechnicalPassport 
            { 
                SerialNumber = "TP002", 
                IssueDate = DateTime.Now.AddMonths(-6),
                ExpiryDate = DateTime.Now.AddYears(2)
            };
            
            trolleybus.TechnicalPassport = new TechnicalPassport 
            { 
                SerialNumber = "TB003", 
                IssueDate = DateTime.Now.AddMonths(-2),
                ExpiryDate = DateTime.Now.AddYears(3)
            };

            bus.MaintenanceRecords = new List<MaintenanceRecord>
            {
                new MaintenanceRecord 
                { 
                    Date = DateTime.Now.AddMonths(-1),
                    Description = "Oil change",
                    Cost = 1500
                },
                new MaintenanceRecord 
                { 
                    Date = DateTime.Now,
                    Description = "Brake check",
                    Cost = 2000
                }
            };

            tram.MaintenanceRecords = new List<MaintenanceRecord>
            {
                new MaintenanceRecord 
                { 
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Wheel replacement",
                    Cost = 3000
                }
            };

            trolleybus.MaintenanceRecords = new List<MaintenanceRecord>
            {
                new MaintenanceRecord 
                { 
                    Date = DateTime.Now.AddDays(-15),
                    Description = "Electric system check",
                    Cost = 1800
                },
                new MaintenanceRecord 
                { 
                    Date = DateTime.Now.AddDays(-5),
                    Description = "Battery replacement",
                    Cost = 5000
                }
            };

            var route1 = new Route { Number = "1A", StartPoint = "Central Station", EndPoint = "Airport" };
            var route2 = new Route { Number = "2B", StartPoint = "Mall", EndPoint = "University" };
            var route3 = new Route { Number = "3C", StartPoint = "Railway Station", EndPoint = "Shopping Center" };
            var route4 = new Route { Number = "4D", StartPoint = "Hospital", EndPoint = "Park" };

            await context.Routes.AddRangeAsync(route1, route2, route3, route4);
            await context.SaveChangesAsync();

            bus.RouteVehicles = new List<RouteVehicle>
            {
                new RouteVehicle { Route = route1 },
                new RouteVehicle { Route = route2 }
            };

            tram.RouteVehicles = new List<RouteVehicle>
            {
                new RouteVehicle { Route = route2 },
                new RouteVehicle { Route = route3 }
            };

            trolleybus.RouteVehicles = new List<RouteVehicle>
            {
                new RouteVehicle { Route = route3 },
                new RouteVehicle { Route = route4 }
            };

            await context.Vehicles.AddRangeAsync(bus, tram, trolleybus);
            await context.SaveChangesAsync();
        }
    }

    private static async Task ProcessChoice(string choice, DbContextOptions<TransportContext> options)
    {
        using (var context = new TransportContext(options))
        {
            switch (choice)
            {
                case "1":
                    await ShowAllVehicles(context);
                    break;
                case "2":
                    await AddNewVehicle(context);
                    break;
                case "3":
                    await DeleteVehicle(context);
                    break;
            }
        }
    }

    private static async Task ShowAllVehicles(TransportContext context)
    {
        var vehicles = await context.Vehicles
            .Include(v => v.TechnicalPassport)
            .Include(v => v.MaintenanceRecords)
            .Include(v => v.RouteVehicles)
                .ThenInclude(rv => rv.Route)
            .ToListAsync();

        if (!vehicles.Any())
        {
            Console.WriteLine("\nНемає доступних транспортних засобів.");
            return;
        }

        Console.WriteLine("\nСписок всіх транспортних засобів:");
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine($"\nТранспорт: {vehicle.Model} ({vehicle.GetType().Name})");
            
            if (vehicle.TechnicalPassport != null)
                Console.WriteLine($"Тех. паспорт: {vehicle.TechnicalPassport.SerialNumber}");
            
            Console.WriteLine("Записи про обслуговування:");
            foreach (var record in vehicle.MaintenanceRecords)
                Console.WriteLine($"- {record.Date:d}: {record.Description} (${record.Cost})");
            
            Console.WriteLine("Маршрути:");
            foreach (var rv in vehicle.RouteVehicles)
                Console.WriteLine($"- {rv.Route.Number}: {rv.Route.StartPoint} - {rv.Route.EndPoint}");
        }
    }

    private static async Task AddNewVehicle(TransportContext context)
    {
        Console.WriteLine("\nДодавання нового транспорту");
        Console.WriteLine("1. Автобус");
        Console.WriteLine("2. Трамвай");
        Console.WriteLine("3. Тролейбус");
        Console.Write("Виберіть тип транспорту: ");
        var type = Console.ReadLine();

        Console.Write("Введіть модель: ");
        var model = Console.ReadLine();
        Console.Write("Введіть рік випуску: ");
        var year = int.Parse(Console.ReadLine() ?? "0");

        Vehicle vehicle = type switch
        {
            "1" => new Bus { Model = model, Year = year, Seats = 40 },
            "2" => new Tram { Model = model, Year = year, PowerSupply = "Electric" },
            "3" => new Trolleybus { Model = model, Year = year, PowerSupply = "Electric" },
            _ => throw new ArgumentException("Невідомий тип транспорту")
        };

        Console.Write("Введіть серійний номер технічного паспорту: ");
        var serialNumber = Console.ReadLine();
        vehicle.TechnicalPassport = new TechnicalPassport
        {
            SerialNumber = serialNumber,
            IssueDate = DateTime.Now,
            ExpiryDate = DateTime.Now.AddYears(3)
        };

        await context.Vehicles.AddAsync(vehicle);
        await context.SaveChangesAsync();
        Console.WriteLine("\nТранспортний засіб успішно додано!");
    }

    private static async Task DeleteVehicle(TransportContext context)
    {
        var vehicles = await context.Vehicles.ToListAsync();
        if (!vehicles.Any())
        {
            Console.WriteLine("\nНемає доступних транспортних засобів для видалення.");
            return;
        }

        Console.WriteLine("\nСписок транспортних засобів:");
        for (int i = 0; i < vehicles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {vehicles[i].Model} ({vehicles[i].GetType().Name})");
        }

        Console.Write("\nВиберіть номер транспорту для видалення: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= vehicles.Count)
        {
            var vehicleToDelete = vehicles[index - 1];
            context.Vehicles.Remove(vehicleToDelete);
            await context.SaveChangesAsync();
            Console.WriteLine("Транспортний засіб успішно видалено!");
        }
        else
        {
            Console.WriteLine("Невірний номер транспорту!");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Dummy Bus class for testing
[Serializable]
public class Bus
{
    public Guid Id { get; set; }
    public int Seats { get; set; }

    public static Bus CreateNew()
    {
        return new Bus
        {
            Id = Guid.NewGuid(),
            Seats = new Random().Next(10, 100)
        };
    }
}

[TestClass]
public class InMemoryCrudServiceAsyncTests
{
    private string _testFilePath;

    [TestInitialize]
    public void Setup()
    {
        _testFilePath = Path.GetTempFileName();
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldAddElement()
    {
        var service = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
        var bus = Bus.CreateNew();

        var result = await service.CreateAsync(bus);

        Assert.IsTrue(result);
        var all = await service.ReadAllAsync();
        Assert.AreEqual(1, all.Count());
        Assert.AreEqual(bus.Id, all.First().Id);
    }

    [TestMethod]
    public async Task ReadAsync_ShouldReturnElementById()
    {
        var service = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        var found = await service.ReadAsync(bus.Id);

        Assert.IsNotNull(found);
        Assert.AreEqual(bus.Id, found.Id);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateElement()
    {
        var service = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        bus.Seats = 99;
        var updated = await service.UpdateAsync(bus);

        Assert.IsTrue(updated);
        var found = await service.ReadAsync(bus.Id);
        Assert.AreEqual(99, found.Seats);
    }

    [TestMethod]
    public async Task RemoveAsync_ShouldRemoveElement()
    {
        var service = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
        var bus = Bus.CreateNew();
        await service.CreateAsync(bus);

        var removed = await service.RemoveAsync(bus);

        Assert.IsTrue(removed);
        var found = await service.ReadAsync(bus.Id);
        Assert.IsNull(found);
    }

    [TestMethod]
    public async Task SaveAsync_And_LoadFromFile_ShouldPersistData()
    {
        var bus = Bus.CreateNew();
        {
            var service = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
            await service.CreateAsync(bus);
            await service.SaveAsync();
        }
        {
            var service2 = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
            var all = await service2.ReadAllAsync();
            Assert.AreEqual(1, all.Count());
            Assert.AreEqual(bus.Id, all.First().Id);
        }
    }

    [TestMethod]
    public async Task ReadAllAsync_WithPagination_ShouldReturnCorrectPage()
    {
        var service = new InMemoryCrudServiceAsync<Bus>(_testFilePath);
        var buses = new List<Bus>();
        for (int i = 0; i < 10; i++)
        {
            var bus = Bus.CreateNew();
            buses.Add(bus);
            await service.CreateAsync(bus);
        }

        var page = await service.ReadAllAsync(2, 3); // page 2, 3 items per page

        Assert.AreEqual(3, page.Count());
        CollectionAssert.AreEqual(
            buses.Skip(3).Take(3).Select(b => b.Id).ToList(),
            page.Select(b => b.Id).ToList()
        );
    }
}

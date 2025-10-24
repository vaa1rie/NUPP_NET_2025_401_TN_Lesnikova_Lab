using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace lab2.Tests
{
    [TestClass]
    public class InMemoryCrudServiceAsyncTests
    {
        private string testFilePath;
        private InMemoryCrudServiceAsync<Bus> service;

        [TestInitialize]
        public void Setup()
        {
            testFilePath = "test_buses.dat";
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);

            service = new InMemoryCrudServiceAsync<Bus>(testFilePath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        [TestMethod]
        public async Task CreateAsync_AddsBus()
        {
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            var all = await service.ReadAllAsync();
            Assert.AreEqual(1, all.Count());
            Assert.AreEqual(bus.Id, all.First().Id);
        }

        [TestMethod]
        public async Task ReadAsync_ReturnsCorrectBus()
        {
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            var result = await service.ReadAsync(bus.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(bus.Id, result.Id);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesBus()
        {
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            bus.Seats += 10;
            await service.UpdateAsync(bus);

            var updated = await service.ReadAsync(bus.Id);
            Assert.AreEqual(bus.Seats, updated.Seats);
        }

        [TestMethod]
        public async Task DeleteAsync_RemovesBus()
        {
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);

            await service.RemoveAsync(bus); 

            var all = await service.ReadAllAsync();
            Assert.AreEqual(0, all.Count());
        }

        [TestMethod]
        public async Task SaveAsync_PersistsData()
        {
            var bus = Bus.CreateNew();
            await service.CreateAsync(bus);
            await service.SaveAsync();

            var newService = new InMemoryCrudServiceAsync<Bus>(testFilePath);
            var all = await newService.ReadAllAsync();
            Assert.AreEqual(1, all.Count());
            Assert.AreEqual(bus.Id, all.First().Id);
        }
    }
}
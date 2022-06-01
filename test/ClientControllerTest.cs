using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using zmdh;
using System.Threading.Tasks;

namespace test
{
    public class ClientControllerTest
    {
        ClientController Cc = new ClientController(GetInMemoryDBMetData());
        private static string databaseName; // zonder deze property kun je geen clean context maken
        private static DBManager GetInMemoryDBMetData() 
        {
            DBManager context = GetNewInMemoryDatabase(true);
            context.Add(new Client(){ClientId = 1, Adres = "Huis", Residence = "Home"});
            context.SaveChanges();
            return GetNewInMemoryDatabase(false); // gebruik een nieuw (clean) object voor de context
        }
        private static DBManager GetNewInMemoryDatabase(bool NewDb) 
        {
            if (NewDb) databaseName = Guid.NewGuid().ToString(); // unieke naam
            var options = new DbContextOptionsBuilder<DBManager>()
            .UseInMemoryDatabase(databaseName)
            .Options;
            return new DBManager(options);
        }
        [Fact]
        public async Task TestDetails()
        {
           var result = await Cc.Details(1);
           var result2 = await Cc.Details(2);

           Assert.IsType<ViewResult>(result);
           Assert.IsType<NotFoundResult>(result2);
        }
        [Fact]
        public async Task TestCreate()
        {
            Client testClient = new Client(){ClientId = 1234, Adres = "Huis", Residence = "Home"};

            var result = await Cc.Create(testClient);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public async Task TestEdit()
        {
            Client beforeClient = await Cc._context.Clienten.SingleAsync(s => s.ClientId == 1);
            Assert.Equal("Huis", beforeClient.Adres);

            beforeClient.Adres = "Kelder";
            await Cc.Edit(1, beforeClient);
            
            Client afterClient = await Cc._context.Clienten.SingleAsync(s => s.ClientId == 1);
            Assert.Equal("Kelder", afterClient.Adres);
        }
        [Fact]
        public async Task TestDelete()
        {
            var result = await Cc.Delete(1);
            var result2 = await Cc.Delete(2);

           Assert.IsType<ViewResult>(result);
           Assert.IsType<NotFoundResult>(result2);
        }
        [Fact]
        public void TestClientExists()
        {
            Assert.True(Cc.ClientExists(1));
            Assert.False(Cc.ClientExists(99999999));
        }
    }
}

using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using zmdh;
using System.Threading.Tasks;
using zmdh.Controllers;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Security.Claims;

namespace test
{
    public class VestigingControllerTest
    {
        VestigingController Vc = new VestigingController(GetInMemoryDBMetData());
        private static string databaseName; // zonder deze property kun je geen clean context maken
        private static DBManager GetInMemoryDBMetData() 
        {
            DBManager context = GetNewInMemoryDatabase(true);
            context.Add(new Vestiging(){Id = 1, Name = "Home", Adress = "nietus", Plaats = "Sukkeltje"});
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
        public async Task testCreate()
        {
            Vestiging testObject = new Vestiging(){Id = 2, Name = "Home", Adress = "nietus", Plaats = "Sukkeltje"};
            var result = await Vc.Create(testObject);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(await Vc._context.Vestigingen.ContainsAsync(testObject));
        }
    }
}
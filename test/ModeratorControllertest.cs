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
    public class ModeratorControllerTest
    {
        ModeratorController Mc = new ModeratorController(GetInMemoryDBMetData());
        private static string databaseName; // zonder deze property kun je geen clean context maken
        private static DBManager GetInMemoryDBMetData() 
        {
            DBManager context = GetNewInMemoryDatabase(true);
            Hulpverlener h = new Hulpverlener(){Id = 1, Name = "A", Adres = "a", Specialisatie = "a", Intro = "a", Study = "a", OverJou = "a", Behandeling = "a", Foto = "a",  VestigingId = 1};
            context.Add(h);
            context.Add(new Client(){ClientId = 1, Adres = "Huis", Residence = "Home", Hulpverlener = h});
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
        public void testFillList()
        {
        }
    }
}
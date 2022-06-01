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
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace test
{
    public class ReportControllerTest
    {
        ReportController Rc = new ReportController(GetInMemoryDBMetData(), GetUsermanager());
        private static string databaseName; // zonder deze property kun je geen clean context maken
        private static DBManager GetInMemoryDBMetData() 
        {
            DBManager context = GetNewInMemoryDatabase(true);
            context.Add(new Hulpverlener(){Id = 1, Name = "A", Adres = "a", Specialisatie = "a", Intro = "a", Study = "a", OverJou = "a", Behandeling = "a", Foto = "a",  VestigingId = 1});
            context.Add(new Hulpverlener(){Id = 2, Name = "B", Adres = "a", Specialisatie = "a", Intro = "a", Study = "a", OverJou = "a", Behandeling = "a", Foto = "a",  VestigingId = 1});
            context.Add(new Hulpverlener(){Id = 3, Name = "C", Adres = "a", Specialisatie = "a", Intro = "a", Study = "a", OverJou = "a", Behandeling = "a", Foto = "a",  VestigingId = 1});
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
        private static UserManager<ApplicationUser> GetUsermanager()
        {
            //setup(mock) _usermanager
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            var user = new ApplicationUser() { Id = "f00", UserName = "f00", Email = "f00@example.com" };
            
            mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mgr.Setup(l => l.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Verifiable();
            mgr.Setup(l => l.CreateAsync(It.IsAny<ApplicationUser>(), "Fail")).ReturnsAsync(IdentityResult.Failed()).Verifiable();
            mgr.Setup(y => y.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Verifiable();

            return mgr.Object;
        }
        [Fact]
        public async Task testCreate()
        {
            Report testObject = new Report(){Content = "content", isHandled = false};
            
            var result = await Rc.Create(testObject);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(await Rc._context.Reports.ContainsAsync(testObject));
        }
    }
}
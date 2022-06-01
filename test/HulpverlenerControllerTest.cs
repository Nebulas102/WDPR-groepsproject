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
    public class HulpverlenerControllerTest
    {
        HulpverlenerController Hc = new HulpverlenerController(GetInMemoryDBMetData(), GetUsermanager());
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
        public void TestSorteer()
        {
            //get hulpverlener list
            IQueryable<Hulpverlener> list = Hc._context.Hulpverleners;
            //assert
            Assert.True(list.First().Name == "A");
            //action
            IQueryable<Hulpverlener> list1 = Hc.Sorteer(list, "naam_aflopend");
            //assert
            Assert.True(list1.First().Name == "C");
        }
        [Fact]
        public void testZoek()
        {
            //get hulpverlener list and single objects
            IQueryable<Hulpverlener> list = Hc._context.Hulpverleners;
            Hulpverlener b = list.Single(d => d.Name == "B");
            Hulpverlener a = list.Single(d => d.Name == "A");
            //action
            IQueryable<Hulpverlener> list1 = Hc.Zoek(list, "B");
            //assert
            Assert.True(list1.Contains(b));
            Assert.False(list1.Contains(a));
        }
        [Fact]
        public void testPagineer()
        {
            //get hulpverlener list
            IQueryable<Hulpverlener> list = Hc._context.Hulpverleners;
            //action
            IQueryable<Hulpverlener> list1 = Hc.Pagineer(list, 0, 2);
            //assert
            Assert.True(list1.Count() == 2);
        }
        //testing if authorize exists
        [Fact]
        public void testAuthorizeInbox()
        {
            //getting the type of the custom attribute added to inbox
            var Authorize = Hc.GetType().GetMethod("Inbox").GetCustomAttributes(typeof(AuthorizeAttribute), true);
            //assert
            Assert.Equal(typeof(AuthorizeAttribute), Authorize[0].GetType());
        }
        [Fact]
        public async Task testInbox()
        {
            //setup
            SpecificApiClientViewModel compareObject = new SpecificApiClientViewModel();
            var result = await Hc.Inbox();
            //test and cast
            var Viewresult = Assert.IsType<ViewResult>(result);
            //assert
            Assert.True(compareObject.GetType() == Viewresult.Model.GetType());
        }
        [Fact]
        public async Task testCreateAccount()
        {
            //setup Tempdata
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            Hc.TempData = tempData;
            //setup two directions
            var result = (RedirectToActionResult)await Hc.CreateAccount("Fake@test.com" , "Test123!");
            var result2 = await Hc.CreateAccount(" ","Fail");
            //assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.IsType<ViewResult>(result2);
        }
        [Fact]
        public async Task testDelete()
        {
            //action
            var result = await Hc.Delete(1);
            var result2 = await Hc.Delete(9999999);
            //assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<NotFoundResult>(result2);
        }
    }
}
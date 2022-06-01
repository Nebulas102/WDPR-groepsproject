using Microsoft.AspNetCore.Identity;
using Xunit;
using System.Threading.Tasks;
using System;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity.UI.Services;
using zmdh.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace test
{
    public class SelfHelpGroupControllerTest
    {
        SelfHelpGroupController Shcc = new SelfHelpGroupController(GetInMemoryDBMetData());
        private static string databaseName; // zonder deze property kun je geen clean context maken
        private static DBManager GetInMemoryDBMetData() 
        {
            DBManager context = GetNewInMemoryDatabase(true);
            context.Add(new Chat(){Name = "ILoveHelloKitty"});
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
            var chad = await Shcc._context.Chats.FirstAsync();
            SelfHelpGroup testObject = new SelfHelpGroup(){Name = "ILoveHelloKittyFanClub", Description = "We Love Hello Kitty", Chat = chad};

            var result = await Shcc.Create(testObject);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.True(await Shcc._context.SelfHelpGroups.ContainsAsync(testObject));
        }
    }
}

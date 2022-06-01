using zmdh.Controllers;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using static zmdh.Areas.Identity.Pages.Account.RegisterModel;

namespace test
{
    public class ClientApiTest
    {
        //performance test
        [Fact]
        public async Task RequestTimeFetchClientIds()
        {
            //setup 
            DateTime start;
            DateTime end;
            //action
            start = DateTime.Now;
            await ClientApi.FetchClientIds();
            end = DateTime.Now;
            //Assert and values
            var expected = 500;//0.5 sec
            var actual = (int)(end - start).TotalMilliseconds;
            Assert.True(actual <= expected, $"Expected total milliseconds of less than or equal to {expected} but was {actual}.");
        }  
        //performance test
        [Fact]
        public async Task RequestTimePullSpecificClient()
        {
            //setup 
            DateTime start;
            DateTime end;
            //action
            start = DateTime.Now;
            await ClientApi.PullSpecificClient(10250);
            end = DateTime.Now;
            //Assert and values
            var expected = 100;//0.1 sec
            var actual = (int)(end - start).TotalMilliseconds;
            Assert.True(actual <= expected, $"Expected total milliseconds of less than or equal to {expected} but was {actual}.");
        }
        //performance test
        [Fact]
        public async Task RequestTimeForPostDelete()
        {
            //setup values
            DateTime startPost;
            DateTime endPost;
            DateTime startDelete;
            DateTime endDelete;
            DateTime startTotal;
            DateTime endTotal;
            ApiInput test = new ApiInput(){FullName = "Test",BSN = "123456789",Iban = "NL1231234567890",Birthdate = new DateTime(1999,05,20)};
            //start the clock and do actions
            startTotal= DateTime.Now;
            startPost = DateTime.Now;
            int id = await ClientApi.PostClient(test);
            endPost = DateTime.Now;
            startDelete = DateTime.Now;
            await ClientApi.DeleteClient(id);
            endDelete = DateTime.Now;
            endTotal= DateTime.Now;
            //test Post
            var expectedPost = 1500;//1.5 sec
            var actualPost = (int)(endPost - startPost).TotalMilliseconds;
            Assert.True(actualPost <= expectedPost, $"Expected total milliseconds of less than or equal to {expectedPost} but was {actualPost}.");
            //test delete
            var expectedDelete = 1500;//1.5 sec
            var actualDelete = (int)(endDelete - startDelete).TotalMilliseconds;
            Assert.True(actualDelete <= expectedDelete, $"Expected total milliseconds of less than or equal to {expectedDelete} but was {actualDelete}.");
            //test total
            var expectedTotal = 3000;//1.5 sec
            var actualTotal = (int)(endTotal - startTotal).TotalMilliseconds;
            Assert.True(actualTotal <= expectedTotal, $"Expected total milliseconds of less than or equal to {expectedTotal} but was {actualTotal}.");
        }
    }
}
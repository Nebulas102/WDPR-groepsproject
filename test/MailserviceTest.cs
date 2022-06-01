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
    public class MailserviceTest
    {
        [Fact]
        public async Task SendEmailAsyncTest()
        {
            var MoqMailSender = new Mock<IMailService>();

            await MoqMailSender.Object.SendEmailAsync(It.IsAny<MailRequest>());

            MoqMailSender.Verify(m => m.SendEmailAsync(It.IsAny<MailRequest>()), Times.Once);
        }
    }
}
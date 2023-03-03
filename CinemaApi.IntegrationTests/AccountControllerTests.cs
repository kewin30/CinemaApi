using CinemaApi.DTO.Users;
using CinemaApi.Entities;
using CinemaApi.IntegrationTests.Helpers;
using CinemaApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CinemaApi.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();
        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    ServiceDescriptor dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CinemaDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddSingleton<IAccountService>(_accountServiceMock.Object);
                    services.AddDbContext<CinemaDbContext>(options => options.UseInMemoryDatabase("CinemaDb"));
                });
            }).CreateClient();
        }
        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            RegisterUserDto user = new RegisterUserDto()
            {
                Login = "MyLogin",
                Email = "Kewin@email.com",
                Password = "Kewin123",
                ConfirmPassword = "Kewin123",
                FirstName = "Kewin",
                LastName = "Januszex"
            };
            HttpContent httpContent = user.ToJsonHttpContent();

            HttpResponseMessage response = await _client.PostAsync("api/account/register", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest(object[] userData)
        {
            string[] userDataStrings = Array.ConvertAll((object[])userData, x => x?.ToString() ?? "");


            RegisterUserDto user = new RegisterUserDto()
            {
                Login = userDataStrings[0],
                Email = userDataStrings[1],
                Password = userDataStrings[2],
                ConfirmPassword = userDataStrings[3],
                FirstName = userDataStrings[4],
                LastName = userDataStrings[5]
            };
            HttpContent httpContent = user.ToJsonHttpContent();

            HttpResponseMessage response = await _client.PostAsync("api/account/register", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("", "", "", "", "", "")]
        [InlineData(null, null, null, null, null, null)]
        [InlineData(null, "", "", "", "", "")]
        [InlineData("", null, "", "", "", "")]
        [InlineData("", "", null, "", "", "")]
        [InlineData("", "", "", null, "", "")]
        [InlineData("", "", "", "", null, "")]
        [InlineData("", "", "", "", "", null)]
        public async Task RegisterUser_ForNullArguments_ReturnsBadRequest(string param1, string param2, string param3, string param4, string param5, string param6)
        {
            RegisterUserDto user = new RegisterUserDto()
            {
                Login = param1,
                Email = param2,
                Password = param3,
                ConfirmPassword = param4,
                FirstName = param5,
                LastName = param6
            };
            HttpContent httpContent = user.ToJsonHttpContent();

            HttpResponseMessage response = await _client.PostAsync("api/account/register", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            _accountServiceMock.Setup(x => x.GenerateJwt(It.IsAny<LoginDto>())).Returns("jwt");
            LoginDto login = new LoginDto()
            {
                Email = "Test@test.com",
                Password = "testowe"
            };
            HttpContent httpContent = login.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/account/login", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }



        public static IEnumerable<object[]> InvalidData =>
            new List<object[]>
            {
        new object[] { new object[] { "", "", "", "", "", "" } },
        new object[] { new object[] { "", "jan.kowalski@example.com", "Password1", "Password1", "Jan", "Kowalski" } },
        new object[] { new object[] { "login123", "", "Password1", "Password1", "Jan", "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski-example.com", "", "Password1", "Jan", "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski.example.com", "Password1", "", "Jan", "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski,example.com", "Password1", "Password2", "Jan", "Kowalski" } },
        new object[] { new object[] { "l", "jan.kowalski@example.com", "Pass1", "Pass1", "Jan", "Kowalski" } },
        new object[] { new object[] { "log1", "jan.kowalski@example.com", "Password1", "Password1", "Jan", "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski.example.com", "Password1", "Password1", "Jan", "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski@example.com", "Password1", "Password", "", "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski@example.com", "Password1", "password1", "Jan", "" } },
        new object[] { new object[] { "login123", "jan.kowalski@example.com", "Password1", "Password1", null, "Kowalski" } },
        new object[] { new object[] { "login123", "jan.kowalski@example.com", "Password1", "Password1", "Jan", null } },
            };
    }
}

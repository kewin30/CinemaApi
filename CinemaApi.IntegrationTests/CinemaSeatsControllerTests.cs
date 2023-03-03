using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CinemaApi.Entities;
using CinemaApi.IntegrationTests.Helpers;
using System.Threading.Tasks;
using CinemaApi.DTO.CinemaSeats;
using System.Net;
using CinemaApi.Services;
using Moq;
using CinemaApi.DTO.Users;

namespace CinemaApi.IntegrationTests
{
    public class CinemaSeatsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public CinemaSeatsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    ServiceDescriptor dbContextOptions = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<CinemaDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<CinemaDbContext>(options => options.UseInMemoryDatabase("CinemaDb"));
                });
            });
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task ReserveCinemaSeat_WithValidModel_ReturnsOK()
        {
            User user = new User()
            {
                Email = "string@wp.pl",
                PasswordHash = "AQAAAAEAACcQAAAAEIp8GNN9RewVW5iueGnUBYdewpAhsu58H9fqxmhyyelnd/Dd62PzcGxLpdEyN8AizA=="
            };
            CinemaHall cinemaHall = new CinemaHall()
            {
                Name = "Sala",
                Row = 10,
                Column = 10
            };
            SeedUser(user);
            SeedCinemaHall(cinemaHall);

            ReservationOfSeat seat = new ReservationOfSeat()
            {
                Name = "Sala",
                Row = 2,
                Column = 2,
                Email = "string@wp.pl",
                Password = "string"
            };
            HttpContent httpContent = seat.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/seats", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "string@wp.pl", "password", 1, 1)]
        [InlineData("seatName", null, "password", 1, 1)]
        [InlineData("seatName", "invalid@wp.pl", "password", 1, 1)]
        [InlineData("seatName", "string@wp.pl", "invalidPassword", 1, 1)]
        [InlineData("seatName", "string@wp.pl", "password", 11, 1)]
        [InlineData("seatName", "string@wp.pl", "password", 1, 11)]
        [InlineData("Salaa", "string@wp.pl", "str1ing", 10, 11)]
        [InlineData("seatName", "string@invalid.pl", "password", 1, 1)]
        [InlineData("InvalidCinemaHallName", "string@wp.pl", "password", 10, 10)]
        [InlineData("seatName", "string@wp.pl", "string1", 11, 10)]
        [InlineData("seatName", "string@wp.pl", "password", 10, 9)]
        [InlineData("seatName", "string@wp.pl", "st1ring", 9, 11)]
        [InlineData(null,null,null, 0, 0)]

        public async Task ReserveCinemaSeat_WithInValidModel_ReturnsBadRequest(string param, string param1, string param2, int row, int column)
        {
            User user = new User()
            {
                Email = "string@wp.pl",
                PasswordHash = "AQAAAAEAACcQAAAAEIp8GNN9RewVW5iueGnUBYdewpAhsu58H9fqxmhyyelnd/Dd62PzcGxLpdEyN8AizA=="
            };
            CinemaHall cinemaHall = new CinemaHall()
            {
                Name = "Sala",
                Row = 10,
                Column = 10
            };
            SeedUser(user);
            SeedCinemaHall(cinemaHall);

            ReservationOfSeat seat = new ReservationOfSeat()
            {
                Name = param,
                Row = row,
                Column = column,
                Email = param1,
                Password = param2
            };
            HttpContent httpContent = seat.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/seats", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData("Salka","string@wp.pl","string",5,6)]
        [InlineData("sal","string@wp.pl","string",5,6)]
        public async Task ReserveCinemaSeat_WithInValidModel_ReturnsNotFound(string param, string param1, string param2, int row, int column)
        {
            User user = new User()
            {
                Email = "string@wp.pl",
                PasswordHash = "AQAAAAEAACcQAAAAEIp8GNN9RewVW5iueGnUBYdewpAhsu58H9fqxmhyyelnd/Dd62PzcGxLpdEyN8AizA=="
            };
            CinemaHall cinemaHall = new CinemaHall()
            {
                Name = "Sala",
                Row = 10,
                Column = 10
            };
            ReservationOfSeat seat = new ReservationOfSeat()
            {
                Name = param,
                Row = row,
                Column = column,
                Email = param1,
                Password = param2
            };
            HttpContent httpContent = seat.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/seats", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        private void SeedUser(User user)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            CinemaDbContext _dbContext = scope.ServiceProvider.GetService<CinemaDbContext>();
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
        }
        private void SeedCinemaHall(CinemaHall hall)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            CinemaDbContext _dbContext = scope.ServiceProvider.GetService<CinemaDbContext>();
            _dbContext.CinemaHalls.Add(hall);
            _dbContext.SaveChanges();
        }
    }
}

using CinemaApi.DTO.Cinema;
using CinemaApi.DTO.Films;
using CinemaApi.Entities;
using CinemaApi.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CinemaApi.IntegrationTests
{
    public class CinemaControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public CinemaControllerTests(WebApplicationFactory<Startup> factory)
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
        public async Task CreateCinema_WithValidModel_ReturnsOk()
        {
            string[] param = { "10", "Filharmonia kaszubska", "Gdynia" ,"10"};
            await CreateCinemaTemplate(HttpStatusCode.OK, param);
        }


        [Theory]
        [InlineData(new object[] { new string[] { "120", "Filharmonia kaszubska", "Gdynia" ,"111"} })]
        [InlineData(new object[] { new string[] { "15", "Filharmonia ", "Gdansk" ,"16"} })]
        public async void CreateCinema_WithInValidModel_ReturnsNotFound(string[] param)
        {
            await CreateCinemaTemplate(HttpStatusCode.NotFound, param);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "89", "", "Gdynia", "90" } })]
        [InlineData(new object[] { new string[] { "92", "Filharmonia ", "", "93" } })]
        [InlineData(new object[] { new string[] { "95", "", null, "97" } })]
        [InlineData(new object[] { new string[] { "155", null, "", "156" } })]
        public async void CreateCinema_WithInValidModel_ReturnsBadRequest(string[] param)
        {
            await CreateCinemaTemplate(HttpStatusCode.BadRequest, param);
        }



        private void SeedFilm(Films film)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            CinemaDbContext _dbContext = scope.ServiceProvider.GetService<CinemaDbContext>();
            _dbContext.Films.Add(film);
            _dbContext.SaveChanges();
        }
        private async Task CreateCinemaTemplate(HttpStatusCode status, string[] param)
        {
            Films film = new Films()
            {
                Id = Convert.ToInt32(param[0]),
                Name = "Avengers",
                Description = "Super nice",
                PathToImage = "xxxx",
                PathToVideo = "xxxx",
                IsRecommended = 10,
                Genre = "action",
                Actors = new List<Actors>()
                {
                    new Actors()
                    {
                        FullName = "Robert dwn jnr",
                        Age = 50,
                        Gender = "Male",
                        PlayingAs = "Iron Man"
                    }

                }
            };
            SeedFilm(film);

            CreateCinemaDto cinemaDto = new CreateCinemaDto()
            {
                Name = param[1],
                City = param[2],
                OpenTime = DateTime.Now,
                ClosingTime = DateTime.Now,
                FilmId = new List<FilmsIdDto>
                {
                    new FilmsIdDto()
                    {
                        Id = Convert.ToInt32(param[3])
                    }
                }
            };

            HttpContent httpContent = cinemaDto.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/cinema", httpContent);
            response.StatusCode.Should().Be(status);
        }

    }
}

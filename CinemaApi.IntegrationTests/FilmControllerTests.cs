using CinemaApi.DTO.Actors;
using CinemaApi.DTO.Cinema;
using CinemaApi.DTO.Director;
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
    public class FilmControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public FilmControllerTests(WebApplicationFactory<Startup> factory)
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
        public async Task CreateFilm_WithValidModel_ReturnsNoContent()
        {
            string[] param = { "Avengers", "Kang Dynasty with kang","Action", "Paul Rodd Jonathan", "Male", "Ant-Man" };
            await CreateFilmTemplate(HttpStatusCode.NoContent, param);
        }
        [Theory]
        [InlineData(new object[] { new string[] { "", "", "", "", "", "" } })]
        [InlineData(new object[] { new string[] { "Kang Dynasty with kang", null, "", "", null, "" } })]
        [InlineData(new object[] { new string[] { null, null, null, null, null, null } })]
        [InlineData(new object[] { new string[] { null, null, null, null, "Male", null } })]
        [InlineData(new object[] { new string[] { "Avengers", "Really nice video", "Action", "Robert dwn jr", "xxx", "steel man" } })]
        [InlineData(new object[] { new string[] { "Avengers", "Really nice video", "Action", "Robert dwn jr", "Female", "" } })]
        [InlineData(new object[] { new string[] { "Avengers", "Really nice video", "Action", "", "xxx", "steel man" } })]
        [InlineData(new object[] { new string[] { "Avengers", "Really nice video", "", "Robert dwn jr", "xxx", "steel man" } })]
        [InlineData(new object[] { new string[] { "Avengers", "", "Action", "Robert dwn jr", "xxx", "steel man" } })]
        [InlineData(new object[] { new string[] { "A'", "Really nice video", "Action", "Robert dwn jr", "xxx", "steel man" } })]
        public async void CreateFilm_WithInValidModel_ReturnsBadRequest(string[] param)
        {
            await CreateFilmTemplate(HttpStatusCode.BadRequest, param);
        }
        [Fact]
        public async void UpdateDirector_WithValidModel_ReturnsNoContent()
        {
            int[] param = { 1, 1 };
            await UpdateDirectorTemplate(HttpStatusCode.NoContent, param);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 50,2 } })]
        [InlineData(new object[] { new int[] { 200,20 } })]
        [InlineData(new object[] { new int[] { 40,1 } })]
        [InlineData(new object[] { new int[] { 3,0 } })]
        public async void UpdateDirector_WithInValidModel_ReturnsNotFound(int[] param)
        {
         
            await UpdateDirectorTemplate(HttpStatusCode.NotFound, param);
        }

        [Theory]
        [InlineData(new object[] { new int[] { 50, 2 } })]
        [InlineData(new object[] { new int[] { 7, 20 } })]
        [InlineData(new object[] { new int[] { 40, 1 } })]
        [InlineData(new object[] { new int[] { 3, 0 } })]
        public async void UpdateCinema_WithInValidModel_ReturnsNotFound(int[] param)
        {
            await UpdateCinemaTemplate(HttpStatusCode.NotFound, param);
        }

        [Fact]
        public async void UpdateCinema_WithValidModel_ReturnsNoContent()
        {
            int[] arr = { 3, 3 };
            await UpdateCinemaTemplate(HttpStatusCode.NoContent, arr);
        }

        private void SeedDirector(Director director)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            CinemaDbContext _dbContext = scope.ServiceProvider.GetService<CinemaDbContext>();
            _dbContext.Directors.Add(director);
            _dbContext.SaveChanges();
        }
        private void SeedCinema(Cinema cinema)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            CinemaDbContext _dbContext = scope.ServiceProvider.GetService<CinemaDbContext>();
            _dbContext.Cinemas.Add(cinema);
            _dbContext.SaveChanges();
        }
        private void SeedFilm(Films film)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            CinemaDbContext _dbContext = scope.ServiceProvider.GetService<CinemaDbContext>();
            _dbContext.Films.Add(film);
            _dbContext.SaveChanges();
        }
        private async Task UpdateDirectorTemplate(HttpStatusCode status, int[] param)
        {
            Director director = new Director()
            {
                FullName = "Jacek Tomczak",
                Age = 45,
                Gender = "Male"
            };
            SeedDirector(director);
            Films film = new Films()
            {
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
            FilmDirectorIdDto directorDto = new FilmDirectorIdDto()
            {
                DirectorId = param[0],
                FilmId = param[1]
            };
            HttpContent httpContent = directorDto.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PutAsync("api/films", httpContent);
            response.StatusCode.Should().Be(status);
        }
        private async Task UpdateCinemaTemplate(HttpStatusCode status, int[] param)
        {
            Films film = new Films()
            {
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
            Cinema cinema = new Cinema()
            {
                Name = "Filharmonia",
                City = "Gdansk",
                OpenTime = DateTime.Now,
                ClosingTime = DateTime.Now,
                Films = new List<Films>() { film }
            };
            SeedCinema(cinema);
            FilmCinemaId CinemaFilm = new FilmCinemaId()
            {
                CinemaId = param[0],
                FilmId = param[1]
            };

            HttpContent httpContent = CinemaFilm.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PutAsync("/api/films/Cinema", httpContent);

            response.StatusCode.Should().Be(status);
        }
        private async Task CreateFilmTemplate(HttpStatusCode status, string[] param)
        {
            CreateFilmDto film = new CreateFilmDto()
            {
                Name = param[0],
                Description = param[1],
                PathToImage = "XXX",
                PathToVideo = "XXX",
                IsRecommended = 9,
                Genre = param[2],
                Actors = new List<ActorsDto>()
                {
                    new ActorsDto()
                    {
                        FullName = param[3],
                        Age = 40,
                        Gender = param[4],
                        PlayingAs = param[5]
                    }
                }
            };
            HttpContent httpContent = film.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/films", httpContent);

            response.StatusCode.Should().Be(status);
        }
    }
}

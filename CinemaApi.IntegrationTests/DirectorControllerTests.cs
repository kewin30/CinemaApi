using CinemaApi.DTO.Director;
using CinemaApi.Entities;
using CinemaApi.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CinemaApi.IntegrationTests
{
    public class DirectorControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public DirectorControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        ServiceDescriptor dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CinemaDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services.AddDbContext<CinemaDbContext>(options => options.UseInMemoryDatabase("CinemaDb"));
                    });
                    
                });
            _client = _factory.CreateClient();
        }
        [Theory]
        [InlineData("Andrzej Wajda",50,"Male")]
        [InlineData("Andrzej Wajda",50,"Female")]
        [InlineData("Andrzej Wajda",11,"Male")]
        [InlineData("Testowy", 100,"Male")]
        [InlineData("Testowy", 100,"Female")]
        [InlineData("Testowy", 11,"Female")]
        [InlineData("Testowy", 11,"Male")]
        [InlineData("Testowy", 61,"Male")]
        public async Task CreateDirector_WithValidModel_ReturnsOk(string param, int param1, string param2)
        {
            DirectorDto director = new DirectorDto()
            {
                FullName = param,
                Age = param1,
                Gender = param2
            };
            HttpContent httpContent = director.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/director", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("",0,"")]
        [InlineData("",100,"")]
        [InlineData("Andrzej Wajda",100,"")]
        [InlineData("",100,"Male")]
        [InlineData("Andrzej Wajda",0,"Male")]
        [InlineData(null,null,null)]
        [InlineData("Andrzejowy",null,null)]
        [InlineData("Andrzejowy",null,"Female")]
        [InlineData(null,null,"Female")]
        [InlineData("",null,"")]
        public async Task CreateDirector_WithInValidModel_ReturnsBadRequest(string param, int param1, string param2)
        {
            DirectorDto director = new DirectorDto()
            {
                FullName = param,
                Age = param1,
                Gender = param2
            };
            HttpContent httpContent = director.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("api/director", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace CinemaApi.IntegrationTests
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly List<Type> _controllerTypes;
        private readonly WebApplicationFactory<Startup> _factory;
        public StartupTests(WebApplicationFactory<Startup> factory)
        {
            _controllerTypes = typeof(CinemaSeeder)
                .Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(Startup)))
                .ToList();
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _controllerTypes.ForEach(c => services.AddScoped(c));
                });
            });
        }

        [Fact]
        public void ConfigureServices_ForControllers_RegistersAllDependencies()
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();

            _controllerTypes.ForEach(x =>
            {
                object controller = scope.ServiceProvider.GetService(x);
                controller.Should().NotBeNull();
            });
        }

    }
}

// File: Tests/TestBase.cs
using AutoAuction_H2.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AutoAuction_H2.Tests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly ServiceProvider Provider;

        protected TestBase()
        {
            var services = new ServiceCollection();

            // 🔧 1. Register shared HttpClient for all ApiBase services
            services.AddSingleton(new HttpClient());

            // 🔧 2. Register all API-related services that depend on ApiBase
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<AuctionService>();

            // 🔧 3. Build provider
            Provider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            if (Provider is IDisposable d)
                d.Dispose();
        }
    }
}

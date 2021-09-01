using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CurrencyConvert.Infrastructure.Services;
using CurrencyConverter.Infrastructure.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Application.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var context = host.Services.GetService<ExchangeRateDbContext>();
            context.Database.EnsureCreated();
            context.Database.Migrate();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<StoreNewExchangeRatesHostedService>();
                    services.AddTransient<ICurrencyService, CurrencyService>();
                    services.AddTransient<FixerApiHandler>();
                });
    }
}

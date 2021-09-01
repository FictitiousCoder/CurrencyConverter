using System;
using System.Globalization;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConvert.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CurrencyConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            var currencyService = host.Services.GetService<ICurrencyService>();

            Console.WriteLine("--- Currency Converter ---");

            Console.WriteLine("Write the amount you wish to convert into a different currency.");
            var amount = GetCurrencyAmountFromInput();

            Console.WriteLine($"{Environment.NewLine}Write your base currency (note: only 'EUR' is currently allowed)");
            var baseCurrency = Console.ReadLine();

            Console.WriteLine($"{Environment.NewLine}Write the currency you want {baseCurrency} exchanged into.");
            var counterCurrency = Console.ReadLine();

            Console.WriteLine($"{Environment.NewLine}Converting...{Environment.NewLine}");
            var currencyPair = new CurrencyPair(amount, baseCurrency, counterCurrency);
            var convertedAmount = await ConvertBaseCurrency(currencyService, currencyPair);

            Console.WriteLine($"{currencyPair.BaseAmount} {currencyPair.BaseCurrency} = {convertedAmount} {currencyPair.CounterCurrency}");

            Console.Write($"{Environment.NewLine}Press any key to exit...");
            Console.ReadKey(true);
        }

        private static decimal GetCurrencyAmountFromInput()
        {
            decimal parsedAmount;

            while (!decimal.TryParse(Console.ReadLine(), NumberStyles.Currency, CultureInfo.InvariantCulture, out parsedAmount))
                Console.WriteLine("Invalid amount. Please type only numbers.");

            return parsedAmount;
        }

        private static async Task<decimal> ConvertBaseCurrency(ICurrencyService currencyService, CurrencyPair currencyPair)
        {
            try
            {
                return await currencyService.Convert(currencyPair);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to convert currency due to the following exception:");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddTransient<ICurrencyService, CurrencyService>()
                        .AddTransient<FixerApiHandler>());
    }
}

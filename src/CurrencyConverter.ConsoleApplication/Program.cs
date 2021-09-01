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

            Console.WriteLine(
                $"{Environment.NewLine}If you want to use the exchange rates for a specific date, enter the desired date.{Environment.NewLine}" +
                $"To use the latest rates, simply use an empty input and proceed.");
            var exchangeRateDate = GetExchangeRateDateFromInput();

            Console.WriteLine($"{Environment.NewLine}Converting...{Environment.NewLine}");
            var currencyPair = new CurrencyPair(amount, baseCurrency, counterCurrency, exchangeRateDate);
            var convertedAmount = await ConvertBaseCurrency(currencyService, currencyPair);

            if (currencyPair.ExchangeRateDate.HasValue)
                Console.WriteLine($"Using exchanges rates from {currencyPair.ExchangeRateDate.Value:dd.MM.yyyy}:");

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

        private static DateTime? GetExchangeRateDateFromInput()
        {
            DateTime? result = null;
            var skipParse = false;

            while (!skipParse && !result.HasValue)
            {
                var input = Console.ReadLine();
                skipParse = string.IsNullOrWhiteSpace(input);

                if (!DateTime.TryParse(input, out var parsedDate))
                    Console.WriteLine("Unable to parse date. Please type the date in a recognized format");
                else
                    result = parsedDate;
            }

            return result;
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

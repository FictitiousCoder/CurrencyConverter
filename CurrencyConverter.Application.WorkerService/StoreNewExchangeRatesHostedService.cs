using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConvert.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Application.WorkerService
{
    public class StoreNewExchangeRatesHostedService : IHostedService, IDisposable
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<StoreNewExchangeRatesHostedService> _logger;
        private Timer _timer;

        public StoreNewExchangeRatesHostedService(ICurrencyService currencyService,
            ILogger<StoreNewExchangeRatesHostedService> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(StoreNewExchangeRatesHostedService)} is running.");

            _timer = new Timer(StoreNewestExchangeRatesInDatabase, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void StoreNewestExchangeRatesInDatabase(object state)
        {
            _logger.LogInformation(
                $"Fetching daily currency exchange rates ({DateTimeOffset.Now})");

            var exchangeRates = await GetExchangeRates();
            await SaveExchangeRatesToDatabase(exchangeRates);

            _logger.LogInformation(
                $"Successfully fetched and saved {exchangeRates.Rates.Count} exchange rates to the database ({DateTimeOffset.Now})");
        }

        private async Task<ExchangeRatesDto> GetExchangeRates()
        {
            try
            {
                return await _currencyService.GetExchangeRates();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to convert currency due to the following exception:");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task SaveExchangeRatesToDatabase(ExchangeRatesDto exchangeRates)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(StoreNewExchangeRatesHostedService)} is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConverter.Application.Commands;
using CurrencyConverter.Application.Queries;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.Application.WorkerService
{
    public class StoreNewExchangeRatesHostedService : IHostedService, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StoreNewExchangeRatesHostedService> _logger;
        private Timer _timer;

        public StoreNewExchangeRatesHostedService(IMediator mediator, ILogger<StoreNewExchangeRatesHostedService> logger)
        {
            _mediator = mediator;
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
                return await _mediator.Send(new GetExchangeRatesQuery());
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
            var commands = exchangeRates.Rates.Select(rate =>
                new CreateExchangeRateCommand
                {
                    Currency = rate.Key,
                    Rate = rate.Value,
                    TimeStamp = exchangeRates.TimeStamp,
                });

            await _mediator.Send(new CreateExchangeRatesCommand
            {
                ExchangeRates = commands,
            });
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

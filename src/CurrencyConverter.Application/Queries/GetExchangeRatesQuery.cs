using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConvert.Infrastructure.Services;
using MediatR;

namespace CurrencyConverter.Application.Queries
{
    public class GetExchangeRatesQuery : IRequest<ExchangeRatesDto>
    {
        public string? BaseCurrency { get; set; }
        public string[]? RatesToGet { get; set; }
        public DateTime? RatesDate { get; set; }
    }

    internal class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, ExchangeRatesDto>
    {
        private readonly ICurrencyService _currencyService;

        public GetExchangeRatesQueryHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<ExchangeRatesDto> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            return await _currencyService.GetExchangeRates(request.BaseCurrency, request.RatesToGet, request.RatesDate);
        }
    }
}

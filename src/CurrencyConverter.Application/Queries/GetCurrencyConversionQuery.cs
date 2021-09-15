using System.Threading;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConvert.Infrastructure.Services;
using MediatR;

namespace CurrencyConverter.Application.Queries
{
    public class GetCurrencyConversionQuery : IRequest<decimal>
    {
        public CurrencyPair CurrencyPair { get; set; }
    }

    internal class GetCurrencyConversionQueryHandler : IRequestHandler<GetCurrencyConversionQuery, decimal>
    {
        private readonly ICurrencyService _currencyService;

        public GetCurrencyConversionQueryHandler(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<decimal> Handle(GetCurrencyConversionQuery request, CancellationToken cancellationToken)
        {
            return await _currencyService.Convert(request.CurrencyPair);
        }
    }
}

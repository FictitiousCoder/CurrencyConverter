using System;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;

namespace CurrencyConvert.Infrastructure.Services
{
    public interface ICurrencyService
    {
        Task<Decimal> Convert(CurrencyPair currencyPair);
    }
}

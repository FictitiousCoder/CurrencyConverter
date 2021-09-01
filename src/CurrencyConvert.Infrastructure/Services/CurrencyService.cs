using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;

namespace CurrencyConvert.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private const string FixerApiUrl = "http://data.fixer.io/api/";
        private const string DefaultBaseCurrency = "EUR";

        private readonly HttpClient _httpClient;

        public CurrencyService(FixerApiHandler fixerApiHandler)
        {
            _httpClient = new HttpClient(fixerApiHandler)
            {
                BaseAddress = new Uri(FixerApiUrl),
            };
        }

        public async Task<Decimal> Convert(CurrencyPair currencyPair)
        {
            var exchangeRates = await GetExchangeRates(currencyPair.BaseCurrency, new []{ currencyPair.CounterCurrency }, currencyPair.ExchangeRateDate);
            var exchangeRate = GetExchangeRateForCurrency(exchangeRates, currencyPair.CounterCurrency);
            return currencyPair.BaseAmount * exchangeRate;
        }

        public async Task<ExchangeRatesDto> GetExchangeRates(string baseCurrency = DefaultBaseCurrency, string[] ratesToGet = null, DateTime? ratesDate = null)
        {
            var request = (ratesDate.HasValue ? ratesDate.Value.ToString("yyyy-MM-dd") : "latest") +
                          $"?base={baseCurrency}"
                          + (ratesToGet != null && ratesToGet.Length > 0
                              ? "&rates=" + string.Join(',', ratesToGet)
                              : string.Empty);

            using (var response = await _httpClient.GetAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.ReasonPhrase);

                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExchangeRatesDto>(result, GetSerializerOptions());
            }
        }

        private Decimal GetExchangeRateForCurrency(ExchangeRatesDto rates, string currency)
        {
            if (!rates.Rates.ContainsKey(currency))
                throw new ArgumentException($"{currency} is not not a recognized currency.");

            return rates.Rates[currency];
        }

        private JsonSerializerOptions GetSerializerOptions()
            => new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
    }
}

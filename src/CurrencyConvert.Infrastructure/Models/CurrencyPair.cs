using System;

namespace CurrencyConvert.Infrastructure.Models
{
    public class CurrencyPair
    {
        public decimal BaseAmount { get; set; }
        public string BaseCurrency { get; set; }
        public string CounterCurrency { get; set; }
        public DateTime? ExchangeRateDate { get; set; }

        public CurrencyPair(decimal baseAmount, string baseCurrency, string counterCurrency, DateTime? exchangeRateDate= null)
        {
            BaseAmount = baseAmount;
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            ExchangeRateDate = exchangeRateDate;
        }
    }
}

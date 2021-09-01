namespace CurrencyConvert.Infrastructure.Models
{
    public class CurrencyPair
    {
        public decimal BaseAmount { get; set; }
        public string BaseCurrency { get; set; }
        public string CounterCurrency { get; set; }

        public CurrencyPair(decimal baseAmount, string baseCurrency, string counterCurrency)
        {
            BaseAmount = baseAmount;
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
        }
    }
}

using System;

namespace CurrencyConverter.Domain.Entities
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}

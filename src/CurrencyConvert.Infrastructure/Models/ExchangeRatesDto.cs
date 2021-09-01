using System;
using System.Collections.Generic;

namespace CurrencyConvert.Infrastructure.Models
{
    public class ExchangeRatesDto
    {
        public DateTimeOffset TimeStamp { get; set; }
        public string Base{ get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}

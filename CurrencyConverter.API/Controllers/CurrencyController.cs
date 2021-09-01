using System;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConvert.Infrastructure.Services;
using CurrencyConverter.Infrastructure.RelationalStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ExchangeRateDbContext _exchangeRateDbContext;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrencyService currencyService, ExchangeRateDbContext exchangeRateDbContext, ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _exchangeRateDbContext = exchangeRateDbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] decimal baseAmount,
            [FromQuery] string baseCurrency, [FromQuery] string counterCurrency, [FromQuery] DateTime? date)
        {
            if (baseAmount == decimal.Zero)
                return BadRequest($"{nameof(baseAmount)} must be included in the request, but was not found.");

            if (string.IsNullOrWhiteSpace(baseCurrency))
                return BadRequest($"{nameof(baseCurrency)} must be included in the request, but was not found.");

            if (string.IsNullOrWhiteSpace(counterCurrency))
                return BadRequest($"{nameof(counterCurrency)} must be included in the request, but was not found.");

            try
            {
                var result = await _currencyService.Convert(new CurrencyPair(baseAmount, baseCurrency, counterCurrency, date));
                return Ok(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                _logger.LogError(exception, exception.Message);
                return BadRequest(exception.Message);
            }
        }
    }
}

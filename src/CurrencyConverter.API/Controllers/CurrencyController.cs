using System;
using System.Threading.Tasks;
using CurrencyConvert.Infrastructure.Models;
using CurrencyConverter.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CurrencyConverter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(IMediator mediator, ILogger<CurrencyController> logger)
        {
            _mediator = mediator;
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
                var result = await _mediator.Send(new GetCurrencyConversionQuery
                {
                    CurrencyPair = new CurrencyPair(baseAmount, baseCurrency, counterCurrency, date)
                });
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

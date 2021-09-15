using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConverter.Domain.Entities;
using CurrencyConverter.Infrastructure.RelationalStorage;
using MediatR;

namespace CurrencyConverter.Application.Commands
{
    public class CreateExchangeRatesCommand : IRequest<IEnumerable<int>>
    {
        public IEnumerable<CreateExchangeRateCommand> ExchangeRates { get; set; }
    }

    internal class CreateExchangeRatesCommandHandler : IRequestHandler<CreateExchangeRatesCommand, IEnumerable<int>>
    {
        private readonly ExchangeRateDbContext _context;

        public CreateExchangeRatesCommandHandler(ExchangeRateDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> Handle(CreateExchangeRatesCommand request, CancellationToken cancellationToken)
        {
            var entities = request.ExchangeRates.Select(rate => new ExchangeRate
            {
                Rate = rate.Rate,
                Currency = rate.Currency,
                TimeStamp = rate.TimeStamp,
            });

            await _context.AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entities.Select(e => e.Id);

        }
    }
}

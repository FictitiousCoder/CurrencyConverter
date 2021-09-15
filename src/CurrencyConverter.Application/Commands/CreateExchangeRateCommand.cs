using System;
using System.Threading;
using System.Threading.Tasks;
using CurrencyConverter.Domain.Entities;
using CurrencyConverter.Infrastructure.RelationalStorage;
using MediatR;

namespace CurrencyConverter.Application.Commands
{
    public class CreateExchangeRateCommand : IRequest<int>
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }

    internal class CreateExchangeRateCommandHandler : IRequestHandler<CreateExchangeRateCommand, int>
    {
        private readonly ExchangeRateDbContext _context;

        public CreateExchangeRateCommandHandler(ExchangeRateDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var entity = new ExchangeRate
            {
                Rate = request.Rate,
                Currency = request.Currency,
                TimeStamp = request.TimeStamp,
            };

            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;

        }
    }
}

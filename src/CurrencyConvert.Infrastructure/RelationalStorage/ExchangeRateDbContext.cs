using System;
using CurrencyConverter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Infrastructure.RelationalStorage
{
    public class ExchangeRateDbContext : DbContext
    {
        public DbSet<ExchangeRate> Rates { get; set; }

        public string DbPath { get; private set; }

        public ExchangeRateDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}exchangeRates.db";
        }


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}

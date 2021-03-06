// <auto-generated />
using CurrencyConverter.Infrastructure.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CurrencyConvert.Infrastructure.Migrations
{
    [DbContext(typeof(ExchangeRateDbContext))]
    partial class ExchangeRateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("CurrencyConverter.Domain.Entities.ExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Currency")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Rate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Rates");
                });
#pragma warning restore 612, 618
        }
    }
}

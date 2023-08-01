using System;
using Microsoft.EntityFrameworkCore;
using AWS_Referential.Implementation;


namespace AWS_Referential.DataBase
{
    public  class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Instrument> InstrumentTable { get; set; }
        public DbSet<Stock> StockTable { get; set; }
        public DbSet<Dividend> DividendTable { get; set; }
        public DbSet<Listing> ListingTable { get; set; }
        public DbSet<BasketPriceComposition> BasketPriceCompositionTable { get; set; }
        public DbSet<BasketPrice> BasketPriceTable { get; set; }
        public DbSet<BasketPriceComponent> BasketPriceComponentTable { get; set; }
        public DbSet<Modification> ModificationTable { get; set; }
        public DbSet<DividendError> DividendErrorTable { get; set; }
        public DbSet<Derivative> DerivativeTable { get; set; }
        public DbSet<Price> PriceTable { get; set; }
        public DbSet<EarningEstimate> EarningEstimateTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;user=admin;pwd=MpMf7kmtCD;database=Referential");
                //optionsBuilder.UseLazyLoadingProxies();


            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //To check to remove on cascade deleting
            //var cascadeFKs = modelBuilder.Model.GetEntityTypes()
        //.SelectMany(t => t.GetForeignKeys())
        //.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

          //  foreach (var fk in cascadeFKs)
          //      fk.DeleteBehavior = DeleteBehavior.Restrict;

            //base.OnModelCreating(modelBuilder);
            //
        }


    }
}

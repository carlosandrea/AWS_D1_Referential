﻿// <auto-generated />
using System;
using AWS_Referential.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AWS_Referential.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20220729135009_FirstTest")]
    partial class FirstTest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AWS_Referential.Implementation.BasketPriceComponent", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BasketPriceCompositionId")
                        .HasColumnType("int");

                    b.Property<decimal>("FreeFloatAdjustmentFactor")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("InstrumentId")
                        .HasColumnType("int");

                    b.Property<decimal>("OtherAdjustmentFactor1")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("OtherAdjustmentFactor2")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("PriceAdjustmentFactor")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Units")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("UniqueId");

                    b.HasIndex("BasketPriceCompositionId");

                    b.HasIndex("InstrumentId");

                    b.ToTable("BasketPriceComponentTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.BasketPriceComposition", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BasketPriceId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CompositionEndDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CompositionStartDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Divisor")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("UniqueId");

                    b.HasIndex("BasketPriceId");

                    b.ToTable("BasketPriceCompositionTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Dividend", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("AgmDate")
                        .HasColumnType("datetime");

                    b.Property<int>("AgmStatus")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrencyMultiplier")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("DividendCurrency")
                        .HasColumnType("int");

                    b.Property<int>("DividendForm")
                        .HasColumnType("int");

                    b.Property<int>("DividendSource")
                        .HasColumnType("int");

                    b.Property<int>("DividendStatus")
                        .HasColumnType("int");

                    b.Property<int>("DividendType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("FxRate")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("GrossAmount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("InstrumentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsScriptOptionnal")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime");

                    b.Property<string>("MarkitId")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<string>("OtherEstimates")
                        .HasColumnType("text");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("StockUniqueId")
                        .HasColumnType("int");

                    b.Property<int>("TaxCode")
                        .HasColumnType("int");

                    b.Property<int>("TaxJuridiction")
                        .HasColumnType("int");

                    b.HasKey("UniqueId");

                    b.HasIndex("InstrumentId");

                    b.HasIndex("StockUniqueId");

                    b.ToTable("DividendTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.DividendError", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("InsertionDate")
                        .HasColumnType("datetime");

                    b.Property<string>("MarkitId")
                        .HasColumnType("text");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<string>("Ticker")
                        .HasColumnType("text");

                    b.HasKey("UniqueId");

                    b.ToTable("DividendErrorTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.EarningEstimate", b =>
                {
                    b.Property<int>("UniqueId")
                        .HasColumnType("int");

                    b.Property<double?>("Asset_TurnOver")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_EBIT")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_EBITDA")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_EPS")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_ESTIMATE_FCF")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_Gross_Margin")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_ROA")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_TARGET_MEDIAN")
                        .HasColumnType("double");

                    b.Property<double?>("BEST_TARGET_PRICE")
                        .HasColumnType("double");

                    b.Property<double?>("CF_From_Operation")
                        .HasColumnType("double");

                    b.Property<double?>("DPS_To_EPS")
                        .HasColumnType("double");

                    b.Property<double?>("Dividend_Per_Share")
                        .HasColumnType("double");

                    b.Property<double?>("EBIT")
                        .HasColumnType("double");

                    b.Property<double?>("EBITDA")
                        .HasColumnType("double");

                    b.Property<double?>("EquityShares_Outstanding")
                        .HasColumnType("double");

                    b.Property<double?>("FREE_CASH_FLOW_YIELD")
                        .HasColumnType("double");

                    b.Property<double?>("Financial_Leverage")
                        .HasColumnType("double");

                    b.Property<double?>("Gross_Margin")
                        .HasColumnType("double");

                    b.Property<double?>("IS_EPS")
                        .HasColumnType("double");

                    b.Property<double?>("LTDEBT_To_TotalAsset")
                        .HasColumnType("double");

                    b.Property<DateTime>("Last_Update")
                        .HasColumnType("datetime");

                    b.Property<double?>("Net_Income")
                        .HasColumnType("double");

                    b.Property<double?>("PE_To_LTGR")
                        .HasColumnType("double");

                    b.Property<double?>("Price_Earning")
                        .HasColumnType("double");

                    b.Property<double?>("Price_To_Book")
                        .HasColumnType("double");

                    b.Property<double?>("Price_To_CF")
                        .HasColumnType("double");

                    b.Property<double?>("Price_To_Sale")
                        .HasColumnType("double");

                    b.Property<double?>("ROA")
                        .HasColumnType("double");

                    b.Property<double?>("ROE")
                        .HasColumnType("double");

                    b.Property<double?>("Sales")
                        .HasColumnType("double");

                    b.Property<double?>("Total_Asset")
                        .HasColumnType("double");

                    b.HasKey("UniqueId");

                    b.ToTable("EarningEstimateTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Instrument", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Cusip")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("DesactivationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Figi")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Isin")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("UniqueId");

                    b.ToTable("InstrumentTable");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Instrument");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Listing", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ActivationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("BloombergCode")
                        .HasColumnType("text");

                    b.Property<int>("CurrencyCode")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrencyMultiplier")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Cusip")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DesactivationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("InstrumentId")
                        .HasColumnType("int");

                    b.Property<string>("Mic")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryMic")
                        .HasColumnType("text");

                    b.Property<string>("Ric")
                        .HasColumnType("text");

                    b.Property<string>("Sedol")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("StockUniqueId")
                        .HasColumnType("int");

                    b.HasKey("UniqueId");

                    b.HasIndex("InstrumentId");

                    b.HasIndex("StockUniqueId");

                    b.ToTable("ListingTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Modification", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BasketPriceUniqueId")
                        .HasColumnType("int");

                    b.Property<int?>("DerivativeUniqueId")
                        .HasColumnType("int");

                    b.Property<int?>("DividendUniqueId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime");

                    b.Property<string>("ModifiedField")
                        .HasColumnType("text");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<int>("ObjectModification")
                        .HasColumnType("int");

                    b.Property<int>("ObjectModifiedId")
                        .HasColumnType("int");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.HasKey("UniqueId");

                    b.HasIndex("BasketPriceUniqueId");

                    b.HasIndex("DerivativeUniqueId");

                    b.HasIndex("DividendUniqueId");

                    b.ToTable("ModificationTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Price", b =>
                {
                    b.Property<int>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("ListingId")
                        .HasColumnType("int");

                    b.Property<int>("PriceType")
                        .HasColumnType("int");

                    b.Property<double?>("Quantity")
                        .HasColumnType("double");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<double>("Value")
                        .HasColumnType("double");

                    b.HasKey("UniqueId");

                    b.HasIndex("ListingId");

                    b.ToTable("PriceTable");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.BasketPrice", b =>
                {
                    b.HasBaseType("AWS_Referential.Implementation.Instrument");

                    b.Property<int>("IndexReturnType")
                        .HasColumnType("int");

                    b.Property<int?>("ListingUniqueId")
                        .HasColumnType("int");

                    b.HasIndex("ListingUniqueId");

                    b.HasDiscriminator().HasValue("BasketPrice");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Derivative", b =>
                {
                    b.HasBaseType("AWS_Referential.Implementation.Instrument");

                    b.Property<int>("DerivativeType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ListingUniqueId")
                        .HasColumnName("Derivative_ListingUniqueId")
                        .HasColumnType("int");

                    b.Property<double>("PointValue")
                        .HasColumnType("double");

                    b.Property<int?>("UnderlyingUniqueId")
                        .HasColumnType("int");

                    b.HasIndex("ListingUniqueId");

                    b.HasIndex("UnderlyingUniqueId");

                    b.HasDiscriminator().HasValue("Derivative");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Stock", b =>
                {
                    b.HasBaseType("AWS_Referential.Implementation.Instrument");

                    b.HasDiscriminator().HasValue("Stock");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.BasketPriceComponent", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.BasketPriceComposition", "BasketPriceComposition")
                        .WithMany("Composition")
                        .HasForeignKey("BasketPriceCompositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AWS_Referential.Implementation.Instrument", "Instrument")
                        .WithMany()
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AWS_Referential.Implementation.BasketPriceComposition", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.BasketPrice", "BasketPrice")
                        .WithMany("PriceComposition")
                        .HasForeignKey("BasketPriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Dividend", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.Instrument", "Instrument")
                        .WithMany()
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AWS_Referential.Implementation.Stock", null)
                        .WithMany("Dividends")
                        .HasForeignKey("StockUniqueId");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.EarningEstimate", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("UniqueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Listing", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.Instrument", "Instrument")
                        .WithMany()
                        .HasForeignKey("InstrumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AWS_Referential.Implementation.Stock", null)
                        .WithMany("Listings")
                        .HasForeignKey("StockUniqueId");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Modification", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.BasketPrice", null)
                        .WithMany("Modifications")
                        .HasForeignKey("BasketPriceUniqueId");

                    b.HasOne("AWS_Referential.Implementation.Derivative", null)
                        .WithMany("Modifications")
                        .HasForeignKey("DerivativeUniqueId");

                    b.HasOne("AWS_Referential.Implementation.Dividend", null)
                        .WithMany("Modifications")
                        .HasForeignKey("DividendUniqueId");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Price", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AWS_Referential.Implementation.BasketPrice", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingUniqueId");
                });

            modelBuilder.Entity("AWS_Referential.Implementation.Derivative", b =>
                {
                    b.HasOne("AWS_Referential.Implementation.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingUniqueId");

                    b.HasOne("AWS_Referential.Implementation.Instrument", "Underlying")
                        .WithMany()
                        .HasForeignKey("UnderlyingUniqueId");
                });
#pragma warning restore 612, 618
        }
    }
}

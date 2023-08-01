using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace AWS_Referential.Migrations
{
    public partial class FirstTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DividendErrorTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MarkitId = table.Column<string>(nullable: true),
                    Ticker = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    InsertionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendErrorTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "BasketPriceComponentTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BasketPriceCompositionId = table.Column<int>(nullable: false),
                    InstrumentId = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false),
                    PriceAdjustmentFactor = table.Column<decimal>(nullable: false),
                    Units = table.Column<decimal>(nullable: false),
                    FreeFloatAdjustmentFactor = table.Column<decimal>(nullable: false),
                    OtherAdjustmentFactor1 = table.Column<decimal>(nullable: false),
                    OtherAdjustmentFactor2 = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketPriceComponentTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "ModificationTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ObjectModification = table.Column<int>(nullable: false),
                    ObjectModifiedId = table.Column<int>(nullable: false),
                    ModifiedField = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    BasketPriceUniqueId = table.Column<int>(nullable: true),
                    DerivativeUniqueId = table.Column<int>(nullable: true),
                    DividendUniqueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModificationTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "BasketPriceCompositionTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BasketPriceId = table.Column<int>(nullable: false),
                    CompositionStartDate = table.Column<DateTime>(nullable: false),
                    CompositionEndDate = table.Column<DateTime>(nullable: true),
                    Divisor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketPriceCompositionTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "DividendTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    InstrumentId = table.Column<int>(nullable: false),
                    DividendCurrency = table.Column<int>(nullable: false),
                    CurrencyMultiplier = table.Column<decimal>(nullable: false),
                    FxRate = table.Column<decimal>(nullable: false),
                    GrossAmount = table.Column<decimal>(nullable: false),
                    DividendSource = table.Column<int>(nullable: false),
                    DividendForm = table.Column<int>(nullable: false),
                    DividendStatus = table.Column<int>(nullable: false),
                    ExDate = table.Column<DateTime>(nullable: false),
                    PayDate = table.Column<DateTime>(nullable: false),
                    RecordDate = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    AgmDate = table.Column<DateTime>(nullable: true),
                    AgmStatus = table.Column<int>(nullable: false),
                    DividendType = table.Column<int>(nullable: false),
                    IsScriptOptionnal = table.Column<bool>(nullable: false),
                    TaxCode = table.Column<int>(nullable: false),
                    TaxJuridiction = table.Column<int>(nullable: false),
                    MarkitId = table.Column<string>(maxLength: 100, nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    OtherEstimates = table.Column<string>(nullable: true),
                    StockUniqueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "EarningEstimateTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false),
                    BEST_EPS = table.Column<double>(nullable: true),
                    IS_EPS = table.Column<double>(nullable: true),
                    BEST_TARGET_PRICE = table.Column<double>(nullable: true),
                    BEST_TARGET_MEDIAN = table.Column<double>(nullable: true),
                    BEST_EBIT = table.Column<double>(nullable: true),
                    EBIT = table.Column<double>(nullable: true),
                    EBITDA = table.Column<double>(nullable: true),
                    BEST_EBITDA = table.Column<double>(nullable: true),
                    FREE_CASH_FLOW_YIELD = table.Column<double>(nullable: true),
                    BEST_ESTIMATE_FCF = table.Column<double>(nullable: true),
                    Price_Earning = table.Column<double>(nullable: true),
                    Price_To_Book = table.Column<double>(nullable: true),
                    Price_To_Sale = table.Column<double>(nullable: true),
                    Price_To_CF = table.Column<double>(nullable: true),
                    PE_To_LTGR = table.Column<double>(nullable: true),
                    DPS_To_EPS = table.Column<double>(nullable: true),
                    Dividend_Per_Share = table.Column<double>(nullable: true),
                    Sales = table.Column<double>(nullable: true),
                    Net_Income = table.Column<double>(nullable: true),
                    ROE = table.Column<double>(nullable: true),
                    BEST_ROA = table.Column<double>(nullable: true),
                    BEST_Gross_Margin = table.Column<double>(nullable: true),
                    CF_From_Operation = table.Column<double>(nullable: true),
                    ROA = table.Column<double>(nullable: true),
                    Total_Asset = table.Column<double>(nullable: true),
                    Financial_Leverage = table.Column<double>(nullable: true),
                    LTDEBT_To_TotalAsset = table.Column<double>(nullable: true),
                    EquityShares_Outstanding = table.Column<double>(nullable: true),
                    Gross_Margin = table.Column<double>(nullable: true),
                    Asset_TurnOver = table.Column<double>(nullable: true),
                    Last_Update = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarningEstimateTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "ListingTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    InstrumentId = table.Column<int>(nullable: false),
                    Ric = table.Column<string>(nullable: true),
                    Mic = table.Column<string>(nullable: true),
                    PrimaryMic = table.Column<string>(nullable: true),
                    BloombergCode = table.Column<string>(nullable: true),
                    Sedol = table.Column<string>(maxLength: 100, nullable: true),
                    Cusip = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<int>(nullable: false),
                    CurrencyMultiplier = table.Column<decimal>(nullable: false),
                    ActivationDate = table.Column<DateTime>(nullable: false),
                    DesactivationDate = table.Column<DateTime>(nullable: true),
                    StockUniqueId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingTable", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Isin = table.Column<string>(maxLength: 100, nullable: true),
                    Figi = table.Column<string>(maxLength: 100, nullable: true),
                    Cusip = table.Column<string>(maxLength: 100, nullable: true),
                    ActivationDate = table.Column<DateTime>(nullable: false),
                    DesactivationDate = table.Column<DateTime>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ListingUniqueId = table.Column<int>(nullable: true),
                    IndexReturnType = table.Column<int>(nullable: true),
                    Derivative_ListingUniqueId = table.Column<int>(nullable: true),
                    UnderlyingUniqueId = table.Column<int>(nullable: true),
                    DerivativeType = table.Column<int>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    PointValue = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentTable", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_InstrumentTable_ListingTable_ListingUniqueId",
                        column: x => x.ListingUniqueId,
                        principalTable: "ListingTable",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstrumentTable_ListingTable_Derivative_ListingUniqueId",
                        column: x => x.Derivative_ListingUniqueId,
                        principalTable: "ListingTable",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstrumentTable_InstrumentTable_UnderlyingUniqueId",
                        column: x => x.UnderlyingUniqueId,
                        principalTable: "InstrumentTable",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceTable",
                columns: table => new
                {
                    UniqueId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ListingId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    PriceType = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    Quantity = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTable", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_PriceTable_ListingTable_ListingId",
                        column: x => x.ListingId,
                        principalTable: "ListingTable",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketPriceComponentTable_BasketPriceCompositionId",
                table: "BasketPriceComponentTable",
                column: "BasketPriceCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketPriceComponentTable_InstrumentId",
                table: "BasketPriceComponentTable",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketPriceCompositionTable_BasketPriceId",
                table: "BasketPriceCompositionTable",
                column: "BasketPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_DividendTable_InstrumentId",
                table: "DividendTable",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DividendTable_StockUniqueId",
                table: "DividendTable",
                column: "StockUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentTable_ListingUniqueId",
                table: "InstrumentTable",
                column: "ListingUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentTable_Derivative_ListingUniqueId",
                table: "InstrumentTable",
                column: "Derivative_ListingUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentTable_UnderlyingUniqueId",
                table: "InstrumentTable",
                column: "UnderlyingUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingTable_InstrumentId",
                table: "ListingTable",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingTable_StockUniqueId",
                table: "ListingTable",
                column: "StockUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ModificationTable_BasketPriceUniqueId",
                table: "ModificationTable",
                column: "BasketPriceUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ModificationTable_DerivativeUniqueId",
                table: "ModificationTable",
                column: "DerivativeUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ModificationTable_DividendUniqueId",
                table: "ModificationTable",
                column: "DividendUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTable_ListingId",
                table: "PriceTable",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketPriceComponentTable_BasketPriceCompositionTable_Basket~",
                table: "BasketPriceComponentTable",
                column: "BasketPriceCompositionId",
                principalTable: "BasketPriceCompositionTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketPriceComponentTable_InstrumentTable_InstrumentId",
                table: "BasketPriceComponentTable",
                column: "InstrumentId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModificationTable_InstrumentTable_BasketPriceUniqueId",
                table: "ModificationTable",
                column: "BasketPriceUniqueId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModificationTable_InstrumentTable_DerivativeUniqueId",
                table: "ModificationTable",
                column: "DerivativeUniqueId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModificationTable_DividendTable_DividendUniqueId",
                table: "ModificationTable",
                column: "DividendUniqueId",
                principalTable: "DividendTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketPriceCompositionTable_InstrumentTable_BasketPriceId",
                table: "BasketPriceCompositionTable",
                column: "BasketPriceId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DividendTable_InstrumentTable_InstrumentId",
                table: "DividendTable",
                column: "InstrumentId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DividendTable_InstrumentTable_StockUniqueId",
                table: "DividendTable",
                column: "StockUniqueId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EarningEstimateTable_InstrumentTable_UniqueId",
                table: "EarningEstimateTable",
                column: "UniqueId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingTable_InstrumentTable_InstrumentId",
                table: "ListingTable",
                column: "InstrumentId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingTable_InstrumentTable_StockUniqueId",
                table: "ListingTable",
                column: "StockUniqueId",
                principalTable: "InstrumentTable",
                principalColumn: "UniqueId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingTable_InstrumentTable_InstrumentId",
                table: "ListingTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingTable_InstrumentTable_StockUniqueId",
                table: "ListingTable");

            migrationBuilder.DropTable(
                name: "BasketPriceComponentTable");

            migrationBuilder.DropTable(
                name: "DividendErrorTable");

            migrationBuilder.DropTable(
                name: "EarningEstimateTable");

            migrationBuilder.DropTable(
                name: "ModificationTable");

            migrationBuilder.DropTable(
                name: "PriceTable");

            migrationBuilder.DropTable(
                name: "BasketPriceCompositionTable");

            migrationBuilder.DropTable(
                name: "DividendTable");

            migrationBuilder.DropTable(
                name: "InstrumentTable");

            migrationBuilder.DropTable(
                name: "ListingTable");
        }
    }
}

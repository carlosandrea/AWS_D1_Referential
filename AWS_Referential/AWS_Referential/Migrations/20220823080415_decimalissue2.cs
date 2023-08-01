using Microsoft.EntityFrameworkCore.Migrations;

namespace AWS_Referential.Migrations
{
    public partial class decimalissue2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CurrencyMultiplier",
                table: "ListingTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossAmount",
                table: "DividendTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FxRate",
                table: "DividendTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrencyMultiplier",
                table: "DividendTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Divisor",
                table: "BasketPriceCompositionTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Units",
                table: "BasketPriceComponentTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CurrencyMultiplier",
                table: "ListingTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossAmount",
                table: "DividendTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FxRate",
                table: "DividendTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrencyMultiplier",
                table: "DividendTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Divisor",
                table: "BasketPriceCompositionTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Units",
                table: "BasketPriceComponentTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");
        }
    }
}

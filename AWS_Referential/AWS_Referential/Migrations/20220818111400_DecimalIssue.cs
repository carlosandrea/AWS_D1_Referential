using Microsoft.EntityFrameworkCore.Migrations;

namespace AWS_Referential.Migrations
{
    public partial class DecimalIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAdjustmentFactor",
                table: "BasketPriceComponentTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OtherAdjustmentFactor2",
                table: "BasketPriceComponentTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OtherAdjustmentFactor1",
                table: "BasketPriceComponentTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeFloatAdjustmentFactor",
                table: "BasketPriceComponentTable",
                type: "decimal(38,18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAdjustmentFactor",
                table: "BasketPriceComponentTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OtherAdjustmentFactor2",
                table: "BasketPriceComponentTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OtherAdjustmentFactor1",
                table: "BasketPriceComponentTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FreeFloatAdjustmentFactor",
                table: "BasketPriceComponentTable",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38,18)");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Darts.Api.Data.Migrations
{
    public partial class NullableStatsLegs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WinLegs",
                table: "PlayerStats",
                type: "decimal(13,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LooseLegs",
                table: "PlayerStats",
                type: "decimal(13,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WinLegs",
                table: "PlayerStats",
                type: "decimal(13,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LooseLegs",
                table: "PlayerStats",
                type: "decimal(13,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,4)",
                oldNullable: true);
        }
    }
}

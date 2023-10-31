using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class INDHNEW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "INDHS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "INDHS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlateformeGestionnaire",
                table: "INDHS",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "ClientFinances",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "INDHS");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "INDHS");

            migrationBuilder.DropColumn(
                name: "PlateformeGestionnaire",
                table: "INDHS");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "ClientFinances");
        }
    }
}

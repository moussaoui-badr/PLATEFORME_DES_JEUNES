using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class fonctionnement_financement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateBudget",
                table: "Fonctionnements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmetteurBudget",
                table: "Fonctionnements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MontantBudget",
                table: "Fonctionnements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Financements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateBudget",
                table: "Financements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Emeteur",
                table: "Financements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmetteurBudget",
                table: "Financements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MontantBudget",
                table: "Financements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBudget",
                table: "Fonctionnements");

            migrationBuilder.DropColumn(
                name: "EmetteurBudget",
                table: "Fonctionnements");

            migrationBuilder.DropColumn(
                name: "MontantBudget",
                table: "Fonctionnements");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Financements");

            migrationBuilder.DropColumn(
                name: "DateBudget",
                table: "Financements");

            migrationBuilder.DropColumn(
                name: "Emeteur",
                table: "Financements");

            migrationBuilder.DropColumn(
                name: "EmetteurBudget",
                table: "Financements");

            migrationBuilder.DropColumn(
                name: "MontantBudget",
                table: "Financements");
        }
    }
}

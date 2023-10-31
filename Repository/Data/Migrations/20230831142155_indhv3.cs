using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class indhv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MontantDevis",
                table: "INDHS",
                newName: "PartIndh");

            migrationBuilder.RenameColumn(
                name: "Montant",
                table: "INDHS",
                newName: "MontantAcquisition");

            migrationBuilder.AddColumn<double>(
                name: "ApportEnAmenagement",
                table: "INDHS",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ApportEnDhs",
                table: "INDHS",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Autre",
                table: "INDHS",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApportEnAmenagement",
                table: "INDHS");

            migrationBuilder.DropColumn(
                name: "ApportEnDhs",
                table: "INDHS");

            migrationBuilder.DropColumn(
                name: "Autre",
                table: "INDHS");

            migrationBuilder.RenameColumn(
                name: "PartIndh",
                table: "INDHS",
                newName: "MontantDevis");

            migrationBuilder.RenameColumn(
                name: "MontantAcquisition",
                table: "INDHS",
                newName: "Montant");
        }
    }
}

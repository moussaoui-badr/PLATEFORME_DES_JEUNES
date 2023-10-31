using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class pub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InscriptionFormation_ClientsPublic_ClientPublicID",
                table: "InscriptionFormation");

            migrationBuilder.DropIndex(
                name: "IX_InscriptionFormation_ClientPublicID",
                table: "InscriptionFormation");

            migrationBuilder.DropColumn(
                name: "ClientPublicID",
                table: "InscriptionFormation");

            migrationBuilder.AddColumn<bool>(
                name: "Public",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Public",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "ClientPublicID",
                table: "InscriptionFormation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InscriptionFormation_ClientPublicID",
                table: "InscriptionFormation",
                column: "ClientPublicID");

            migrationBuilder.AddForeignKey(
                name: "FK_InscriptionFormation_ClientsPublic_ClientPublicID",
                table: "InscriptionFormation",
                column: "ClientPublicID",
                principalTable: "ClientsPublic",
                principalColumn: "ClientPublicID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

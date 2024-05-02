using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class responsable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Personnes_ResponsableFamilleId",
                table: "Personnes",
                column: "ResponsableFamilleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnes_Personnes_ResponsableFamilleId",
                table: "Personnes",
                column: "ResponsableFamilleId",
                principalTable: "Personnes",
                principalColumn: "PersonneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnes_Personnes_ResponsableFamilleId",
                table: "Personnes");

            migrationBuilder.DropIndex(
                name: "IX_Personnes_ResponsableFamilleId",
                table: "Personnes");
        }
    }
}

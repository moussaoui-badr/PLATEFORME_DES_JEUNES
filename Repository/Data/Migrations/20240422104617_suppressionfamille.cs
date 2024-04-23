using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class suppressionfamille : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnes_Familles_FamilleId",
                table: "Personnes");

            migrationBuilder.DropTable(
                name: "Familles");

            migrationBuilder.DropIndex(
                name: "IX_Personnes_FamilleId",
                table: "Personnes");

            migrationBuilder.DropColumn(
                name: "FamilleId",
                table: "Personnes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FamilleId",
                table: "Personnes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Familles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_FamilleId",
                table: "Personnes",
                column: "FamilleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnes_Familles_FamilleId",
                table: "Personnes",
                column: "FamilleId",
                principalTable: "Familles",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class Election : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontantAgio",
                table: "Fonctionnements");

            migrationBuilder.DropColumn(
                name: "MontantAgio",
                table: "Financements");

            migrationBuilder.CreateTable(
                name: "Familles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Secteurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secteurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypesRelationParente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesRelationParente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personnes",
                columns: table => new
                {
                    PersonneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GSM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecteurId = table.Column<int>(type: "int", nullable: true),
                    RelationParenteId = table.Column<int>(type: "int", nullable: true),
                    FamilleId = table.Column<int>(type: "int", nullable: true),
                    IsResponsable = table.Column<bool>(type: "bit", nullable: true),
                    PivotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnes", x => x.PersonneId);
                    table.ForeignKey(
                        name: "FK_Personnes_Familles_FamilleId",
                        column: x => x.FamilleId,
                        principalTable: "Familles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Personnes_Personnes_PivotId",
                        column: x => x.PivotId,
                        principalTable: "Personnes",
                        principalColumn: "PersonneId");
                    table.ForeignKey(
                        name: "FK_Personnes_Secteurs_SecteurId",
                        column: x => x.SecteurId,
                        principalTable: "Secteurs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Personnes_TypesRelationParente_RelationParenteId",
                        column: x => x.RelationParenteId,
                        principalTable: "TypesRelationParente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_FamilleId",
                table: "Personnes",
                column: "FamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_PivotId",
                table: "Personnes",
                column: "PivotId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_RelationParenteId",
                table: "Personnes",
                column: "RelationParenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_SecteurId",
                table: "Personnes",
                column: "SecteurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personnes");

            migrationBuilder.DropTable(
                name: "Familles");

            migrationBuilder.DropTable(
                name: "Secteurs");

            migrationBuilder.DropTable(
                name: "TypesRelationParente");

            migrationBuilder.AddColumn<double>(
                name: "MontantAgio",
                table: "Fonctionnements",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MontantAgio",
                table: "Financements",
                type: "float",
                nullable: true);
        }
    }
}

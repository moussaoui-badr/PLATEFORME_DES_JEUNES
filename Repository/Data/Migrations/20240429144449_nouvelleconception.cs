using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class nouvelleconception : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonnePivot",
                columns: table => new
                {
                    PersonnePivotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GSM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecteurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnePivot", x => x.PersonnePivotId);
                    table.ForeignKey(
                        name: "FK_PersonnePivot_Secteurs_SecteurId",
                        column: x => x.SecteurId,
                        principalTable: "Secteurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonneResponsable",
                columns: table => new
                {
                    PersonneResponsableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GSM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecteurId = table.Column<int>(type: "int", nullable: true),
                    PivotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonneResponsable", x => x.PersonneResponsableId);
                    table.ForeignKey(
                        name: "FK_PersonneResponsable_PersonnePivot_PivotId",
                        column: x => x.PivotId,
                        principalTable: "PersonnePivot",
                        principalColumn: "PersonnePivotId");
                    table.ForeignKey(
                        name: "FK_PersonneResponsable_Secteurs_SecteurId",
                        column: x => x.SecteurId,
                        principalTable: "Secteurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PersonneMembre",
                columns: table => new
                {
                    PersonneMembreId = table.Column<int>(type: "int", nullable: false)
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
                    ResponsableId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonneMembre", x => x.PersonneMembreId);
                    table.ForeignKey(
                        name: "FK_PersonneMembre_PersonneResponsable_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "PersonneResponsable",
                        principalColumn: "PersonneResponsableId");
                    table.ForeignKey(
                        name: "FK_PersonneMembre_Secteurs_SecteurId",
                        column: x => x.SecteurId,
                        principalTable: "Secteurs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonneMembre_TypesRelationParente_RelationParenteId",
                        column: x => x.RelationParenteId,
                        principalTable: "TypesRelationParente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonneMembre_RelationParenteId",
                table: "PersonneMembre",
                column: "RelationParenteId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonneMembre_ResponsableId",
                table: "PersonneMembre",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonneMembre_SecteurId",
                table: "PersonneMembre",
                column: "SecteurId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnePivot_SecteurId",
                table: "PersonnePivot",
                column: "SecteurId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonneResponsable_PivotId",
                table: "PersonneResponsable",
                column: "PivotId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonneResponsable_SecteurId",
                table: "PersonneResponsable",
                column: "SecteurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonneMembre");

            migrationBuilder.DropTable(
                name: "PersonneResponsable");

            migrationBuilder.DropTable(
                name: "PersonnePivot");
        }
    }
}

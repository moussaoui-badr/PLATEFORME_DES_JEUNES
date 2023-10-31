using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class INDH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientFinances",
                columns: table => new
                {
                    ClientFinanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sexe = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAderation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontantProjet = table.Column<double>(type: "float", nullable: false),
                    MontantApportPersonnel = table.Column<double>(type: "float", nullable: false),
                    MontantINDH = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFinances", x => x.ClientFinanceID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentINDHS",
                columns: table => new
                {
                    DocumentINDHID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomFichier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentINDHS", x => x.DocumentINDHID);
                    table.ForeignKey(
                        name: "FK_DocumentINDHS_ClientFinances_ClientID",
                        column: x => x.ClientID,
                        principalTable: "ClientFinances",
                        principalColumn: "ClientFinanceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INDHS",
                columns: table => new
                {
                    INDHID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Materiel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Montant = table.Column<double>(type: "float", nullable: false),
                    Etat = table.Column<int>(type: "int", nullable: false),
                    ClientFinanceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INDHS", x => x.INDHID);
                    table.ForeignKey(
                        name: "FK_INDHS_ClientFinances_ClientFinanceID",
                        column: x => x.ClientFinanceID,
                        principalTable: "ClientFinances",
                        principalColumn: "ClientFinanceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentINDHS_ClientID",
                table: "DocumentINDHS",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_INDHS_ClientFinanceID",
                table: "INDHS",
                column: "ClientFinanceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentINDHS");

            migrationBuilder.DropTable(
                name: "INDHS");

            migrationBuilder.DropTable(
                name: "ClientFinances");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class chapitre2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Chapitres",
            //    columns: table => new
            //    {
            //        ChapitreID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ChapitreTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        MontantTotale = table.Column<double>(type: "float", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Chapitres", x => x.ChapitreID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Financements",
            //    columns: table => new
            //    {
            //        FinancementID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Candidat = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NumeroCheque = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Beneficiaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Montant = table.Column<double>(type: "float", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Financements", x => x.FinancementID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Fonctionnements",
            //    columns: table => new
            //    {
            //        FonctionnementID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ChapitreId = table.Column<int>(type: "int", nullable: false),
            //        NuméroCheque = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Beneficiaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Montant = table.Column<double>(type: "float", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Fonctionnements", x => x.FonctionnementID);
            //        table.ForeignKey(
            //            name: "FK_Fonctionnements_Chapitres_ChapitreId",
            //            column: x => x.ChapitreId,
            //            principalTable: "Chapitres",
            //            principalColumn: "ChapitreID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Fonctionnements_ChapitreId",
            //    table: "Fonctionnements",
            //    column: "ChapitreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Financements");

            //migrationBuilder.DropTable(
            //    name: "Fonctionnements");

            //migrationBuilder.DropTable(
            //    name: "Chapitres");
        }
    }
}

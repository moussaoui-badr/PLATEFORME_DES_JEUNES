using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class Budgets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fonctionnements_Chapitres_ChapitreId",
                table: "Fonctionnements");

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

            migrationBuilder.DropColumn(
                name: "Ecart",
                table: "ClientFinances");

            migrationBuilder.DropColumn(
                name: "TotalDevis",
                table: "ClientFinances");

            migrationBuilder.AlterColumn<double>(
                name: "Montant",
                table: "Fonctionnements",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreation",
                table: "Fonctionnements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ChapitreId",
                table: "Fonctionnements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Montant",
                table: "Financements",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreation",
                table: "Financements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Financements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TypeFinancement",
                table: "Financements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BudgetFinancement",
                columns: table => new
                {
                    BudgetFinancementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateBudget = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontantBudget = table.Column<double>(type: "float", nullable: false),
                    EmetteurBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NuméroCheque = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetFinancement", x => x.BudgetFinancementID);
                });

            migrationBuilder.CreateTable(
                name: "BudgetFonctionnement",
                columns: table => new
                {
                    BudgetFonctionnementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateBudget = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontantBudget = table.Column<double>(type: "float", nullable: false),
                    EmetteurBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NuméroCheque = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetFonctionnement", x => x.BudgetFonctionnementID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Fonctionnements_Chapitres_ChapitreId",
                table: "Fonctionnements",
                column: "ChapitreId",
                principalTable: "Chapitres",
                principalColumn: "ChapitreID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fonctionnements_Chapitres_ChapitreId",
                table: "Fonctionnements");

            migrationBuilder.DropTable(
                name: "BudgetFinancement");

            migrationBuilder.DropTable(
                name: "BudgetFonctionnement");

            migrationBuilder.DropColumn(
                name: "TypeFinancement",
                table: "Financements");

            migrationBuilder.AlterColumn<double>(
                name: "Montant",
                table: "Fonctionnements",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreation",
                table: "Fonctionnements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChapitreId",
                table: "Fonctionnements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<double>(
                name: "Montant",
                table: "Financements",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreation",
                table: "Financements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Financements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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

            migrationBuilder.AddColumn<double>(
                name: "Ecart",
                table: "ClientFinances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDevis",
                table: "ClientFinances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Fonctionnements_Chapitres_ChapitreId",
                table: "Fonctionnements",
                column: "ChapitreId",
                principalTable: "Chapitres",
                principalColumn: "ChapitreID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

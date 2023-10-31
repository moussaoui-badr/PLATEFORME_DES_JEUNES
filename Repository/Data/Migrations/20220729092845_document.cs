using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Clients_ClientID",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ClientsPublic_ClientPublicID",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "ClientPublicID",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Clients_ClientID",
                table: "Documents",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ClientsPublic_ClientPublicID",
                table: "Documents",
                column: "ClientPublicID",
                principalTable: "ClientsPublic",
                principalColumn: "ClientPublicID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Clients_ClientID",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_ClientsPublic_ClientPublicID",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "ClientPublicID",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Clients_ClientID",
                table: "Documents",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_ClientsPublic_ClientPublicID",
                table: "Documents",
                column: "ClientPublicID",
                principalTable: "ClientsPublic",
                principalColumn: "ClientPublicID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

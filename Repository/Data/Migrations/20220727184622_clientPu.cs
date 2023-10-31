using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Data.Migrations
{
    public partial class clientPu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }
        //    {
        //        migrationBuilder.DropForeignKey(
        //            name: "FK_Diplomes_Clients_ClientID",
        //            table: "Diplomes");

        //        migrationBuilder.DropForeignKey(
        //            name: "FK_Documents_Clients_ClientID",
        //            table: "Documents");

        //        migrationBuilder.DropIndex(
        //            name: "IX_Clients_CIN",
        //            table: "Clients");

        //        migrationBuilder.AlterColumn<int>(
        //            name: "ClientID",
        //            table: "Documents",
        //            type: "int",
        //            nullable: true,
        //            oldClrType: typeof(int),
        //            oldType: "int");

        //        migrationBuilder.AlterColumn<int>(
        //            name: "ClientID",
        //            table: "Diplomes",
        //            type: "int",
        //            nullable: true,
        //            oldClrType: typeof(int),
        //            oldType: "int");

        //        migrationBuilder.AlterColumn<string>(
        //            name: "CIN",
        //            table: "Clients",
        //            type: "nvarchar(max)",
        //            nullable: true,
        //            oldClrType: typeof(string),
        //            oldType: "nvarchar(450)",
        //            oldNullable: true);

        //        migrationBuilder.AddForeignKey(
        //            name: "FK_Diplomes_Clients_ClientID",
        //            table: "Diplomes",
        //            column: "ClientID",
        //            principalTable: "Clients",
        //            principalColumn: "ClientID",
        //            onDelete: ReferentialAction.Restrict);

        //        migrationBuilder.AddForeignKey(
        //            name: "FK_Documents_Clients_ClientID",
        //            table: "Documents",
        //            column: "ClientID",
        //            principalTable: "Clients",
        //            principalColumn: "ClientID",
        //            onDelete: ReferentialAction.Restrict);
        //    }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diplomes_Clients_ClientID",
                table: "Diplomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Clients_ClientID",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientID",
                table: "Diplomes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CIN",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CIN",
                table: "Clients",
                column: "CIN",
                unique: true,
                filter: "[CIN] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Diplomes_Clients_ClientID",
                table: "Diplomes",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Clients_ClientID",
                table: "Documents",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class editClientmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTokens_Admins_AdminId",
                table: "AccountTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountTokens_Clients_ClientId",
                table: "AccountTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountTokens_ApplicationUsers_UserId",
                table: "AccountTokens");

            migrationBuilder.DropIndex(
                name: "IX_AccountTokens_AdminId",
                table: "AccountTokens");

            migrationBuilder.DropIndex(
                name: "IX_AccountTokens_ClientId",
                table: "AccountTokens");

            migrationBuilder.DropIndex(
                name: "IX_AccountTokens_UserId",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AccountTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AccountTokens");

            migrationBuilder.RenameColumn(
                name: "IsPasking",
                table: "Clients",
                newName: "IsParking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsParking",
                table: "Clients",
                newName: "IsPasking");

            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "AccountTokens",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "AccountTokens",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AccountTokens",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountTokens_AdminId",
                table: "AccountTokens",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTokens_ClientId",
                table: "AccountTokens",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTokens_UserId",
                table: "AccountTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTokens_Admins_AdminId",
                table: "AccountTokens",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTokens_Clients_ClientId",
                table: "AccountTokens",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTokens_ApplicationUsers_UserId",
                table: "AccountTokens",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

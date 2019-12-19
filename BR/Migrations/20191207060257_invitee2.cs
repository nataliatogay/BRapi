using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class invitee2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitees_ApplicationUsers_UserId",
                table: "Invitees");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitees_ApplicationUsers_UserId",
                table: "Invitees",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitees_ApplicationUsers_UserId",
                table: "Invitees");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitees_ApplicationUsers_UserId",
                table: "Invitees",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

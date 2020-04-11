using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_adminNotif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_ClientId",
                table: "AdminNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_RequestId",
                table: "AdminNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_ClientId",
                table: "AdminNotifications",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_RequestId",
                table: "AdminNotifications",
                column: "RequestId",
                unique: true,
                filter: "[RequestId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_ClientId",
                table: "AdminNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_RequestId",
                table: "AdminNotifications");

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_ClientId",
                table: "AdminNotifications",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_RequestId",
                table: "AdminNotifications",
                column: "RequestId");
        }
    }
}

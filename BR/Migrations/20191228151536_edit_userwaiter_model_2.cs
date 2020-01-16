using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_userwaiter_model_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificatinTag",
                table: "Waiters",
                newName: "NotificationTag");

            migrationBuilder.RenameColumn(
                name: "NotificatinTag",
                table: "ApplicationUsers",
                newName: "NotificationTag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationTag",
                table: "Waiters",
                newName: "NotificatinTag");

            migrationBuilder.RenameColumn(
                name: "NotificationTag",
                table: "ApplicationUsers",
                newName: "NotificatinTag");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_userwaiter_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificatinTag",
                table: "Waiters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificatinTag",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationTime",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificatinTag",
                table: "Waiters");

            migrationBuilder.DropColumn(
                name: "NotificatinTag",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "NotificationTime",
                table: "ApplicationUsers");
        }
    }
}

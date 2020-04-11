using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_notif : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "AdminNotifications");

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "AdminNotifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Done",
                table: "AdminNotifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "AdminNotifications",
                nullable: false,
                defaultValue: false);
        }
    }
}

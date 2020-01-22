using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_token_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationTag",
                table: "Waiters");

            migrationBuilder.DropColumn(
                name: "NotificationTag",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<string>(
                name: "NotificationTag",
                table: "AccountTokens",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationTag",
                table: "AccountTokens");

            migrationBuilder.AddColumn<string>(
                name: "NotificationTag",
                table: "Waiters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationTag",
                table: "ApplicationUsers",
                nullable: true);
        }
    }
}

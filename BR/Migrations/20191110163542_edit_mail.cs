using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_mail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UserMails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ClientMails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UserMails");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ClientMails");
        }
    }
}

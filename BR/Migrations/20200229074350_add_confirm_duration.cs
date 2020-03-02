using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_confirm_duration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmationDuration",
                table: "Clients",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationDuration",
                table: "Clients");
        }
    }
}

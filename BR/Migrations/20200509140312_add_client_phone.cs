using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_client_phone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminPhoneNumber",
                table: "Clients",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminPhoneNumber",
                table: "Clients");
        }
    }
}

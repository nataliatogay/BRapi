using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class bar_reserve_duration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BarReserveDuration",
                table: "Clients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarReserveDuration",
                table: "Clients");
        }
    }
}

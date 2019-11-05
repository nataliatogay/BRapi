using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_columns_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Clients",
                newName: "OpenTime");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Clients",
                newName: "Long");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Clients",
                newName: "Lat");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "Clients",
                newName: "CloseTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenTime",
                table: "Clients",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "Long",
                table: "Clients",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "Clients",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "CloseTime",
                table: "Clients",
                newName: "EndTime");
        }
    }
}

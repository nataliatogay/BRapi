using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RestaurantName",
                table: "Clients",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "AdminName",
                table: "Clients",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_RestaurantName",
                table: "Clients",
                column: "RestaurantName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_RestaurantName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AdminName",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantName",
                table: "Clients",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

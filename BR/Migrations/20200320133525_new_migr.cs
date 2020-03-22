using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class new_migr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BarTableId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BarTableId",
                table: "Reservations",
                column: "BarTableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BarTables_BarTableId",
                table: "Reservations",
                column: "BarTableId",
                principalTable: "BarTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BarTables_BarTableId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BarTableId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BarTableId",
                table: "Reservations");
        }
    }
}

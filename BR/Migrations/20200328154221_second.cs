using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Cuisines",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Cuisines_Title",
                table: "Cuisines",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cuisines_Title",
                table: "Cuisines");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Cuisines",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

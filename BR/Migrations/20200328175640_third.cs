using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SpecialDiets",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MealTypes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "GoodFors",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Features",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Dishes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClientTypes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDiets_Title",
                table: "SpecialDiets",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealTypes_Title",
                table: "MealTypes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoodFors_Title",
                table: "GoodFors",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_Title",
                table: "Features",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_Title",
                table: "Dishes",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientTypes_Title",
                table: "ClientTypes",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpecialDiets_Title",
                table: "SpecialDiets");

            migrationBuilder.DropIndex(
                name: "IX_MealTypes_Title",
                table: "MealTypes");

            migrationBuilder.DropIndex(
                name: "IX_GoodFors_Title",
                table: "GoodFors");

            migrationBuilder.DropIndex(
                name: "IX_Features_Title",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_Title",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_ClientTypes_Title",
                table: "ClientTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SpecialDiets",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MealTypes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "GoodFors",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Features",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Dishes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClientTypes",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_model3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "ClientRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NotificationTypes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_Title",
                table: "NotificationTypes",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationTypes_Title",
                table: "NotificationTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "NotificationTypes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "ClientRequests",
                nullable: false,
                defaultValue: false);
        }
    }
}

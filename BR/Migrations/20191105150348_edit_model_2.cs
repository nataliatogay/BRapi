using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_model_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_PhoneCodes_PhoneCodeId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_PhoneCodeId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PhoneCodeId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ApplicationUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneCodeId",
                table: "ApplicationUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ApplicationUsers",
                maxLength: 25,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_PhoneCodeId",
                table: "ApplicationUsers",
                column: "PhoneCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_PhoneCodes_PhoneCodeId",
                table: "ApplicationUsers",
                column: "PhoneCodeId",
                principalTable: "PhoneCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

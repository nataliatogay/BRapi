using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_user_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "ApplicationUsers",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "ApplicationUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "ApplicationUsers",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "ApplicationUsers",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}

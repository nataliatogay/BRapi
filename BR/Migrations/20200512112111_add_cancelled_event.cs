using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_cancelled_event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posted",
                table: "Events");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Events",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPosted",
                table: "Events",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsPosted",
                table: "Events");

            migrationBuilder.AddColumn<DateTime>(
                name: "Posted",
                table: "Events",
                nullable: true);
        }
    }
}

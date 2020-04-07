using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_visitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_CancelledByIdentityUserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "CancelledByIdentityUserId",
                table: "Reservations",
                newName: "CancelledByIdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_CancelledByIdentityUserId",
                table: "Reservations",
                newName: "IX_Reservations_CancelledByIdentityId");

            migrationBuilder.AddColumn<string>(
                name: "AddedByIdentityId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedByAdmin",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TableId = table.Column<int>(nullable: true),
                    BarTableId = table.Column<int>(nullable: true),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    GuestCount = table.Column<int>(nullable: false),
                    AddedByIdentityId = table.Column<string>(nullable: true),
                    IsCompleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visitors_AspNetUsers_AddedByIdentityId",
                        column: x => x.AddedByIdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visitors_BarTables_BarTableId",
                        column: x => x.BarTableId,
                        principalTable: "BarTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Visitors_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AddedByIdentityId",
                table: "Reservations",
                column: "AddedByIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_AddedByIdentityId",
                table: "Visitors",
                column: "AddedByIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_BarTableId",
                table: "Visitors",
                column: "BarTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_TableId",
                table: "Visitors",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_AddedByIdentityId",
                table: "Reservations",
                column: "AddedByIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_CancelledByIdentityId",
                table: "Reservations",
                column: "CancelledByIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_AddedByIdentityId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_CancelledByIdentityId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_AddedByIdentityId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "AddedByIdentityId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "IsConfirmedByAdmin",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "CancelledByIdentityId",
                table: "Reservations",
                newName: "CancelledByIdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_CancelledByIdentityId",
                table: "Reservations",
                newName: "IX_Reservations_CancelledByIdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_CancelledByIdentityUserId",
                table: "Reservations",
                column: "CancelledByIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

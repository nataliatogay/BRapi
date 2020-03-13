using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_cancel_reasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ApplicationUsers_UserId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CancelReasonId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledByIdentityUserId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CancelReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancelReasons_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CancelReasonId",
                table: "Reservations",
                column: "CancelReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CancelledByIdentityUserId",
                table: "Reservations",
                column: "CancelledByIdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelReasons_RoleId",
                table: "CancelReasons",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_CancelReasons_CancelReasonId",
                table: "Reservations",
                column: "CancelReasonId",
                principalTable: "CancelReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_CancelledByIdentityUserId",
                table: "Reservations",
                column: "CancelledByIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ApplicationUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_CancelReasons_CancelReasonId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_CancelledByIdentityUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ApplicationUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "CancelReasons");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CancelReasonId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CancelledByIdentityUserId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CancelReasonId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CancelledByIdentityUserId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ApplicationUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_event_marks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_EventTypeId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventTypeId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "EventTypeId",
                table: "Events",
                newName: "EntranceFee");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedByIdentityId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventMarks",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMarks", x => new { x.UserId, x.EventId });
                    table.UniqueConstraint("AK_EventMarks_EventId_UserId", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventMarks_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventMarks_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_AddedByIdentityId",
                table: "Events",
                column: "AddedByIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_AddedByIdentityId",
                table: "Events",
                column: "AddedByIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_AddedByIdentityId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventMarks");

            migrationBuilder.DropIndex(
                name: "IX_Events_AddedByIdentityId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AddedByIdentityId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "EntranceFee",
                table: "Events",
                newName: "EventTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId",
                table: "Events",
                column: "EventTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_EventTypeId",
                table: "Events",
                column: "EventTypeId",
                principalTable: "EventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

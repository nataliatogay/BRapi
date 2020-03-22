using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_owner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.RenameColumn(
                name: "IsShow",
                table: "ClientPhones",
                newName: "IsWhatsApp");

            migrationBuilder.CreateTable(
                name: "ClientRatings",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRatings", x => new { x.UserId, x.ClientId });
                    table.UniqueConstraint("AK_ClientRatings_ClientId_UserId", x => new { x.ClientId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ClientRatings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRatings_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientRatings");

            migrationBuilder.RenameColumn(
                name: "IsWhatsApp",
                table: "ClientPhones",
                newName: "IsShow");

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => new { x.UserId, x.ClientId });
                    table.UniqueConstraint("AK_UserRatings_ClientId_UserId", x => new { x.ClientId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRatings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRatings_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}

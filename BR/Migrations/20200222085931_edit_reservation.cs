using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_reservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationStates_ReservationStateId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_TableStates_TableStateId",
                table: "Tables");

            migrationBuilder.DropTable(
                name: "TableStates");

            migrationBuilder.DropIndex(
                name: "IX_Tables_TableStateId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "TableStateId",
                table: "Tables");

            migrationBuilder.AlterColumn<int>(
                name: "ReservationStateId",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ReserveDurationAvg",
                table: "Clients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationStates_ReservationStateId",
                table: "Reservations",
                column: "ReservationStateId",
                principalTable: "ReservationStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationStates_ReservationStateId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReserveDurationAvg",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "TableStateId",
                table: "Tables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReservationStateId",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TableStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_TableStateId",
                table: "Tables",
                column: "TableStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationStates_ReservationStateId",
                table: "Reservations",
                column: "ReservationStateId",
                principalTable: "ReservationStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_TableStates_TableStateId",
                table: "Tables",
                column: "TableStateId",
                principalTable: "TableStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

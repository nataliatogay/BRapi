using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableReservation_Reservations_ReservationId",
                table: "TableReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_TableReservation_Tables_TableId",
                table: "TableReservation");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TableReservation_ReservationId_TableId",
                table: "TableReservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableReservation",
                table: "TableReservation");

            migrationBuilder.DropColumn(
                name: "ClientRequestId",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "TableReservation",
                newName: "TableReservations");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TableReservations_ReservationId_TableId",
                table: "TableReservations",
                columns: new[] { "ReservationId", "TableId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableReservations",
                table: "TableReservations",
                columns: new[] { "TableId", "ReservationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservations_Reservations_ReservationId",
                table: "TableReservations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservations_Tables_TableId",
                table: "TableReservations",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableReservations_Reservations_ReservationId",
                table: "TableReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_TableReservations_Tables_TableId",
                table: "TableReservations");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TableReservations_ReservationId_TableId",
                table: "TableReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableReservations",
                table: "TableReservations");

            migrationBuilder.RenameTable(
                name: "TableReservations",
                newName: "TableReservation");

            migrationBuilder.AddColumn<int>(
                name: "ClientRequestId",
                table: "Clients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TableReservation_ReservationId_TableId",
                table: "TableReservation",
                columns: new[] { "ReservationId", "TableId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableReservation",
                table: "TableReservation",
                columns: new[] { "TableId", "ReservationId" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservation_Reservations_ReservationId",
                table: "TableReservation",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservation_Tables_TableId",
                table: "TableReservation",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

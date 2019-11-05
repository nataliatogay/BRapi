using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BR.Migrations
{
    public partial class edit_table_reserve_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableReservation_Tables_TableId",
                table: "TableReservation");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TableReservation_ReservationId_TabletId",
                table: "TableReservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableReservation",
                table: "TableReservation");

            migrationBuilder.DropIndex(
                name: "IX_TableReservation_TableId",
                table: "TableReservation");

            migrationBuilder.DropColumn(
                name: "TabletId",
                table: "TableReservation");

            migrationBuilder.AlterColumn<int>(
                name: "TableId",
                table: "TableReservation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TableReservation_ReservationId_TableId",
                table: "TableReservation",
                columns: new[] { "ReservationId", "TableId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableReservation",
                table: "TableReservation",
                columns: new[] { "TableId", "ReservationId" });

            migrationBuilder.CreateTable(
                name: "ClientAccountTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ClientId = table.Column<int>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    Expires = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAccountTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAccountTokens_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientAccountTokens_ClientId",
                table: "ClientAccountTokens",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservation_Tables_TableId",
                table: "TableReservation",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableReservation_Tables_TableId",
                table: "TableReservation");

            migrationBuilder.DropTable(
                name: "ClientAccountTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_TableReservation_ReservationId_TableId",
                table: "TableReservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableReservation",
                table: "TableReservation");

            migrationBuilder.AlterColumn<int>(
                name: "TableId",
                table: "TableReservation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TabletId",
                table: "TableReservation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_TableReservation_ReservationId_TabletId",
                table: "TableReservation",
                columns: new[] { "ReservationId", "TabletId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableReservation",
                table: "TableReservation",
                columns: new[] { "TabletId", "ReservationId" });

            migrationBuilder.CreateIndex(
                name: "IX_TableReservation_TableId",
                table: "TableReservation",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableReservation_Tables_TableId",
                table: "TableReservation",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

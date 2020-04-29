using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_reservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_AddedByIdentityId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BarTables_BarTableId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ApplicationUsers_UserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_BarTables_BarTableId",
                table: "Visitors");

            migrationBuilder.DropTable(
                name: "BarTables");

            migrationBuilder.DropTable(
                name: "TableReservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BarTableId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BarTableId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "BarTableId",
                table: "Visitors",
                newName: "BarId");

            migrationBuilder.RenameIndex(
                name: "IX_Visitors_BarTableId",
                table: "Visitors",
                newName: "IX_Visitors_BarId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reservations",
                newName: "ReservationRequestId");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo",
                table: "Reservations",
                newName: "IdentityUserId");

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "Waiters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "AddedByIdentityId",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Reservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BarReservationId",
                table: "Invitees",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaxGuestCount = table.Column<int>(nullable: false),
                    HallId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bars_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRequestStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRequestStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BarReservationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestedByIdentityId = table.Column<string>(nullable: false),
                    ReservationDateTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    BarId = table.Column<int>(nullable: false),
                    GuestCount = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    ReviewedByIndentityId = table.Column<string>(nullable: true),
                    ReservationRequestStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarReservationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarReservationRequests_Bars_BarId",
                        column: x => x.BarId,
                        principalTable: "Bars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarReservationRequests_AspNetUsers_RequestedByIdentityId",
                        column: x => x.RequestedByIdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarReservationRequests_ReservationRequestStates_ReservationRequestStateId",
                        column: x => x.ReservationRequestStateId,
                        principalTable: "ReservationRequestStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarReservationRequests_AspNetUsers_ReviewedByIndentityId",
                        column: x => x.ReviewedByIndentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestedByIdentityId = table.Column<string>(nullable: false),
                    ReservationDateTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: false),
                    GuestCount = table.Column<int>(nullable: false),
                    ChildFree = table.Column<bool>(nullable: false),
                    PetsFree = table.Column<bool>(nullable: false),
                    Invalids = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    ReviewedByIndentityId = table.Column<string>(nullable: true),
                    ReservationRequestStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationRequests_AspNetUsers_RequestedByIdentityId",
                        column: x => x.RequestedByIdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationRequests_ReservationRequestStates_ReservationRequestStateId",
                        column: x => x.ReservationRequestStateId,
                        principalTable: "ReservationRequestStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationRequests_AspNetUsers_ReviewedByIndentityId",
                        column: x => x.ReviewedByIndentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationRequests_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BarReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentityUserId = table.Column<string>(nullable: true),
                    ReservationDate = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    BarId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    GuestCount = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    ReservationStateId = table.Column<int>(nullable: true),
                    CancelReasonId = table.Column<int>(nullable: true),
                    CancelledByIdentityId = table.Column<string>(nullable: true),
                    AddedByIdentityId = table.Column<string>(nullable: false),
                    BarReservationRequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarReservations_AspNetUsers_AddedByIdentityId",
                        column: x => x.AddedByIdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarReservations_Bars_BarId",
                        column: x => x.BarId,
                        principalTable: "Bars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarReservations_BarReservationRequests_BarReservationRequestId",
                        column: x => x.BarReservationRequestId,
                        principalTable: "BarReservationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarReservations_CancelReasons_CancelReasonId",
                        column: x => x.CancelReasonId,
                        principalTable: "CancelReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarReservations_AspNetUsers_CancelledByIdentityId",
                        column: x => x.CancelledByIdentityId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarReservations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarReservations_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BarReservations_ReservationStates_ReservationStateId",
                        column: x => x.ReservationStateId,
                        principalTable: "ReservationStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IdentityUserId",
                table: "Reservations",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationRequestId",
                table: "Reservations",
                column: "ReservationRequestId",
                unique: true,
                filter: "[ReservationRequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableId",
                table: "Reservations",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitees_BarReservationId",
                table: "Invitees",
                column: "BarReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservationRequests_BarId",
                table: "BarReservationRequests",
                column: "BarId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservationRequests_RequestedByIdentityId",
                table: "BarReservationRequests",
                column: "RequestedByIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservationRequests_ReservationRequestStateId",
                table: "BarReservationRequests",
                column: "ReservationRequestStateId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservationRequests_ReviewedByIndentityId",
                table: "BarReservationRequests",
                column: "ReviewedByIndentityId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_AddedByIdentityId",
                table: "BarReservations",
                column: "AddedByIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_BarId",
                table: "BarReservations",
                column: "BarId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_BarReservationRequestId",
                table: "BarReservations",
                column: "BarReservationRequestId",
                unique: true,
                filter: "[BarReservationRequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_CancelReasonId",
                table: "BarReservations",
                column: "CancelReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_CancelledByIdentityId",
                table: "BarReservations",
                column: "CancelledByIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_ClientId",
                table: "BarReservations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_IdentityUserId",
                table: "BarReservations",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BarReservations_ReservationStateId",
                table: "BarReservations",
                column: "ReservationStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Bars_HallId",
                table: "Bars",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_RequestedByIdentityId",
                table: "ReservationRequests",
                column: "RequestedByIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_ReservationRequestStateId",
                table: "ReservationRequests",
                column: "ReservationRequestStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_ReviewedByIndentityId",
                table: "ReservationRequests",
                column: "ReviewedByIndentityId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_TableId",
                table: "ReservationRequests",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequestStates_Title",
                table: "ReservationRequestStates",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitees_BarReservations_BarReservationId",
                table: "Invitees",
                column: "BarReservationId",
                principalTable: "BarReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_AddedByIdentityId",
                table: "Reservations",
                column: "AddedByIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationRequests_ReservationRequestId",
                table: "Reservations",
                column: "ReservationRequestId",
                principalTable: "ReservationRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tables_TableId",
                table: "Reservations",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Bars_BarId",
                table: "Visitors",
                column: "BarId",
                principalTable: "Bars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitees_BarReservations_BarReservationId",
                table: "Invitees");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_AddedByIdentityId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationRequests_ReservationRequestId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tables_TableId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Bars_BarId",
                table: "Visitors");

            migrationBuilder.DropTable(
                name: "BarReservations");

            migrationBuilder.DropTable(
                name: "ReservationRequests");

            migrationBuilder.DropTable(
                name: "BarReservationRequests");

            migrationBuilder.DropTable(
                name: "Bars");

            migrationBuilder.DropTable(
                name: "ReservationRequestStates");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_IdentityUserId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ReservationRequestId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TableId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Invitees_BarReservationId",
                table: "Invitees");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Waiters");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BarReservationId",
                table: "Invitees");

            migrationBuilder.RenameColumn(
                name: "BarId",
                table: "Visitors",
                newName: "BarTableId");

            migrationBuilder.RenameIndex(
                name: "IX_Visitors_BarId",
                table: "Visitors",
                newName: "IX_Visitors_BarTableId");

            migrationBuilder.RenameColumn(
                name: "ReservationRequestId",
                table: "Reservations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Reservations",
                newName: "AdditionalInfo");

            migrationBuilder.AlterColumn<string>(
                name: "AddedByIdentityId",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalInfo",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BarTableId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BarTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HallId = table.Column<int>(nullable: false),
                    MaxGuestCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BarTables_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableReservations",
                columns: table => new
                {
                    TableId = table.Column<int>(nullable: false),
                    ReservationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableReservations", x => new { x.TableId, x.ReservationId });
                    table.UniqueConstraint("AK_TableReservations_ReservationId_TableId", x => new { x.ReservationId, x.TableId });
                    table.ForeignKey(
                        name: "FK_TableReservations_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableReservations_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BarTableId",
                table: "Reservations",
                column: "BarTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BarTables_HallId",
                table: "BarTables",
                column: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_AddedByIdentityId",
                table: "Reservations",
                column: "AddedByIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BarTables_BarTableId",
                table: "Reservations",
                column: "BarTableId",
                principalTable: "BarTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ApplicationUsers_UserId",
                table: "Reservations",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_BarTables_BarTableId",
                table: "Visitors",
                column: "BarTableId",
                principalTable: "BarTables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

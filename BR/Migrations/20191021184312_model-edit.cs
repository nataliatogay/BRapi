using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BR.Migrations
{
    public partial class modeledit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Clients_ClientId",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Reservations_ReservationId",
                table: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Tables_ReservationId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Tables");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Tables",
                newName: "TableStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Tables_ClientId",
                table: "Tables",
                newName: "IX_Tables_TableStateId");

            migrationBuilder.AddColumn<int>(
                name: "HallId",
                table: "Tables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "SocialLinks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ReservationStates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PaymentTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Halls",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JsonInfo",
                table: "Halls",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Cuisines",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClientTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MainImagePath",
                table: "Clients",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Admins",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AdminAccountTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PhotoPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(nullable: false),
                    HallId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoPoints_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableReservation",
                columns: table => new
                {
                    TabletId = table.Column<int>(nullable: false),
                    ReservationId = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableReservation", x => new { x.TabletId, x.ReservationId });
                    table.UniqueConstraint("AK_TableReservation_ReservationId_TabletId", x => new { x.ReservationId, x.TabletId });
                    table.ForeignKey(
                        name: "FK_TableReservation_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableReservation_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TableState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableState", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_HallId",
                table: "Tables",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoPoints_HallId",
                table: "PhotoPoints",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservation_TableId",
                table: "TableReservation",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Halls_HallId",
                table: "Tables",
                column: "HallId",
                principalTable: "Halls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_TableState_TableStateId",
                table: "Tables",
                column: "TableStateId",
                principalTable: "TableState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Halls_HallId",
                table: "Tables");

            migrationBuilder.DropForeignKey(
                name: "FK_Tables_TableState_TableStateId",
                table: "Tables");

            migrationBuilder.DropTable(
                name: "PhotoPoints");

            migrationBuilder.DropTable(
                name: "TableReservation");

            migrationBuilder.DropTable(
                name: "TableState");

            migrationBuilder.DropIndex(
                name: "IX_Tables_HallId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "HallId",
                table: "Tables");

            migrationBuilder.RenameColumn(
                name: "TableStateId",
                table: "Tables",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Tables_TableStateId",
                table: "Tables",
                newName: "IX_Tables_ClientId");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Tables",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "SocialLinks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ReservationStates",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "PaymentTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "News",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "News",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Halls",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "JsonInfo",
                table: "Halls",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Cuisines",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ClientTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MainImagePath",
                table: "Clients",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Admins",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AdminAccountTokens",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Tables_ReservationId",
                table: "Tables",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Clients_ClientId",
                table: "Tables",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Reservations_ReservationId",
                table: "Tables",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

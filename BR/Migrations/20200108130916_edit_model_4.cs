using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_model_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Floor_Clients_ClientId",
                table: "Floor");

            migrationBuilder.DropForeignKey(
                name: "FK_Halls_Floor_FloorId",
                table: "Halls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Floor",
                table: "Floor");

            migrationBuilder.RenameTable(
                name: "Floor",
                newName: "Floors");

            migrationBuilder.RenameIndex(
                name: "IX_Floor_ClientId",
                table: "Floors",
                newName: "IX_Floors_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Floors",
                table: "Floors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Floors_Clients_ClientId",
                table: "Floors",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Halls_Floors_FloorId",
                table: "Halls",
                column: "FloorId",
                principalTable: "Floors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Floors_Clients_ClientId",
                table: "Floors");

            migrationBuilder.DropForeignKey(
                name: "FK_Halls_Floors_FloorId",
                table: "Halls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Floors",
                table: "Floors");

            migrationBuilder.RenameTable(
                name: "Floors",
                newName: "Floor");

            migrationBuilder.RenameIndex(
                name: "IX_Floors_ClientId",
                table: "Floor",
                newName: "IX_Floor_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Floor",
                table: "Floor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Floor_Clients_ClientId",
                table: "Floor",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Halls_Floor_FloorId",
                table: "Halls",
                column: "FloorId",
                principalTable: "Floor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

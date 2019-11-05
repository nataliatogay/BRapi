using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_table_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToBeClient_Clients_ClientId",
                table: "ToBeClient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToBeClient",
                table: "ToBeClient");

            migrationBuilder.RenameTable(
                name: "ToBeClient",
                newName: "ToBeClients");

            migrationBuilder.RenameIndex(
                name: "IX_ToBeClient_ClientId",
                table: "ToBeClients",
                newName: "IX_ToBeClients_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToBeClients",
                table: "ToBeClients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToBeClients_Clients_ClientId",
                table: "ToBeClients",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToBeClients_Clients_ClientId",
                table: "ToBeClients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ToBeClients",
                table: "ToBeClients");

            migrationBuilder.RenameTable(
                name: "ToBeClients",
                newName: "ToBeClient");

            migrationBuilder.RenameIndex(
                name: "IX_ToBeClients_ClientId",
                table: "ToBeClient",
                newName: "IX_ToBeClient_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToBeClient",
                table: "ToBeClient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToBeClient_Clients_ClientId",
                table: "ToBeClient",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

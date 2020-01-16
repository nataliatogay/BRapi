using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class add_table_state : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_TableState_TableStateId",
                table: "Tables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableState",
                table: "TableState");

            migrationBuilder.RenameTable(
                name: "TableState",
                newName: "TableStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableStates",
                table: "TableStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_TableStates_TableStateId",
                table: "Tables",
                column: "TableStateId",
                principalTable: "TableStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_TableStates_TableStateId",
                table: "Tables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TableStates",
                table: "TableStates");

            migrationBuilder.RenameTable(
                name: "TableStates",
                newName: "TableState");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TableState",
                table: "TableState",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_TableState_TableStateId",
                table: "Tables",
                column: "TableStateId",
                principalTable: "TableState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_onDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCuisines_Cuisines_CuisineId",
                table: "ClientCuisines");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCuisines_Cuisines_CuisineId",
                table: "ClientCuisines",
                column: "CuisineId",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCuisines_Cuisines_CuisineId",
                table: "ClientCuisines");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCuisines_Cuisines_CuisineId",
                table: "ClientCuisines",
                column: "CuisineId",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class edit_parameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialLinks",
                table: "SocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_SocialLinks_ClientId",
                table: "SocialLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientPhones",
                table: "ClientPhones");

            migrationBuilder.DropIndex(
                name: "IX_ClientPhones_ClientId",
                table: "ClientPhones");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ClientPhones");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "SocialLinks",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "ClientRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "ClientPhones",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialLinks",
                table: "SocialLinks",
                columns: new[] { "ClientId", "Link" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientPhones",
                table: "ClientPhones",
                columns: new[] { "ClientId", "Number" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialLinks",
                table: "SocialLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientPhones",
                table: "ClientPhones");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "ClientRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "SocialLinks",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SocialLinks",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "ClientPhones",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ClientPhones",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialLinks",
                table: "SocialLinks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientPhones",
                table: "ClientPhones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_ClientId",
                table: "SocialLinks",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPhones_ClientId",
                table: "ClientPhones",
                column: "ClientId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class client_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminNotifications_OwnerRequests_RequestId",
                table: "AdminNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_RequestId",
                table: "AdminNotifications");

            migrationBuilder.DropColumn(
                name: "IsConfirmedByAdmin",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "AdminNotifications",
                newName: "OwnerRequestId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminConfirmation",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Clients",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_OwnerRequestId",
                table: "AdminNotifications",
                column: "OwnerRequestId",
                unique: true,
                filter: "[OwnerRequestId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminNotifications_OwnerRequests_OwnerRequestId",
                table: "AdminNotifications",
                column: "OwnerRequestId",
                principalTable: "OwnerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminNotifications_OwnerRequests_OwnerRequestId",
                table: "AdminNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AdminNotifications_OwnerRequestId",
                table: "AdminNotifications");

            migrationBuilder.DropColumn(
                name: "AdminConfirmation",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "OwnerRequestId",
                table: "AdminNotifications",
                newName: "RequestId");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedByAdmin",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_RequestId",
                table: "AdminNotifications",
                column: "RequestId",
                unique: true,
                filter: "[RequestId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminNotifications_OwnerRequests_RequestId",
                table: "AdminNotifications",
                column: "RequestId",
                principalTable: "OwnerRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

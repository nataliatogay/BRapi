using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class change_client : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Clients_ClientId",
                table: "ClientRequests");

            migrationBuilder.DropTable(
                name: "ClientPaymentTypes");

            migrationBuilder.DropTable(
                name: "PhoneCodes");

            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests");

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsBusinessLunch",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsChildrenZone",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsLiveMusic",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsOpenSpace",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsParking",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsWiFi",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "BarReserveDuration",
                table: "Clients",
                newName: "OrganizationId");

            migrationBuilder.RenameColumn(
                name: "JsonInfo",
                table: "ClientRequests",
                newName: "OwnerPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ClientRequests",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "IsShow",
                table: "ClientPhones",
                newName: "IsWhatsApp");

            migrationBuilder.AddColumn<int>(
                name: "BarReserveDurationAvg",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Blocked",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestaurantName",
                table: "Clients",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "ClientRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ClientRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "ClientRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "ClientRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ClientRatings",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRatings", x => new { x.UserId, x.ClientId });
                    table.UniqueConstraint("AK_ClientRatings_ClientId_UserId", x => new { x.ClientId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ClientRatings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRatings_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Editable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodFors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodFors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    LogoPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDiets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDiets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientDishes",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    DishId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDishes", x => new { x.ClientId, x.DishId });
                    table.ForeignKey(
                        name: "FK_ClientDishes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientFeatures",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFeatures", x => new { x.ClientId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_ClientFeatures_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientGoodFors",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    GoodForId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGoodFors", x => new { x.ClientId, x.GoodForId });
                    table.ForeignKey(
                        name: "FK_ClientGoodFors_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientGoodFors_GoodFors_GoodForId",
                        column: x => x.GoodForId,
                        principalTable: "GoodFors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IdentityId = table.Column<int>(nullable: false),
                    IdentityIdS = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Owners_AspNetUsers_IdentityIdS",
                        column: x => x.IdentityIdS,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Owners_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientSpecialDiets",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    SpecialDietId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSpecialDiets", x => new { x.ClientId, x.SpecialDietId });
                    table.ForeignKey(
                        name: "FK_ClientSpecialDiets_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientSpecialDiets_SpecialDiets_SpecialDietId",
                        column: x => x.SpecialDietId,
                        principalTable: "SpecialDiets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_OrganizationId",
                table: "Clients",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_OwnerId",
                table: "ClientRequests",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDishes_DishId",
                table: "ClientDishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeatures_FeatureId",
                table: "ClientFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGoodFors_GoodForId",
                table: "ClientGoodFors",
                column: "GoodForId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSpecialDiets_SpecialDietId",
                table: "ClientSpecialDiets",
                column: "SpecialDietId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_IdentityIdS",
                table: "Owners",
                column: "IdentityIdS");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_OrganizationId",
                table: "Owners",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Owners_OwnerId",
                table: "ClientRequests",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Organizations_OrganizationId",
                table: "Clients",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientRequests_Owners_OwnerId",
                table: "ClientRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Organizations_OrganizationId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "ClientDishes");

            migrationBuilder.DropTable(
                name: "ClientFeatures");

            migrationBuilder.DropTable(
                name: "ClientGoodFors");

            migrationBuilder.DropTable(
                name: "ClientRatings");

            migrationBuilder.DropTable(
                name: "ClientSpecialDiets");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "GoodFors");

            migrationBuilder.DropTable(
                name: "SpecialDiets");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Clients_OrganizationId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_ClientRequests_OwnerId",
                table: "ClientRequests");

            migrationBuilder.DropColumn(
                name: "BarReserveDurationAvg",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Blocked",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RestaurantName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "ClientRequests");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ClientRequests");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "ClientRequests");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "ClientRequests");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Clients",
                newName: "BarReserveDuration");

            migrationBuilder.RenameColumn(
                name: "OwnerPhoneNumber",
                table: "ClientRequests",
                newName: "JsonInfo");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "ClientRequests",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "IsWhatsApp",
                table: "ClientPhones",
                newName: "IsShow");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Clients",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Clients",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBusinessLunch",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsChildrenZone",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiveMusic",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenSpace",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsParking",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWiFi",
                table: "Clients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Clients",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Country = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => new { x.UserId, x.ClientId });
                    table.UniqueConstraint("AK_UserRatings_ClientId_UserId", x => new { x.ClientId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRatings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRatings_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPaymentTypes",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    PaymentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPaymentTypes", x => new { x.ClientId, x.PaymentTypeId });
                    table.ForeignKey(
                        name: "FK_ClientPaymentTypes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientPaymentTypes_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientRequests_ClientId",
                table: "ClientRequests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPaymentTypes_PaymentTypeId",
                table: "ClientPaymentTypes",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientRequests_Clients_ClientId",
                table: "ClientRequests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BR.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(nullable: false),
            //        Name = table.Column<string>(maxLength: 256, nullable: true),
            //        NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
            //        ConcurrencyStamp = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(nullable: false),
            //        UserName = table.Column<string>(maxLength: 256, nullable: true),
            //        NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
            //        Email = table.Column<string>(maxLength: 256, nullable: true),
            //        NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
            //        EmailConfirmed = table.Column<bool>(nullable: false),
            //        PasswordHash = table.Column<string>(nullable: true),
            //        SecurityStamp = table.Column<string>(nullable: true),
            //        ConcurrencyStamp = table.Column<string>(nullable: true),
            //        PhoneNumber = table.Column<string>(nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
            //        LockoutEnabled = table.Column<bool>(nullable: false),
            //        AccessFailedCount = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Cuisines",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Cuisines", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EventTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EventTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "MealTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_MealTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PaymentTypes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PaymentTypes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PhoneCodes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Country = table.Column<string>(nullable: false),
            //        Code = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PhoneCodes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ReservationStates",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ReservationStates", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TableStates",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TableStates", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserPhones",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Number = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserPhones", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoleClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        RoleId = table.Column<string>(nullable: false),
            //        ClaimType = table.Column<string>(nullable: true),
            //        ClaimValue = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AccountTokens",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        IdentityUserId = table.Column<string>(nullable: false),
            //        RefreshToken = table.Column<string>(nullable: false),
            //        Expires = table.Column<DateTime>(nullable: false),
            //        NotificationTag = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AccountTokens", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AccountTokens_AspNetUsers_IdentityUserId",
            //            column: x => x.IdentityUserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Admins",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        IdentityId = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Admins", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Admins_AspNetUsers_IdentityId",
            //            column: x => x.IdentityId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ApplicationUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        FirstName = table.Column<string>(nullable: false),
            //        LastName = table.Column<string>(nullable: false),
            //        ImagePath = table.Column<string>(nullable: true),
            //        Gender = table.Column<bool>(nullable: true),
            //        IsBlocked = table.Column<bool>(nullable: false),
            //        BirthDate = table.Column<DateTime>(nullable: true),
            //        NotificationTime = table.Column<int>(nullable: false),
            //        IdentityId = table.Column<string>(nullable: true),
            //        RegistrationDate = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ApplicationUsers_AspNetUsers_IdentityId",
            //            column: x => x.IdentityId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        UserId = table.Column<string>(nullable: false),
            //        ClaimType = table.Column<string>(nullable: true),
            //        ClaimValue = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserLogins",
            //    columns: table => new
            //    {
            //        LoginProvider = table.Column<string>(nullable: false),
            //        ProviderKey = table.Column<string>(nullable: false),
            //        ProviderDisplayName = table.Column<string>(nullable: true),
            //        UserId = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserRoles",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(nullable: false),
            //        RoleId = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserTokens",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(nullable: false),
            //        LoginProvider = table.Column<string>(nullable: false),
            //        Name = table.Column<string>(nullable: false),
            //        Value = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Clients",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(maxLength: 30, nullable: false),
            //        Address = table.Column<string>(maxLength: 150, nullable: false),
            //        Lat = table.Column<float>(nullable: false),
            //        Long = table.Column<float>(nullable: false),
            //        OpenTime = table.Column<int>(nullable: false),
            //        CloseTime = table.Column<int>(nullable: false),
            //        IsParking = table.Column<bool>(nullable: false),
            //        IsWiFi = table.Column<bool>(nullable: false),
            //        IsLiveMusic = table.Column<bool>(nullable: true),
            //        IsOpenSpace = table.Column<bool>(nullable: false),
            //        IsChildrenZone = table.Column<bool>(nullable: false),
            //        IsBusinessLunch = table.Column<bool>(nullable: false),
            //        AdditionalInfo = table.Column<string>(maxLength: 250, nullable: true),
            //        MainImagePath = table.Column<string>(nullable: false),
            //        MaxReserveDays = table.Column<int>(nullable: false),
            //        IsBlocked = table.Column<bool>(nullable: false),
            //        RegistrationDate = table.Column<DateTime>(nullable: false),
            //        IdentityId = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Clients", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Clients_AspNetUsers_IdentityId",
            //            column: x => x.IdentityId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Reservations",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        ReservationDate = table.Column<DateTime>(nullable: false),
            //        UserId = table.Column<int>(nullable: false),
            //        ChildFree = table.Column<bool>(nullable: false),
            //        GuestCount = table.Column<int>(nullable: false),
            //        Comments = table.Column<string>(nullable: true),
            //        ReservationStateId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Reservations", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Reservations_ReservationStates_ReservationStateId",
            //            column: x => x.ReservationStateId,
            //            principalTable: "ReservationStates",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Reservations_ApplicationUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "ApplicationUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserUserPhones",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(nullable: false),
            //        UserPhoneId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserUserPhones", x => new { x.UserId, x.UserPhoneId });
            //        table.ForeignKey(
            //            name: "FK_UserUserPhones_ApplicationUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "ApplicationUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UserUserPhones_UserPhones_UserPhoneId",
            //            column: x => x.UserPhoneId,
            //            principalTable: "UserPhones",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientClientTypes",
            //    columns: table => new
            //    {
            //        ClientId = table.Column<int>(nullable: false),
            //        ClientTypeId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientClientTypes", x => new { x.ClientId, x.ClientTypeId });
            //        table.ForeignKey(
            //            name: "FK_ClientClientTypes_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ClientClientTypes_ClientTypes_ClientTypeId",
            //            column: x => x.ClientTypeId,
            //            principalTable: "ClientTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientCuisines",
            //    columns: table => new
            //    {
            //        ClientId = table.Column<int>(nullable: false),
            //        CuisineId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientCuisines", x => new { x.ClientId, x.CuisineId });
            //        table.ForeignKey(
            //            name: "FK_ClientCuisines_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ClientCuisines_Cuisines_CuisineId",
            //            column: x => x.CuisineId,
            //            principalTable: "Cuisines",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientImages",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        ImagePath = table.Column<string>(nullable: true),
            //        ClientId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientImages", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ClientImages_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientMealTypes",
            //    columns: table => new
            //    {
            //        ClientId = table.Column<int>(nullable: false),
            //        MealTypeId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientMealTypes", x => new { x.ClientId, x.MealTypeId });
            //        table.ForeignKey(
            //            name: "FK_ClientMealTypes_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ClientMealTypes_MealTypes_MealTypeId",
            //            column: x => x.MealTypeId,
            //            principalTable: "MealTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientPaymentTypes",
            //    columns: table => new
            //    {
            //        ClientId = table.Column<int>(nullable: false),
            //        PaymentTypeId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientPaymentTypes", x => new { x.ClientId, x.PaymentTypeId });
            //        table.ForeignKey(
            //            name: "FK_ClientPaymentTypes_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ClientPaymentTypes_PaymentTypes_PaymentTypeId",
            //            column: x => x.PaymentTypeId,
            //            principalTable: "PaymentTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientPhones",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Number = table.Column<string>(nullable: false),
            //        ClientId = table.Column<int>(nullable: false),
            //        IsShow = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientPhones", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ClientPhones_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ClientRequests",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        RegisteredDate = table.Column<DateTime>(nullable: false),
            //        JsonInfo = table.Column<string>(nullable: false),
            //        ClientId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ClientRequests", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ClientRequests_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Events",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Date = table.Column<DateTime>(nullable: false),
            //        Title = table.Column<string>(nullable: true),
            //        Description = table.Column<string>(nullable: true),
            //        ImagePath = table.Column<string>(nullable: true),
            //        ClientId = table.Column<int>(nullable: false),
            //        EventTypeId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Events", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Events_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Events_EventTypes_EventTypeId",
            //            column: x => x.EventTypeId,
            //            principalTable: "EventTypes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Favourites",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(nullable: false),
            //        ClientId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Favourites", x => new { x.UserId, x.ClientId });
            //        table.UniqueConstraint("AK_Favourites_ClientId_UserId", x => new { x.ClientId, x.UserId });
            //        table.ForeignKey(
            //            name: "FK_Favourites_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Favourites_ApplicationUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "ApplicationUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Floors",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Number = table.Column<int>(nullable: false),
            //        ClientId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Floors", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Floors_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "News",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        ClientId = table.Column<int>(nullable: false),
            //        ImagePath = table.Column<string>(nullable: false),
            //        Description = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_News", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_News_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SocialLinks",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Link = table.Column<string>(nullable: false),
            //        ClientId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SocialLinks", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_SocialLinks_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Waiters",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        IdentityId = table.Column<string>(nullable: false),
            //        FirstName = table.Column<string>(nullable: false),
            //        LastName = table.Column<string>(nullable: false),
            //        ClientId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Waiters", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Waiters_Clients_ClientId",
            //            column: x => x.ClientId,
            //            principalTable: "Clients",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Waiters_AspNetUsers_IdentityId",
            //            column: x => x.IdentityId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Invitees",
            //    columns: table => new
            //    {
            //        ReservationId = table.Column<int>(nullable: false),
            //        UserId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Invitees", x => new { x.UserId, x.ReservationId });
            //        table.UniqueConstraint("AK_Invitees_ReservationId_UserId", x => new { x.ReservationId, x.UserId });
            //        table.ForeignKey(
            //            name: "FK_Invitees_Reservations_ReservationId",
            //            column: x => x.ReservationId,
            //            principalTable: "Reservations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Invitees_ApplicationUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "ApplicationUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Halls",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: false),
            //        FloorId = table.Column<int>(nullable: false),
            //        JsonInfo = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Halls", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Halls_Floors_FloorId",
            //            column: x => x.FloorId,
            //            principalTable: "Floors",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PhotoPoints",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        X = table.Column<int>(nullable: false),
            //        Y = table.Column<int>(nullable: false),
            //        ImagePath = table.Column<string>(nullable: false),
            //        HallId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PhotoPoints", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PhotoPoints_Halls_HallId",
            //            column: x => x.HallId,
            //            principalTable: "Halls",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tables",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Number = table.Column<int>(nullable: false),
            //        HallId = table.Column<int>(nullable: false),
            //        MaxGuests = table.Column<int>(nullable: false),
            //        MinGuests = table.Column<int>(nullable: false),
            //        TableStateId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tables", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Tables_Halls_HallId",
            //            column: x => x.HallId,
            //            principalTable: "Halls",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Tables_TableStates_TableStateId",
            //            column: x => x.TableStateId,
            //            principalTable: "TableStates",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TableReservations",
            //    columns: table => new
            //    {
            //        TableId = table.Column<int>(nullable: false),
            //        ReservationId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TableReservations", x => new { x.TableId, x.ReservationId });
            //        table.UniqueConstraint("AK_TableReservations_ReservationId_TableId", x => new { x.ReservationId, x.TableId });
            //        table.ForeignKey(
            //            name: "FK_TableReservations_Reservations_ReservationId",
            //            column: x => x.ReservationId,
            //            principalTable: "Reservations",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TableReservations_Tables_TableId",
            //            column: x => x.TableId,
            //            principalTable: "Tables",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AccountTokens_IdentityUserId",
            //    table: "AccountTokens",
            //    column: "IdentityUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Admins_IdentityId",
            //    table: "Admins",
            //    column: "IdentityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ApplicationUsers_IdentityId",
            //    table: "ApplicationUsers",
            //    column: "IdentityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName",
            //    unique: true,
            //    filter: "[NormalizedName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName",
            //    unique: true,
            //    filter: "[NormalizedUserName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientClientTypes_ClientTypeId",
            //    table: "ClientClientTypes",
            //    column: "ClientTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientCuisines_CuisineId",
            //    table: "ClientCuisines",
            //    column: "CuisineId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientImages_ClientId",
            //    table: "ClientImages",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientMealTypes_MealTypeId",
            //    table: "ClientMealTypes",
            //    column: "MealTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientPaymentTypes_PaymentTypeId",
            //    table: "ClientPaymentTypes",
            //    column: "PaymentTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientPhones_ClientId",
            //    table: "ClientPhones",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientRequests_ClientId",
            //    table: "ClientRequests",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Clients_IdentityId",
            //    table: "Clients",
            //    column: "IdentityId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Events_ClientId",
            //    table: "Events",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Events_EventTypeId",
            //    table: "Events",
            //    column: "EventTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Floors_ClientId",
            //    table: "Floors",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Halls_FloorId",
            //    table: "Halls",
            //    column: "FloorId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_News_ClientId",
            //    table: "News",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PhotoPoints_HallId",
            //    table: "PhotoPoints",
            //    column: "HallId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reservations_ReservationStateId",
            //    table: "Reservations",
            //    column: "ReservationStateId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reservations_UserId",
            //    table: "Reservations",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SocialLinks_ClientId",
            //    table: "SocialLinks",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tables_HallId",
            //    table: "Tables",
            //    column: "HallId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tables_TableStateId",
            //    table: "Tables",
            //    column: "TableStateId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserUserPhones_UserPhoneId",
            //    table: "UserUserPhones",
            //    column: "UserPhoneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Waiters_ClientId",
            //    table: "Waiters",
            //    column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Waiters_IdentityId",
            //    table: "Waiters",
            //    column: "IdentityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTokens");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClientClientTypes");

            migrationBuilder.DropTable(
                name: "ClientCuisines");

            migrationBuilder.DropTable(
                name: "ClientImages");

            migrationBuilder.DropTable(
                name: "ClientMealTypes");

            migrationBuilder.DropTable(
                name: "ClientPaymentTypes");

            migrationBuilder.DropTable(
                name: "ClientPhones");

            migrationBuilder.DropTable(
                name: "ClientRequests");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "Invitees");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "PhoneCodes");

            migrationBuilder.DropTable(
                name: "PhotoPoints");

            migrationBuilder.DropTable(
                name: "SocialLinks");

            migrationBuilder.DropTable(
                name: "TableReservations");

            migrationBuilder.DropTable(
                name: "UserUserPhones");

            migrationBuilder.DropTable(
                name: "Waiters");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ClientTypes");

            migrationBuilder.DropTable(
                name: "Cuisines");

            migrationBuilder.DropTable(
                name: "MealTypes");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "UserPhones");

            migrationBuilder.DropTable(
                name: "ReservationStates");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "TableStates");

            migrationBuilder.DropTable(
                name: "Floors");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

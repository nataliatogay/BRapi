﻿// <auto-generated />
using System;
using BR.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BR.Migrations
{
    [DbContext(typeof(BRDbContext))]
    partial class BRDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BR.Models.AccountToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Expires");

                    b.Property<string>("IdentityUserId")
                        .IsRequired();

                    b.Property<string>("NotificationTag")
                        .IsRequired();

                    b.Property<string>("RefreshToken")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.ToTable("AccountTokens");
                });

            modelBuilder.Entity("BR.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IdentityId");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("BR.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(250);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("CloseTime");

                    b.Property<string>("IdentityId");

                    b.Property<bool>("IsBlocked");

                    b.Property<bool>("IsBusinessLunch");

                    b.Property<bool>("IsChildrenZone");

                    b.Property<bool?>("IsLiveMusic");

                    b.Property<bool>("IsOpenSpace");

                    b.Property<bool>("IsParking");

                    b.Property<bool>("IsWiFi");

                    b.Property<float>("Lat");

                    b.Property<float>("Long");

                    b.Property<string>("MainImagePath")
                        .IsRequired();

                    b.Property<int>("MaxReserveDays");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("OpenTime");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("BR.Models.ClientClientType", b =>
                {
                    b.Property<int>("ClientId");

                    b.Property<int>("ClientTypeId");

                    b.HasKey("ClientId", "ClientTypeId");

                    b.HasIndex("ClientTypeId");

                    b.ToTable("ClientClientTypes");
                });

            modelBuilder.Entity("BR.Models.ClientCuisine", b =>
                {
                    b.Property<int>("ClientId");

                    b.Property<int>("CuisineId");

                    b.HasKey("ClientId", "CuisineId");

                    b.HasIndex("CuisineId");

                    b.ToTable("ClientCuisines");
                });

            modelBuilder.Entity("BR.Models.ClientFavourite", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ClientId");

                    b.HasKey("UserId", "ClientId");

                    b.HasAlternateKey("ClientId", "UserId");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("BR.Models.ClientImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("ImagePath");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientImages");
                });

            modelBuilder.Entity("BR.Models.ClientMealType", b =>
                {
                    b.Property<int>("ClientId");

                    b.Property<int>("MealTypeId");

                    b.HasKey("ClientId", "MealTypeId");

                    b.HasIndex("MealTypeId");

                    b.ToTable("ClientMealTypes");
                });

            modelBuilder.Entity("BR.Models.ClientPaymentType", b =>
                {
                    b.Property<int>("ClientId");

                    b.Property<int>("PaymentTypeId");

                    b.HasKey("ClientId", "PaymentTypeId");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("ClientPaymentTypes");
                });

            modelBuilder.Entity("BR.Models.ClientPhone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<bool>("IsShow");

                    b.Property<string>("Number")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientPhones");
                });

            modelBuilder.Entity("BR.Models.ClientRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId");

                    b.Property<string>("JsonInfo")
                        .IsRequired();

                    b.Property<DateTime>("RegisteredDate");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientRequests");
                });

            modelBuilder.Entity("BR.Models.ClientType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ClientTypes");
                });

            modelBuilder.Entity("BR.Models.Cuisine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Cuisines");
                });

            modelBuilder.Entity("BR.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("EventTypeId");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("BR.Models.EventType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("EventTypes");
                });

            modelBuilder.Entity("BR.Models.Floor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<int>("Number");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Floors");
                });

            modelBuilder.Entity("BR.Models.Hall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FloorId");

                    b.Property<string>("JsonInfo")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FloorId");

                    b.ToTable("Halls");
                });

            modelBuilder.Entity("BR.Models.Invitee", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ReservationId");

                    b.HasKey("UserId", "ReservationId");

                    b.HasAlternateKey("ReservationId", "UserId");

                    b.ToTable("Invitees");
                });

            modelBuilder.Entity("BR.Models.MealType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("MealTypes");
                });

            modelBuilder.Entity("BR.Models.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImagePath")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("BR.Models.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("BR.Models.PhoneCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code");

                    b.Property<string>("Country")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("PhoneCodes");
                });

            modelBuilder.Entity("BR.Models.PhotoPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HallId");

                    b.Property<string>("ImagePath")
                        .IsRequired();

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.ToTable("PhotoPoints");
                });

            modelBuilder.Entity("BR.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ChildFree");

                    b.Property<string>("Comments");

                    b.Property<int>("GuestCount");

                    b.Property<DateTime>("ReservationDate");

                    b.Property<int>("ReservationStateId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ReservationStateId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("BR.Models.ReservationState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ReservationStates");
                });

            modelBuilder.Entity("BR.Models.SocialLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("Link")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("SocialLinks");
                });

            modelBuilder.Entity("BR.Models.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HallId");

                    b.Property<int>("MaxGuests");

                    b.Property<int>("MinGuests");

                    b.Property<int>("Number");

                    b.Property<int>("TableStateId");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.HasIndex("TableStateId");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("BR.Models.TableReservation", b =>
                {
                    b.Property<int>("TableId");

                    b.Property<int>("ReservationId");

                    b.HasKey("TableId", "ReservationId");

                    b.HasAlternateKey("ReservationId", "TableId");

                    b.ToTable("TableReservations");
                });

            modelBuilder.Entity("BR.Models.TableState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TableStates");
                });

            modelBuilder.Entity("BR.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool?>("Gender");

                    b.Property<string>("IdentityId");

                    b.Property<string>("ImagePath");

                    b.Property<bool>("IsBlocked");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int>("NotificationTime");

                    b.Property<DateTime>("RegistrationDate");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("BR.Models.UserPhone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Number");

                    b.HasKey("Id");

                    b.ToTable("UserPhones");
                });

            modelBuilder.Entity("BR.Models.UserUserPhone", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("UserPhoneId");

                    b.HasKey("UserId", "UserPhoneId");

                    b.HasIndex("UserPhoneId");

                    b.ToTable("UserUserPhones");
                });

            modelBuilder.Entity("BR.Models.Waiter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("IdentityId")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("IdentityId");

                    b.ToTable("Waiters");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BR.Models.AccountToken", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Admin", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("BR.Models.Client", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("BR.Models.ClientClientType", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientClientTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.ClientType", "ClientType")
                        .WithMany("ClientClientTypes")
                        .HasForeignKey("ClientTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BR.Models.ClientCuisine", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientCuisines")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.Cuisine", "Cuisine")
                        .WithMany("ClientCuisines")
                        .HasForeignKey("CuisineId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BR.Models.ClientFavourite", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientFavourites")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.User", "User")
                        .WithMany("ClientFavourites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.ClientImage", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientImages")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.ClientMealType", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientMealTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.MealType", "MealType")
                        .WithMany("ClientMealTypes")
                        .HasForeignKey("MealTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BR.Models.ClientPaymentType", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientPaymentTypes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.PaymentType", "PaymentType")
                        .WithMany("ClientPaymentTypes")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BR.Models.ClientPhone", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("ClientPhones")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.ClientRequest", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("BR.Models.Event", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("Events")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.EventType", "EventType")
                        .WithMany("Events")
                        .HasForeignKey("EventTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Floor", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("Floors")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Hall", b =>
                {
                    b.HasOne("BR.Models.Floor", "Floor")
                        .WithMany("Halls")
                        .HasForeignKey("FloorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Invitee", b =>
                {
                    b.HasOne("BR.Models.Reservation", "Reservation")
                        .WithMany("Invitees")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.User", "User")
                        .WithMany("Invitees")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BR.Models.News", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("News")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.PhotoPoint", b =>
                {
                    b.HasOne("BR.Models.Hall", "Hall")
                        .WithMany("PhotoPoints")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Reservation", b =>
                {
                    b.HasOne("BR.Models.ReservationState", "ReservationState")
                        .WithMany("Reservations")
                        .HasForeignKey("ReservationStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.SocialLink", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("SocialLinks")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Table", b =>
                {
                    b.HasOne("BR.Models.Hall", "Hall")
                        .WithMany("Tables")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.TableState", "TableState")
                        .WithMany("Tables")
                        .HasForeignKey("TableStateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.TableReservation", b =>
                {
                    b.HasOne("BR.Models.Reservation", "Reservation")
                        .WithMany("TableReservations")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.Table", "Table")
                        .WithMany("TableReservations")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.User", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("BR.Models.UserUserPhone", b =>
                {
                    b.HasOne("BR.Models.User", "User")
                        .WithMany("UserUserPhones")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BR.Models.UserPhone", "UserPhone")
                        .WithMany("UserUserPhones")
                        .HasForeignKey("UserPhoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BR.Models.Waiter", b =>
                {
                    b.HasOne("BR.Models.Client", "Client")
                        .WithMany("Waiters")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

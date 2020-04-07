using BR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.EF
{
    public class BRDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<User> ApplicationUsers { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<ClientRequest> ClientRequests { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<SocialLink> SocialLinks { get; set; }

        public DbSet<Cuisine> Cuisines { get; set; }

        public DbSet<ClientType> ClientTypes { get; set; }

        public DbSet<MealType> MealTypes { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<GoodFor> GoodFors { get; set; }

        public DbSet<SpecialDiet> SpecialDiets { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<ClientPhone> ClientPhones { get; set; }

        public DbSet<ClientMealType> ClientMealTypes { get; set; }

        public DbSet<ClientClientType> ClientClientTypes { get; set; }

        public DbSet<ClientCuisine> ClientCuisines { get; set; }

        public DbSet<ClientDish> ClientDishes { get; set; }

        public DbSet<ClientGoodFor> ClientGoodFors { get; set; }

        public DbSet<ClientFeature> ClientFeatures { get; set; }

        public DbSet<ClientSpecialDiet> ClientSpecialDiets { get; set; }

        public DbSet<Floor> Floors { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<BarTable> BarTables { get; set; }

        public DbSet<ReservationState> ReservationStates { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<TableReservation> TableReservations { get; set; }

        public DbSet<CancelReason> CancelReasons { get; set; }

        public DbSet<Invitee> Invitees { get; set; }

        public DbSet<PhotoPoint> PhotoPoints { get; set; }

        public DbSet<AccountToken> AccountTokens { get; set; }

        public DbSet<Waiter> Waiters { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<ClientImage> ClientImages { get; set; }

        public DbSet<ClientFavourite> Favourites { get; set; }

        public DbSet<UserPhone> UserPhones { get; set; }

        public DbSet<UserUserPhone> UserUserPhones { get; set; }

        public DbSet<ClientRating> ClientRatings { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Privilege> Privileges { get; set; }

        public DbSet<UserPrivileges> UserPrivileges { get; set; }

        public DbSet<EventMark> EventMarks { get; set; }

        public DbSet<Visitor> Visitors { get; set; }



        public BRDbContext(DbContextOptions options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientMealType>().HasKey(p => new { p.ClientId, p.MealTypeId });

            modelBuilder.Entity<ClientClientType>().HasKey(c => new { c.ClientId, c.ClientTypeId });

            modelBuilder.Entity<ClientCuisine>().HasKey(c => new { c.ClientId, c.CuisineId });

            modelBuilder.Entity<ClientSpecialDiet>().HasKey(c => new { c.ClientId, c.SpecialDietId });

            modelBuilder.Entity<ClientDish>().HasKey(c => new { c.ClientId, c.DishId });

            modelBuilder.Entity<ClientGoodFor>().HasKey(c => new { c.ClientId, c.GoodForId });

            modelBuilder.Entity<ClientFeature>().HasKey(c => new { c.ClientId, c.FeatureId });

            modelBuilder.Entity<TableReservation>().HasKey(t => new { t.TableId, t.ReservationId });

            modelBuilder.Entity<Invitee>().HasKey(i => new { i.UserId, i.ReservationId });

            modelBuilder.Entity<UserUserPhone>().HasKey(u => new { u.UserId, u.UserPhoneId });

            modelBuilder.Entity<ClientFavourite>().HasKey(c => new { c.UserId, c.ClientId });

            modelBuilder.Entity<ClientRating>().HasKey(u => new { u.UserId, u.ClientId });

            modelBuilder.Entity<UserPrivileges>().HasKey(u => new { u.PrivilegeId, u.IdentityId });

            modelBuilder.Entity<SocialLink>().HasKey(u => new { u.ClientId, u.Link });

            modelBuilder.Entity<ClientPhone>().HasKey(u => new { u.ClientId, u.Number });

            modelBuilder.Entity<EventMark>().HasKey(m => new { m.UserId, m.EventId });

            modelBuilder.Entity<Invitee>().HasOne(i => i.User).WithMany(a => a.Invitees).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientCuisine>().HasOne(c => c.Cuisine).WithMany(c => c.ClientCuisines).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientClientType>().HasOne(c => c.ClientType).WithMany(c => c.ClientClientTypes).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientMealType>().HasOne(c => c.MealType).WithMany(c => c.ClientMealTypes).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>().HasOne(r => r.BarTable).WithMany(c => c.Reservations).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>().HasOne(r => r.Organization).WithMany(c => c.Clients).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Owner>().HasOne(r => r.Organization).WithMany(c => c.Owners).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>().HasOne(r => r.Client).WithMany(c => c.Reservations).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cuisine>().HasIndex(u => u.Title).IsUnique();

            modelBuilder.Entity<MealType>().HasIndex(t => t.Title).IsUnique();

            modelBuilder.Entity<ClientType>().HasIndex(t => t.Title).IsUnique();

            modelBuilder.Entity<GoodFor>().HasIndex(g => g.Title).IsUnique();

            modelBuilder.Entity<Feature>().HasIndex(f => f.Title).IsUnique();

            modelBuilder.Entity<SpecialDiet>().HasIndex(d => d.Title).IsUnique();

            modelBuilder.Entity<Dish>().HasIndex(d => d.Title).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}

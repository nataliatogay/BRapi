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
        public DbSet<PhoneCode> PhoneCodes { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<ClientPhone> ClientPhones { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ClientPaymentType> ClientPaymentTypes { get; set; }
        public DbSet<ClientMealType> ClientMealTypes { get; set; }
        public DbSet<ClientClientType> ClientClientTypes { get; set; }
        public DbSet<ClientCuisine> ClientCuisines { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ReservationState> ReservationStates { get; set; }
       // public DbSet<TableState> TableStates { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<TableReservation> TableReservations { get; set; }
        public DbSet<CancelReason> CancelReasons { get; set; }
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<PhotoPoint> PhotoPoints { get; set; }
        public DbSet<AccountToken> AccountTokens { get; set; }
        public DbSet<Waiter> Waiters { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ClientImage> ClientImages { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ClientFavourite> Favourites { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }
        public DbSet<UserUserPhone> UserUserPhones { get; set; }
        //public DbSet<ClientMail> ClientMails { get; set; }
        //public DbSet<UserMail> UserMails { get; set; }






        public BRDbContext(DbContextOptions options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientPaymentType>().HasKey(p => new { p.ClientId, p.PaymentTypeId });
            modelBuilder.Entity<ClientMealType>().HasKey(p => new { p.ClientId, p.MealTypeId });
            modelBuilder.Entity<ClientClientType>().HasKey(c => new { c.ClientId, c.ClientTypeId });
            modelBuilder.Entity<ClientCuisine>().HasKey(c => new { c.ClientId, c.CuisineId });
            modelBuilder.Entity<TableReservation>().HasKey(t => new { t.TableId, t.ReservationId });
            modelBuilder.Entity<Invitee>().HasKey(i => new { i.UserId, i.ReservationId });
            modelBuilder.Entity<UserUserPhone>().HasKey(u => new { u.UserId, u.UserPhoneId });
            modelBuilder.Entity<ClientFavourite>().HasKey(c => new { c.UserId, c.ClientId });

            modelBuilder.Entity<Invitee>().HasOne(i => i.User).WithMany(a => a.Invitees).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientCuisine>().HasOne(c => c.Cuisine).WithMany(c => c.ClientCuisines).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientPaymentType>().HasOne(c => c.PaymentType).WithMany(c => c.ClientPaymentTypes).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientClientType>().HasOne(c => c.ClientType).WithMany(c => c.ClientClientTypes).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientMealType>().HasOne(c => c.MealType).WithMany(c => c.ClientMealTypes).OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }

    }
}

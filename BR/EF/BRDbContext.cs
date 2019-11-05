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
        public DbSet<ToBeClient> ToBeClients { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<PhoneCode> PhoneCodes { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<ClientPhone> ClientPhones { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ClientPaymentType> ClientPaymentTypes { get; set; }        
        public DbSet<ClientClientType> ClientClientTypes { get; set; }
        public DbSet<ClientCuisine> ClientCuisines { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<ReservationState> ReservationStates { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<PhotoPoint> PhotoPoints { get; set; }
        public DbSet<AccountToken> AccountTokens { get; set; }





        public BRDbContext(DbContextOptions options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientPaymentType>().HasKey(p => new { p.ClientId, p.PaymentTypeId });
            modelBuilder.Entity<ClientClientType>().HasKey(c => new { c.ClientId, c.ClientTypeId });
            modelBuilder.Entity<ClientCuisine>().HasKey(c => new { c.ClientId, c.CuisineId });
            modelBuilder.Entity<TableReservation>().HasKey(t => new { t.TableId, t.ReservationId });
            modelBuilder.Entity<Invitee>().HasKey(i => new { i.UserId, i.ReservationId });


            base.OnModelCreating(modelBuilder);
        }
    }
}

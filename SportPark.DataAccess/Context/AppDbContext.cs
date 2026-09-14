using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportPark.Domains.Entities;

namespace SportPark.DataAccess.Context
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ClassSession> ClassSessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ClassSession)
                .WithMany()
                .HasForeignKey(b => b.ClassSessionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

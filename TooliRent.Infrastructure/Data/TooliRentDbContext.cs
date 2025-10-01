using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Models;

namespace TooliRent.Infrastructure.Data
{
    public class TooliRentDbContext : IdentityDbContext<User>
    {
        public TooliRentDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookingTool> BookingTools { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Tool>()
               .HasOne(t => t.Category)
                .WithMany(c => c.Tools)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Booking>()
            //    .HasOne(b => b.Tool)
            //    .WithMany(t => t.Bookings)
            //    .HasForeignKey(b => b.ToolId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BookingTool>()
            .HasKey(bt => new { bt.BookingId, bt.ToolId });

            builder.Entity<BookingTool>()
                .HasOne(bt => bt.Booking)
                .WithMany(b => b.BookingTools)
                .HasForeignKey(bt => bt.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<BookingTool>()
                .HasOne(bt => bt.Tool)
                .WithMany(t => t.BookingTools)
                .HasForeignKey(bt => bt.ToolId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}

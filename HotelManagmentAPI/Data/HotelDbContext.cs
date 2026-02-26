using HotelManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagmentAPI.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Guest> Guests { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<Bill> Bills { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Guest>().HasKey(g => g.GuestId);
            modelBuilder.Entity<Room>().HasKey(r => r.RoomId);
            modelBuilder.Entity<Reservation>().HasKey(r => r.ReservationId);
            modelBuilder.Entity<Bill>().HasKey(b => b.BillId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(ro => ro.Reservations)
                .HasForeignKey(r => r.RoomId);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Reservation)
                .WithOne(r => r.Bill)
                .HasForeignKey<Bill>(b => b.ReservationId);

            modelBuilder.Entity<Room>()
                .Property(r => r.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Room>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}
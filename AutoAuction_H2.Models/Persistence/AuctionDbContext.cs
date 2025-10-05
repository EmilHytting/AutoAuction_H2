using Microsoft.EntityFrameworkCore;

namespace AutoAuction_H2.Models.Persistence
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<AuctionEntity> Auctions { get; set; } = null!;
        public DbSet<BidEntity> Bids { get; set; } = null!;
        public DbSet<VehicleEntity> Vehicles { get; set; } = null!;
        public DbSet<CarEntity> Cars { get; set; } = null!;
        public DbSet<TruckEntity> Trucks { get; set; } = null!;
        public DbSet<BusEntity> Buses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Vehicle inheritance
            modelBuilder.Entity<VehicleEntity>()
                .HasDiscriminator<string>("VehicleType")
                .HasValue<CarEntity>("Car")
                .HasValue<PrivateCarEntity>("PrivateCar")
                .HasValue<ProfessionalCarEntity>("ProfessionalCar")
                .HasValue<TruckEntity>("Truck")
                .HasValue<BusEntity>("Bus");


            // Auction → Vehicle (1:1, cascade delete)
            modelBuilder.Entity<AuctionEntity>()
                .HasOne(a => a.Vehicle)
                .WithOne()
                .HasForeignKey<AuctionEntity>(a => a.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Auction → Seller (User not deleted)
            modelBuilder.Entity<AuctionEntity>()
                .HasOne(a => a.Seller)
                .WithMany()
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Auction → HighestBidder (User not deleted)
            modelBuilder.Entity<AuctionEntity>()
                .HasOne(a => a.HighestBidder)
                .WithMany()
                .HasForeignKey(a => a.HighestBidderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Auction → Bids (cascade delete)
            modelBuilder.Entity<BidEntity>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bid → User (restrict, anonymize instead of delete)
            modelBuilder.Entity<BidEntity>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Precision for decimals
            modelBuilder.Entity<UserEntity>().Property(u => u.Balance).HasPrecision(18, 2);
            modelBuilder.Entity<UserEntity>().Property(u => u.CreditLimit).HasPrecision(18, 2);
            modelBuilder.Entity<VehicleEntity>().Property(v => v.PurchasePrice).HasPrecision(18, 2);
            modelBuilder.Entity<AuctionEntity>().Property(a => a.MinPrice).HasPrecision(18, 2);
            modelBuilder.Entity<AuctionEntity>().Property(a => a.CurrentBid).HasPrecision(18, 2);
            modelBuilder.Entity<BidEntity>().Property(b => b.Amount).HasPrecision(18, 2);
        }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<Bid> Bids { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Tender>().HasKey(t => t.Id);
            modelBuilder.Entity<Bid>().HasKey(b => b.Id);

            // Tender -> Bids (One-to-Many)
            modelBuilder.Entity<Tender>()
                        .HasMany(t => t.Bids)
                        .WithOne(b => b.Tender)
                        .HasForeignKey(b => b.TenderId);

            // Bid -> Bidder (Many-to-One)
            modelBuilder.Entity<Bid>()
                        .HasOne(b => b.Bidder)
                        .WithMany()
                        .HasForeignKey(b => b.BidderId);

            // Add unique constraint on email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure decimal precision
            modelBuilder.Entity<Bid>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            // Enum storage
            modelBuilder.Entity<Tender>()
                .Property(t => t.State)
                .HasConversion<string>();

            // Value Object: Budget (Money)
            modelBuilder.Entity<Tender>().OwnsOne(t => t.Budget, b =>
            {
                b.Property(p => p.Amount)
                 .HasColumnName("BudgetAmount")
                 .HasPrecision(18, 2);

                b.Property(p => p.Currency)
                 .HasColumnName("BudgetCurrency");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
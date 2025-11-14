using Entities.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
        public DbSet<Stock> Stocks => Set<Stock>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CUSTOMER
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CustomerName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.HasMany(x => x.Addresses)
                      .WithOne(x => x.Customer)
                      .HasForeignKey(x => x.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.Orders)
                      .WithOne(x => x.Customer)
                      .HasForeignKey(x => x.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // CUSTOMER ADDRESS
            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Address).HasMaxLength(500);
                entity.Property(x => x.City).HasMaxLength(100);
                entity.Property(x => x.Country).HasMaxLength(100);
                entity.Property(x => x.Town).HasMaxLength(100);
                entity.Property(x => x.Email).HasMaxLength(200);
                entity.Property(x => x.Phone).HasMaxLength(50);
                entity.Property(x => x.PostalCode).HasMaxLength(20);

                entity.HasMany(x => x.InvoiceOrders)
                      .WithOne(x => x.InvoiceAddress)
                      .HasForeignKey(x => x.InvoiceAddressId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(x => x.DeliveryOrders)
                      .WithOne(x => x.DeliveryAddress)
                      .HasForeignKey(x => x.DeliveryAddressId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // STOCK
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.StockName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(x => x.Unit)
                      .HasMaxLength(50);

                entity.Property(x => x.Price)
                      .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Barcode)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(x => x.Barcode)
                      .IsUnique();
            });

            // ORDER
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.OrderNo)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.TotalPrice)
                      .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Tax)
                      .HasColumnType("decimal(18,2)");
            });

            // ORDER DETAIL
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(x => x.Order)
                      .WithMany(x => x.OrderDetails)
                      .HasForeignKey(x => x.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Stock)
                      .WithMany(x => x.OrderDetails)
                      .HasForeignKey(x => x.StockId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}

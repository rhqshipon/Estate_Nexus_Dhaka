using System.Configuration;
using EstateNexus.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstateNexus.Data
{
    public class EstateNexusDbContext : DbContext
    {
        public EstateNexusDbContext()
        {
        }

        public EstateNexusDbContext(DbContextOptions<EstateNexusDbContext> options)
            : base(options)
        {
        }

        // 19 DbSets strictly matching the ER Diagram
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<PropertyCategory> PropertyCategories { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyImage> PropertyImages { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<PropertyFeature> PropertyFeatures { get; set; }
        public virtual DbSet<PropertyFeatureMapping> PropertyFeatureMappings { get; set; }
        public virtual DbSet<FeaturedListing> FeaturedListings { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Commission> Commissions { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<VisitRequest> VisitRequests { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DatabaseSetup.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Roles
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.RoleName).IsUnique();
                entity.Property(e => e.RoleDescription).HasMaxLength(255);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // 2. Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.ProfileImagePath).HasMaxLength(500);
                entity.Property(e => e.AccountStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Active");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 3. PropertyCategories
            modelBuilder.Entity<PropertyCategory>(entity =>
            {
                entity.ToTable("PropertyCategories");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.CategoryName).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // 4. Properties
            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Properties");
                entity.HasKey(e => e.PropertyId);
                entity.Property(e => e.PropertyTitle).IsRequired().HasMaxLength(150);
                entity.Property(e => e.ListingType).IsRequired().HasMaxLength(20);
                entity.Property(e => e.District).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AreaLocation).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FullAddress).IsRequired().HasMaxLength(255);
                entity.Property(e => e.AreaSize).HasPrecision(10, 2);
                entity.Property(e => e.AreaUnit).IsRequired().HasMaxLength(20).HasDefaultValue("sqft");
                entity.Property(e => e.Bedrooms).HasDefaultValue(0);
                entity.Property(e => e.Bathrooms).HasDefaultValue(0);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.PropertyStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Available");
                entity.Property(e => e.ApprovalStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");
                entity.Property(e => e.IsFeatured).HasDefaultValue(false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Owner)
                      .WithMany(u => u.OwnedProperties)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Properties)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 5. PropertyImages
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.ToTable("PropertyImages");
                entity.HasKey(e => e.ImageId);
                entity.Property(e => e.ImagePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);
                entity.Property(e => e.UploadedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.PropertyImages)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 6. Offers
            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("Offers");
                entity.HasKey(e => e.OfferId);
                entity.Property(e => e.DiscountType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DiscountValue).HasPrecision(18, 2);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.Offers)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 7. PropertyFeatures
            modelBuilder.Entity<PropertyFeature>(entity =>
            {
                entity.ToTable("PropertyFeatures");
                entity.HasKey(e => e.FeatureId);
                entity.Property(e => e.FeatureName).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.FeatureName).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            // 8. PropertyFeatureMappings (Composite Key)
            modelBuilder.Entity<PropertyFeatureMapping>(entity =>
            {
                entity.ToTable("PropertyFeatureMappings");
                entity.HasKey(e => new { e.PropertyId, e.FeatureId });

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.PropertyFeatureMappings)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Feature)
                      .WithMany(f => f.PropertyFeatureMappings)
                      .HasForeignKey(e => e.FeatureId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 9. FeaturedListings
            modelBuilder.Entity<FeaturedListing>(entity =>
            {
                entity.ToTable("FeaturedListings");
                entity.HasKey(e => e.FeaturedListingId);
                entity.Property(e => e.FeaturedFee).HasPrecision(18, 2);
                entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Active");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.FeaturedListings)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 10. Carts
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts");
                entity.HasKey(e => e.CartId);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Customer)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 11. CartItems
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItems");
                entity.HasKey(e => e.CartItemId);
                entity.Property(e => e.RentalMonths).HasDefaultValue(1);
                entity.Property(e => e.OfferedPrice).HasPrecision(18, 2);
                entity.Property(e => e.AddedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(e => e.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 12. Orders
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.OrderStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Completed");
                entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50).HasDefaultValue("Sale");
                entity.Property(e => e.OrderDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Customer)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 13. OrderItems
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.Quantity).HasDefaultValue(1);
                entity.Property(e => e.RentalMonths).HasDefaultValue(0);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2).HasDefaultValue(0m);
                entity.Property(e => e.FinalAmount).HasPrecision(18, 2);

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.OrderItems)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Owner)
                      .WithMany(u => u.OwnedOrderItems)
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 14. Payments
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TransactionId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PaymentAmount).HasPrecision(18, 2);
                entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Completed");
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Payments)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 15. Invoices
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");
                entity.HasKey(e => e.InvoiceId);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
                entity.Property(e => e.SubTotal).HasPrecision(18, 2);
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2).HasDefaultValue(0m);
                entity.Property(e => e.CommissionAmount).HasPrecision(18, 2).HasDefaultValue(0m);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.GeneratedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Order)
                      .WithOne(o => o.Invoice)
                      .HasForeignKey<Invoice>(e => e.OrderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Payment)
                      .WithOne(p => p.Invoice)
                      .HasForeignKey<Invoice>(e => e.PaymentId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 16. Commissions
            modelBuilder.Entity<Commission>(entity =>
            {
                entity.ToTable("Commissions");
                entity.HasKey(e => e.CommissionId);
                entity.Property(e => e.CommissionRate).HasPrecision(5, 2);
                entity.Property(e => e.TransactionAmount).HasPrecision(18, 2);
                entity.Property(e => e.CommissionAmount).HasPrecision(18, 2);
                entity.Property(e => e.OwnerAmount).HasPrecision(18, 2);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Order)
                      .WithOne(o => o.Commission)
                      .HasForeignKey<Commission>(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 17. Complaints
            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.ToTable("Complaints");
                entity.HasKey(e => e.ComplaintId);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ComplaintType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Priority).IsRequired().HasMaxLength(20).HasDefaultValue("Normal");
                entity.Property(e => e.ComplaintStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Customer)
                      .WithMany(u => u.SubmittedComplaints)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.Complaints)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Resolver)
                      .WithMany(u => u.ResolvedComplaints)
                      .HasForeignKey(e => e.ResolvedBy)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 18. VisitRequests
            modelBuilder.Entity<VisitRequest>(entity =>
            {
                entity.ToTable("VisitRequests");
                entity.HasKey(e => e.VisitRequestId);
                entity.Property(e => e.VisitTime).HasMaxLength(20);
                entity.Property(e => e.RequestStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Customer)
                      .WithMany(u => u.VisitRequests)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.VisitRequests)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 19. Reviews
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews");
                entity.HasKey(e => e.ReviewId);
                entity.Property(e => e.ReviewStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Approved");
                entity.Property(e => e.ReviewDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Customer)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.Reviews)
                      .HasForeignKey(e => e.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

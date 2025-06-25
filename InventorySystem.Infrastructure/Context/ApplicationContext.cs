using InventorySystem.Domain.Common;
using InventorySystem.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Context
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
        }
        public DbSet<WhereHosing> WhereHosing { get; set; }
        public DbSet<WhereHosing_Product> WhereHosingProducts { get; set; }
        public DbSet<Transaction_type> TransactionTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction_History> TransactionHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var baseModelType = typeof(BaseModel<>);
            var entities = builder.Model.GetEntityTypes()
           .Where(t => t.ClrType.BaseType != null &&
                       t.ClrType.BaseType.IsGenericType &&
                       t.ClrType.BaseType.GetGenericTypeDefinition() == baseModelType);

            foreach (var entityType in entities)
            {
                var idProperty = entityType.FindProperty("Id");

                builder.Entity(entityType.ClrType)
                    .HasKey("Id");

                if (idProperty.ClrType == typeof(Guid))
                {
                    builder.Entity(entityType.ClrType)
                        .Property("Id")
                        .HasDefaultValueSql("NEWID()");
                }
                else if (idProperty.ClrType == typeof(int))
                {
                    builder.Entity(entityType.ClrType)
                        .Property("Id")
                        .ValueGeneratedOnAdd();
                }

                builder.Entity(entityType.ClrType)
                    .Property("IsDeleted")
                    .HasDefaultValue(false);

                builder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("GETUTCDATE()");
            }
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.Category_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>()
                .HasMany(p => p.TransactionHistories)
                .WithOne(t => t.Product)
                .HasForeignKey(t => t.ProductID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>()
                .HasMany(p => p.WhereHosing_Products)
                .WithOne(wp => wp.Product)
                .HasForeignKey(wp => wp.Product_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction_History>()
                .HasOne(th => th.FromWarehouse)
                .WithMany()
                .HasForeignKey(th => th.FromWherehosing)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction_History>()
                .HasOne(th => th.ToWarehouse)
                .WithMany()
                .HasForeignKey(th => th.ToWherehosing)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction_History>()
                .HasOne(th => th.Transaction_Type)
                .WithMany(tt => tt.TransactionHistories)
                .HasForeignKey(th => th.Transaction_Type_ID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WhereHosing_Product>()
                .HasOne(wp => wp.WhereHosing)
                .WithMany(w => w.WhereHosing_Products)
                .HasForeignKey(wp => wp.WhereHosing_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WhereHosing_Product>()
                .HasOne(wp => wp.Product)
                .WithMany(p => p.WhereHosing_Products)
                .HasForeignKey(wp => wp.Product_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasKey(n => n.Id);
        }
    }
}

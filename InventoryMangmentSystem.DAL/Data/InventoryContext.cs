
using InventoryMangmentSystem.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using InventoryMangmentSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace InventoryMangmentSystem.DAL.Data
{
    public class InventoryContext : IdentityDbContext<ApplicationUser>
    {
        public InventoryContext(DbContextOptions<InventoryContext> options)
            : base(options)
        {
        }


        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Transaction_History> transactions { get; set; }
        public DbSet<Transaction_type> transactions_type { get; set; }
        public DbSet<WhereHosing> whereHosings { get; set; }
        public DbSet<WhereHosing_Product> whereHosing_Products { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<WhereHosing_Product>()
        .HasKey(wp => new { wp.Product_Id, wp.WhereHosing_Id });

    modelBuilder.Entity<WhereHosing_Product>()
        .HasOne(wp => wp.Product)
        .WithMany(p => p.WhereHosing_Products)
        .HasForeignKey(wp => wp.Product_Id);

            modelBuilder.Entity<WhereHosing_Product>()
                .HasOne(wp => wp.WhereHosing)
                .WithMany(w => w.WhereHosing_Products)
                .HasForeignKey(wp => wp.WhereHosing_Id);

            // Default IsDeleted = false
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(BaseModel.IsDelete))
                        .HasDefaultValue(false);
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseModel.IsDelete));
                    var filter = Expression.Lambda(
                        Expression.Equal(property, Expression.Constant(false)),
                        parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
          


        }

        }
    }

using GR.Core.Entities.Identity;
using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using GR.Models.Entities.Property;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //DbSets for your entities can be added here
        // Example: public DbSet<YourEntity> YourEntities { get; set; }
        /// <summary>
        /// 

        public virtual DbSet<HomeBanner> HomeBanners { get; set; }
        public virtual DbSet<HomeSection> HomeSection { get; set; }
        public virtual DbSet<ContactRequest> ContactRequests { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<HomeContact> HomeContacts { get; set; }
        public virtual DbSet<HomeCounter> HomeCounters { get; set; }
        public virtual DbSet<CustomerReview> CustomerReviews { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Neighborhood> Neighborhood { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyCategory> PropertyCategories { get; set; }
        public virtual DbSet<PropertyPhoto> PropertyPhotos { get; set; }
        public virtual DbSet<PropertySubtype> PropertySubtypes { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
            base.OnModelCreating(builder);

            builder.Entity<Property>(e =>
            {
                e.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.District).WithMany().HasForeignKey(x => x.DistrictId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Neighborhood).WithMany().HasForeignKey(x => x.NeighborhoodId).OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<District>(e =>
            {
                e.HasOne(x => x.City).WithMany(x => x.Districts).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Neighborhood>(e =>
            {
                e.HasOne(x => x.District).WithMany(x => x.Neighborhoods).HasForeignKey(x => x.DistrictId).OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Property>()
            .Property(p => p.IsActive)
            .HasDefaultValue(true);

            builder.Entity<PropertyPhoto>()
                .HasIndex(p => p.PropertyId)
                .HasFilter("[IsCover] = 1")  // sadece IsCover=1 için
                .IsUnique();                 // tek kayıt

            builder.Entity<PropertyPhoto>()
                .HasOne(ph => ph.Property)
                .WithMany(p => p.PropertyPhotos)
                .HasForeignKey(ph => ph.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
               .Property(u => u.IsActive)
               .HasDefaultValue(true);


        }
    }
}

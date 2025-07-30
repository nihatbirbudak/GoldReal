using GR.Core.Entities.Identity;
using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Additional model configurations can be added here
        }
    }
}

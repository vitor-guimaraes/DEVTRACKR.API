using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEVTRACKR.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DEVTRACKR.API.Persistence
{
    public class DevTrackRContext : DbContext
    {
        // public DevTrackRContext()
        // {
        //     Packages = new List<Package>(); 
        // }
        // public List<Package> Packages { get; set;}
        public DevTrackRContext(DbContextOptions<DevTrackRContext> options)
             : base(options)
        {

        }
        public DbSet<Package> Packages { get; set; }

        public DbSet<PackageUpdate> PackageUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Package>(e => {
                e.HasKey(p => p.Id);         
                

                e
                .HasMany(p => p.Updates)
                .WithOne()
                .HasForeignKey(pu => pu.PackageId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
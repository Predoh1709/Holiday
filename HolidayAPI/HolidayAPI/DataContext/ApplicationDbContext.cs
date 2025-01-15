using HolidayAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HolidayAPI.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayVariableDate> HolidayVariableDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holiday>()
                .HasMany(h => h.VariableDates)
                .WithOne(vd => vd.Holiday)
                .HasForeignKey(vd => vd.HolidayId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

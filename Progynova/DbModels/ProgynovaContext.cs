using Microsoft.EntityFrameworkCore;

namespace Progynova.DbModels
{
    public class ProgynovaContext : DbContext
    {
        public ProgynovaContext(DbContextOptions<ProgynovaContext> context) : base(context)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.CreatedCourses)
                .WithOne(c => c.Creator)
                .HasForeignKey(c => c.CreatorId);

            builder.Entity<User>()
                .HasMany(u => u.SelectedCourses)
                .WithMany(c => c.Selectee);

            builder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons);
        }
    }
}
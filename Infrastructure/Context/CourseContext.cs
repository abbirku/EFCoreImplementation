using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class CourseContext : DbContext, ICourseContext
    {
        private string _connectionString;
        private string _migrationAssemblyName;

        public CourseContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>()
                .HasKey(e=>e.Id);

            builder.Entity<Course>()
                .HasKey(e => e.Id);

            builder.Entity<StudentRegistration>()
                .HasOne(pc => pc.Student)
                .WithMany(p => p.Courses)
                .HasForeignKey(pc => pc.StudentId);

            builder.Entity<StudentRegistration>()
                .HasOne(pc => pc.Course)
                .WithMany(c => c.Students)
                .HasForeignKey(pc => pc.CourseId);

            base.OnModelCreating(builder);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
    }
}

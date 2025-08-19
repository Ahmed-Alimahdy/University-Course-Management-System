using Microsoft.EntityFrameworkCore;
using universityManagementSys.Models;


namespace universityManagementSys.Data
{
    public class Context : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Student> students { get; set; }
        public DbSet<Instructor> instructors { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Grade> grades { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Semester> semesters { get; set; }
        public DbSet<DepartmentCourse> departmentCourses { get; set; }
        public DbSet<Enrollment> enrollments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;" + 
                "Initial Catalog=test1;" +               
                "Integrated Security=True;" +            
                "Encrypt=False;" +
                "TrustServerCertificate=True"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne<Department>(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .Property(s => s.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Instructor>()
                .HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Instructor>()
                .Property(i => i.ID)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Course>()
                .HasOne<Semester>(c => c.Semester)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.SemesterID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .Property(c => c.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Grade>()
                .Property(g => g.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Department>()
                .Property(d => d.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Semester>()
                .Property(s => s.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DepartmentCourse>().HasKey(dc => new { dc.DepartmentID, dc.CourseID });

            modelBuilder.Entity<DepartmentCourse>()
                .HasOne(dc => dc.Department)
                .WithMany(d => d.DepartmentCourses)
                .HasForeignKey(dc => dc.DepartmentID);

            modelBuilder.Entity<DepartmentCourse>()
                .HasOne(dc => dc.Course)
                .WithMany(c => c.DepartmentCourses)
                .HasForeignKey(dc => dc.CourseID);

            modelBuilder.Entity<Enrollment>().HasKey(e => new { e.StudentID, e.CourseID });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Grade)
                .WithMany(g => g.Enrollments)
                .HasForeignKey(e => e.GradeID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

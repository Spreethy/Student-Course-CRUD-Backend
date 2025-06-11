using StudentDatabaseApi.Models;
using System.Data.Entity;

namespace StudentApi.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext() : base("name=StudentDbConnectionString")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}

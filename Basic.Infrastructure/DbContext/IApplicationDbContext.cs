using Basic.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Basic.Infrastructure.DbContexts
{
    public interface IApplicationDbContext
    {
        DbSet<Course> Courses { get; set; }
        //DbSet<Student> Students { get; set; }
    }
}
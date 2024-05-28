using Microsoft.EntityFrameworkCore;

namespace MyToDoList.Models
{
    public class ToDoDBContext : DbContext
    {
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options)
           : base(options) { }

        public DbSet<ToDo> ToDos { get; set; } = null!;

        public DbSet<Subject> Subjects { get; set; } = null!;

        public DbSet<Status> Status { get; set; } = null!;



        //seed data

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasData(

                  new Subject { SubjectId = "P231", Name = "Programming" },
                  new Subject { SubjectId = "L232", Name = "French" },
                  new Subject { SubjectId = "B234", Name = "Science" },
                  new Subject { SubjectId = "Cal31", Name = "Calculus" },
                  new Subject { SubjectId = "Chem244", Name = "Chemistry" },
                  new Subject { SubjectId = "Phy201", Name = "Physics" }


                ); 
            
            modelBuilder.Entity<Status>().HasData
                (
               new Status { StatusId = "open", Name = "In Progress" },
                new Status { StatusId = "closed", Name = "Completed" },
                 new Status { StatusId = "Not Started", Name = "Pending" }

                );




        }
    }
}

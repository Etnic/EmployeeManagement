using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Employee>().HasData(
            //     new Employee
            //     {
            //         Id = 1,
            //         Name = "mary",
            //         Department = Dept.It,
            //         Email = "mary@gmail.com"
            //     },
            //     new Employee
            //     {
            //         Id = 2,
            //         Name = "Mark",
            //         Department = Dept.It,
            //         Email = "mark@gmail.com"
            //     });
        }
    }
}

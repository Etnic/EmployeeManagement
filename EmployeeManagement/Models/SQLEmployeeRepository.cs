using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext appDbContext;

        public SQLEmployeeRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Employee Add(Employee employee)
        {
            this.appDbContext.Add(employee);
            this.appDbContext.SaveChanges();
            return employee;
        }

        public void Delete(int id)
        {
            var employee = this.appDbContext.Employees.Find(id);

            if (employee != null)
            {
                this.appDbContext.Employees.Remove(employee);
                this.appDbContext.SaveChanges();
            }
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            var reds = this.appDbContext.Employees.ToList();
            return reds;
        }

        public Employee GetEmployee(int id)
        {
            return this.appDbContext.Employees.Find(id);
        }

        public Department GetDepartment(int id)
        {
            return this.appDbContext.Departments.Find(id);
        }

        public Employee Update(Employee employee)
        {
            var employeeChanged = this.appDbContext.Employees.Attach(employee);
            employeeChanged.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            this.appDbContext.SaveChanges();

            return employee;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> employees;

        public MockEmployeeRepository()
        {
            this.employees = new List<Employee>()
            {
                //new Employee(){ Name = "Fero", Department=Dept.Development, Email="fero@gmail.com", Id=0},
                //new Employee(){ Name = "Jano", Department=Dept.HR, Email="jano@gmail.com", Id=1},
                //new Employee(){ Name = "Peto", Department=Dept.Payroll, Email="peto@gmail.com", Id=2},
            };
        }

        public Employee Update(Employee employee)
        {
            var oldEmployeeIndex = this.employees.FindIndex(x => x.Id == employee.Id);
            this.employees[oldEmployeeIndex] = employee;

            return employee;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = employees.Count + 1;
            employees.Add(employee);

            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return this.employees;
        }

        public Employee GetEmployee(int id)
        {
            return this.employees.First(x => x.Id == id);
        }

        public void Delete(int id)
        {
            this.employees.Remove(this.employees.First(x => x.Id == id));
        }

        public Department GetDepartment(int id)
        {
            throw new NotImplementedException();
        }
    }
}

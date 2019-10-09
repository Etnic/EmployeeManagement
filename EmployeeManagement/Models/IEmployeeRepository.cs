using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
     public interface IEmployeeRepository
    {
        Employee GetEmployee(int id);
        Department GetDepartment(int id);
        IEnumerable<Employee> GetAllEmployees();
        Employee Add(Employee employeeCreateViewModel);
        Employee Update(Employee employee);
        void Delete(int id);
    }
}

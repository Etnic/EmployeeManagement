using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.ViewComponents
{
    public class CreateViewComponent : ViewComponent
    {
        public DbContextOptions<AppDbContext> Options { get; }

        public CreateViewComponent(DbContextOptions<AppDbContext> options)
        {
            this.Options = options;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            EmployeeCreateViewModel employeeCreateViewModel = new EmployeeCreateViewModel();

            var departments = await GetDepartmentsAsync();

            employeeCreateViewModel.departmentsSource = departments.Select(x => new SelectListItem { Text = x.Name, Value = x.IdDepartment.ToString() }).ToList();

            return View(employeeCreateViewModel);            
        }


        private async Task<List<Department>> GetDepartmentsAsync()
        {
            EmployeeCreateViewModel employeeCreateViewModel = new EmployeeCreateViewModel();
            using (AppDbContext appDbContext = new AppDbContext(this.Options))
            {
                var dept = await appDbContext.Departments.ToListAsync();
                return dept;
            };
        }
    }
}

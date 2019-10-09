using EmployeeManagement.Filter;
using EmployeeManagement.Filters;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IHostingEnvironment hostingEnviroment;

        public DbContextOptions<AppDbContext> Options { get; }

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnviroment, DbContextOptions<AppDbContext> options)
        {
            this.employeeRepository = employeeRepository;
            this.hostingEnviroment = hostingEnviroment;
            Options = options;
        }

        [Route("")]
        public ViewResult Index()
        {
            var model = this.employeeRepository.GetAllEmployees();
            return View(model);
        }

        [ActionFilter]
        public ViewResult Details(int id)
        {
            var employee = this.employeeRepository.GetEmployee(id);            

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            var department = this.employeeRepository.GetDepartment(employee.IdDepartment);

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                Department = department,
                PageTitle = "Detail title"
            };

            return View(homeDetailsViewModel);
        }

        // [ServiceFilter(typeof(AddHeaderResultServiceFilter))]
        [AsyncActionFilter]
        [HttpGet]
        public ViewResult Create()
        {
            EmployeeCreateViewModel employeeCreateViewModel = new EmployeeCreateViewModel();
            using (AppDbContext appDbContext = new AppDbContext(this.Options))
            {
                // employeeCreateViewModel.departmentsSource = appDbContext.Departments.ToList().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Name, Value = x.IdDepartment.ToString() }).ToList();
                employeeCreateViewModel.Name = "Default";
            };

            return View(employeeCreateViewModel);
        }

        [ActionFilter]
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel employeeCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadPhoto(employeeCreateViewModel);

                Employee newEmployee = new Employee()
                {
                    Name = employeeCreateViewModel.Name,
                    IdDepartment = employeeCreateViewModel.IdDepartment,
                    Email = employeeCreateViewModel.Email,
                    PhotoPath = uniqueFileName
                };

                this.employeeRepository.Add(newEmployee);

                return RedirectToAction("Details", new { id = newEmployee.Id });
            }

            return View();
        }

        [HttpGet]
        [ActionFilter]
        public ViewResult Edit(int id)
        {
            var newEmployee = this.employeeRepository.GetEmployee(id);

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = newEmployee.Id,
                Name = newEmployee.Name,
                Email = newEmployee.Email,
                IdDepartment = newEmployee.IdDepartment,
                ExistingPhotoPath = newEmployee.PhotoPath
            };

            using (AppDbContext appDbContext = new AppDbContext(this.Options))
            {
                employeeEditViewModel.departmentsSource = appDbContext.Departments.ToList().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.Name, Value = x.IdDepartment.ToString() }).ToList();
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employeeEditViewModel)
        {
            if (ModelState.IsValid)
            {
                Employee employee = this.employeeRepository.GetEmployee(employeeEditViewModel.Id);
                employee.Name = employeeEditViewModel.Name;
                employee.Email = employeeEditViewModel.Email;
                employee.IdDepartment = employeeEditViewModel.IdDepartment;

                if (employeeEditViewModel.Photo != null)
                {
                    if (employeeEditViewModel.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnviroment.WebRootPath, "Images", employeeEditViewModel.ExistingPhotoPath);
                        bool canBeDeleted = true;

                        foreach (var item in this.employeeRepository.GetAllEmployees())
                        {
                            if (item.PhotoPath == employeeEditViewModel.ExistingPhotoPath)
                            {
                                canBeDeleted = false;
                                break;
                            }
                        }

                        if (canBeDeleted)
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    employee.PhotoPath = ProcessUploadPhoto(employeeEditViewModel);
                }

                this.employeeRepository.Update(employee);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            this.employeeRepository.Delete(id);

            return RedirectToAction("Index");
        }

        private string ProcessUploadPhoto(EmployeeCreateViewModel employeeCreateViewModel)
        {
            string uniqueFileName = null;
            if (employeeCreateViewModel.Photo != null)
            {
                string pathFolder = Path.Combine(hostingEnviroment.WebRootPath, "Images");

                foreach (var item in employeeCreateViewModel.Photo)
                {
                    uniqueFileName = Path.GetFileName(item.FileName);
                    string filePath = Path.Combine(pathFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                    }

                }
            }

            return uniqueFileName;
        }
    }
}

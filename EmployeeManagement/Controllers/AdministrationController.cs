using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Filter;
using EmployeeManagement.Filters;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.Controllers
{
    //[Authorize(Roles = "Admin")]
    [AddHeader("Author", "Joe Smith")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel createRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var identityRole = new IdentityRole()
                {
                    Name = createRoleViewModel.RoleName
                };

                var result = await this.roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("listroles", "administration");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(createRoleViewModel);
        }

        public IActionResult ListRoles()
        {
            var listRoles = this.roleManager.Roles;
            return View(listRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = "role was not found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in this.userManager.Users)
            {
                if (await this.userManager.IsInRoleAsync(user, model.RoleName))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [ActionFilter]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var identityRole = this.roleManager.Roles.First(x => x.Id == editRoleViewModel.Id);

            identityRole.Name = editRoleViewModel.RoleName;

            var result = await this.roleManager.UpdateAsync(identityRole);

            if (result.Succeeded)
            {
                return RedirectToAction("listroles", "administration");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Cancel()
        {
            return RedirectToAction("listroles", "administration");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var identityRole = await this.roleManager.FindByIdAsync(id);

            if (identityRole != null)
            {
                foreach (var user in this.userManager.Users)
                {

                    if (await this.userManager.IsInRoleAsync(user, identityRole.Name))
                    {
                        var result = await this.userManager.RemoveFromRoleAsync(user, identityRole.Name);
                    }
                }

                await this.roleManager.DeleteAsync(identityRole);
            }

            return RedirectToAction("listroles", "administration");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleName)
        {
            ViewBag.roleId = roleName;

            var identityRole = await this.roleManager.FindByNameAsync(roleName);
            var model = new List<UserRoleViewModel>();

            if (identityRole != null)
            {
                foreach (var user in this.userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await this.userManager.IsInRoleAsync(user, roleName))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }

                    model.Add(userRoleViewModel);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleName)
        {
            var users = this.userManager.Users;

            foreach (var item in model)
            {
                var user = users.First(x => x.UserName == item.UserName);
                var isInRole = await this.userManager.IsInRoleAsync(user, roleName);

                if (item.IsSelected && !isInRole)
                {
                    var result = await this.userManager.AddToRoleAsync(user, roleName);
                }
                else if (!item.IsSelected && isInRole)
                {
                    var removeResult = await this.userManager.RemoveFromRoleAsync(user, roleName);
                }
            }

            return RedirectToAction("listroles", "administration");
        }

        public IActionResult ListUsers()
        {
            var users = this.userManager.Users;
            return View(users);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = this.userManager.Users.First(x => x.Id == id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userRoles = await this.userManager.GetRolesAsync(user);
            var userClaims = await this.userManager.GetClaimsAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            var oldUser = this.userManager.Users.First(x => x.Id == editUserViewModel.Id);

            oldUser.UserName = editUserViewModel.UserName;
            oldUser.Email = editUserViewModel.Email;
            oldUser.City = editUserViewModel.City;

            var result = await this.userManager.UpdateAsync(oldUser);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var result = await this.userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("ListUsers");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something before the action executes.
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
            base.OnActionExecuted(context);
        }
    }
}
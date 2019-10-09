using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Filter
{
    public class ActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            EditRoleViewModel editRoleViewModel = new EditRoleViewModel();

            var queryString = context.HttpContext.Request.QueryString.Value;

            editRoleViewModel.TestFilter = queryString.Substring(queryString.IndexOf('=') + 1);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
        }
    }
}

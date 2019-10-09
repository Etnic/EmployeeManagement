using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("/Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "sorry, resource could not be found";
                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QS = statusCodeResult.OriginalQueryString;

                    this.logger.LogWarning($"War1 : exception path {statusCodeResult.OriginalPath} and qs {statusCodeResult.OriginalQueryString}");

                    break;
            }

            return View("NotFound");
        }

        [Route("/Error/")]
        [AllowAnonymous]
        public IActionResult Error(int statusCode)
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //ViewBag.ExceptionPath = exceptionDetails.Path;
            //ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            //ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            logger.LogError($"Err1 : exception path {exceptionDetails.Path} throw error {exceptionDetails.Error}");

            return View("Error");

        }
    }
}

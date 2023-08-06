using DocumentFormat.OpenXml.EMMA;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.ViewModels.Common;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class AuthenticationController : AdminBaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult UnAuthorized()
        {
            return View("~/Areas/Admin/Views/TicketBox/Index.cshtml");
        }
        public ActionResult AccessDenied()
        {
           // TempData["AuthorizeMessage"] = "Bu sayfaya erişim yetkiniz yoktur!";
            return View();  
        }
        public ActionResult NotAuthorized()
        {
            ViewBag.clientside_js = "<script type=\"text/javascript\">  $(function () {\r\n       if (\"@Model\" != \"\")\r\n \r\n        {\r\n \r\n           toastr.error(\"Bu işlem için yetkiniz yok!\");\r\n \r\n        };\r\n\r\n    }) </script>";
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Bu işlem için yetkiniz yok!");
            //var callResult = new ServiceCallResult() { Success = false };
            //callResult.Success = false;
            //callResult.ErrorMessages.Add("ver yetkiyi gör etkiyi");
            //return Json(
            // new
            // {
            //     success = false,
            //     errorMessages = callResult.ErrorMessages
            // });


        }
    }
}
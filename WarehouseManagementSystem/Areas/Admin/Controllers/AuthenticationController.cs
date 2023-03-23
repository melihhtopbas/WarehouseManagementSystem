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
        [HttpGet]
        public ActionResult UnAuthorized()
        {
            TempData["AuthorizeMessage"] = "Bu sayfaya erişim yetkiniz yoktur!";
            return View();
        }
        public ActionResult AccessDenied()
        {
            TempData["AuthorizeMessage"] = "Bu sayfaya erişim yetkiniz yoktur!";
            return View();  
        }
    }
}
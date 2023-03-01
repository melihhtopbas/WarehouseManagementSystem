using DocumentFormat.OpenXml.EMMA;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserProfile()
        {
            return View();
        }
        [HttpGet,AjaxOnly]
        public ActionResult ChangePassword()
        {
            return PartialView("~/Areas/Admin/Views/User/ChangePassword.cshtml");
        }
    }
}
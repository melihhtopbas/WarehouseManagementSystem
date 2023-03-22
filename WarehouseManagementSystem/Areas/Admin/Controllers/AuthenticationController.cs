using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class AuthenticationController : AdminBaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}
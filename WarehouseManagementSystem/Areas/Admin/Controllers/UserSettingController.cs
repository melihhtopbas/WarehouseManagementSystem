using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class UserSettingController : AdminBaseController
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public ActionResult Index()
        {
            return View();
        }
    }
}
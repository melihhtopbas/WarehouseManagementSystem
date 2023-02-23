using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers.CommonController
{
    public class AdminCommonController : Controller
    {
        private readonly LanguageService _languageService;
        // GET: Admin/AdminCommon
        public AdminCommonController(LanguageService languageService)
        {
            _languageService = languageService;


        }
        // GET: Common

        public ActionResult Sidebar()
        {

            var model = _languageService.GetLanguageListView();
            return PartialView("~/Areas/Admin/Views/AdminCommon/Sidebar.cshtml", model);
        }
    }
}
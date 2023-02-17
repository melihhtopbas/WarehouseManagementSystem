using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;

namespace WarehouseManagementSystem.Controllers
{
    public class AboutController : Controller
    {
        private readonly SettingService _settingService;

        public AboutController(SettingService settingService)
        {
            _settingService = settingService;
        }


        // GET: About
        public ActionResult Index(string lang)
        {
            var model = _settingService.GetAboutViewModel(lang);
            
            return View(model);
        }
    }
}
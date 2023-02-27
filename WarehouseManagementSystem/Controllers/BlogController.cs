using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;

namespace WarehouseManagementSystem.Controllers
{
    public class BlogController : BaseController
    {

        private readonly SettingService _settingService;

        public BlogController(SettingService settingService)
        {
            _settingService = settingService;
        }


        // GET: Blog
        public ActionResult Index(string lang)
        {
            var model = _settingService.GetBlogViewModel(lang);

            return View(model);
        }
    }
}
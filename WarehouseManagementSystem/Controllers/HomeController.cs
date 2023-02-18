using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;
using WarehouseManagementSystem.Controllers;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly SliderService _sliderService;

        private readonly PropertyService _propertyService;
        private readonly SettingService _settingService;
        public HomeController(SliderService sliderService, PropertyService propertyService, SettingService settingService)
        {
            _sliderService = sliderService;
            _propertyService = propertyService;
            _settingService = settingService;
        }
        public ActionResult Index(string lang)
        {
            return View();
        }
        public ActionResult SliderPartial()
        {
            string lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var model = _sliderService.GetSliderListIQueryable().ToList();
            return PartialView("~/Views/Home/SliderPartial.cshtml", model);

        }
        public ActionResult HomePageProperties()
        {
            string lang = "tr";
            var model = _propertyService.GetHomePagePropertyListIQueryable(lang).OrderBy(a => a.Id).ToList();
            return PartialView("~/Views/Home/HomePagePropertyPartial.cshtml", model);
        }
        public ActionResult HomePageAbout()
        {
            string lang = "tr";
            var model = _settingService.GetAboutViewModel(lang);
            return PartialView("~/Views/Home/HomePageAboutPartial.cshtml", model);
        }
    }
}
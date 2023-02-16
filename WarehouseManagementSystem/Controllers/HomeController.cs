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
        public HomeController(SliderService sliderService, PropertyService propertyService)
        {
            _sliderService = sliderService;
            _propertyService = propertyService;
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
            var model = _propertyService.GetHomePagePropertyListIQueryable(lang).OrderBy(a => Guid.NewGuid()).ToList();
            return PartialView("~/Views/Home/HomePagePropertyPartial.cshtml", model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;
using WarehouseManagementSystem.Controllers;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly SliderService _sliderService;

        private readonly PropertyService _propertyService;
        private readonly SettingService _settingService;
        private readonly ReferenceService _referenceService;
        private readonly PageService _pageService;
        public HomeController(SliderService sliderService, PropertyService propertyService, SettingService settingService, ReferenceService referenceService, PageService pageService)
        {
            _sliderService = sliderService;
            _propertyService = propertyService;
            _settingService = settingService;
            _referenceService = referenceService;
            _pageService = pageService;
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
        public ActionResult HomePageReferences()
        {
            var model = _referenceService.GetHomePageReferencesListIQueryable().ToList();
            return PartialView("~/Views/Home/HomePageReferencesPartial.cshtml", model);
        }
        public ActionResult HomePage()
        {
            string lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var model = _pageService.GetHomePageListIQueryable(lang).ToList();
            return PartialView("~/Views/Home/HomePagePartial.cshtml", model);
        }
    }
}
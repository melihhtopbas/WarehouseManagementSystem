using DocumentFormat.OpenXml.EMMA;
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Warehouse.Service.Admin;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class CountrySettingController : AdminBaseController
    {
        private readonly CountryService _countryService;
        private readonly LanguageService _languageService;
        public CountrySettingController(LanguageService languageService, CountryService countryService)
        {
            _languageService = languageService;
            _countryService = countryService;
        }
        // GET: Admin/CountrySetting
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Servisler";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();
            return View();
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CountryList(OrderSearchViewModel searchViewModel, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _countryService.GetCountryListIQueryable(searchViewModel)
                .OrderBy(p => p.Name)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultServicePageSize);

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();
            ModelState.Clear();

            ViewBag.LanguageId = searchViewModel.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CountrySetting/CountryList.cshtml", result)
                })
            };
        }
    }
}
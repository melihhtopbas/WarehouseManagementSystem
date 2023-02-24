 
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Warehouse.Data;
using Warehouse.Service.Admin;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class CountrySettingController : AdminBaseController
    {
        private readonly WarehouseManagementSystemEntities1 _context;
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
            
            ViewBag.Title = "Ülkeler";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();
            return View("~/Areas/Admin/Views/CountrySetting/Index.cshtml");
        }
       
       
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CountryList(CountrySearchViewModel searchViewModel, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _countryService.GetCountryListIQueryable(searchViewModel)
                .OrderBy(x => x.Name)   
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCountryPageSize, page);

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
        [AjaxOnly]
        public ActionResult Add(long languageId)
        {



            var model = new CountryViewModel
            {

            };
            return PartialView("~/Areas/Admin/Views/CountrySetting/_CountryAdd.cshtml", model);
        }


    }
}
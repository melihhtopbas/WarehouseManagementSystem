 
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
        private readonly OrderService _orderService;
        public CountrySettingController(LanguageService languageService, CountryService countryService, OrderService orderService)
        {
            _languageService = languageService;
            _countryService = countryService;
            _orderService = orderService;
        }

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
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCountryPageSize);

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
        [HttpGet]
        public ActionResult Add(long languageId)
        {

            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();

            var model = new CountryViewModel
            {
                Active = false,
                LanguageId = languageId,
            }; 
            return PartialView("~/Areas/Admin/Views/CountrySetting/_CountryAdd.cshtml", model);
           
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CountryViewModel model)
        {
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();

            if (ModelState.IsValid)
            {
                var callResult = await _countryService.AddCountryAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CountryListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CountrySetting/DisplayTemplates/CountryListViewModel.cshtml", viewModel),
                            item = viewModel
                        });
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CountrySetting/_CountryAdd.cshtml", model)
                });

        }
        public async Task<ActionResult> Edit(int countryId)
        {
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            var model = await _countryService.GetCountryEditViewModelAsync(countryId);
            if (model != null)
            {
                 
                return PartialView("~/Areas/Admin/Views/CountrySetting/_CountryEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Servis sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CountryViewModel model)
        {
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            if (ModelState.IsValid)
            {
                var callResult = await _countryService.EditCountryAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CountryListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CountrySetting/DisplayTemplates/CountryListViewModel.cshtml", viewModel),
                            item = viewModel
                        });
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }
            
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CountrySetting/_CountryEdit.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int countryId)
        {
            var callResult = await _countryService.DeleteCountryAsync(countryId);
            if (callResult.Success)
            {

                ModelState.Clear();

                return Json(
                    new
                    {
                        success = true,
                        warningMessages = callResult.WarningMessages,
                        successMessages = callResult.SuccessMessages,
                    });
            }

            return Json(
                new
                {
                    success = false,
                    errorMessages = callResult.ErrorMessages
                });

        }


    }
}
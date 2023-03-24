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
using Warehouse.Service.WebSite;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.Areas.Security;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [CustomAuthorize("admin")]
    public class CitySettingController : AdminBaseController
    {
        private readonly LanguageService _languageService;
        private readonly CountryService _countryService;
        private readonly CityService _cityService;

        public CitySettingController(LanguageService languageService, CountryService countryService, CityService cityService)
        {
            _languageService = languageService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Şehirler";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            return View("~/Areas/Admin/Views/CitySetting/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]

        public async Task<ActionResult> CityList(CitySearchViewModel model, int? page)
            {
            if (model.CountryName != null || model.Name!=null)
            {
                ViewData["CountrySearchName"] = "";
                ViewData["CountrySearchName"] = model.CountryName;

                ViewData["CitySearchName"] = "";
                ViewData["CitySearchName"] = model.Name;
            }
             
            var currentPageIndex = page - 1 ?? 0;

            var result = _cityService.GetCityListIQueryable(model)
                .OrderBy(p => p.Name).OrderBy(x=>x.CountryName)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCityPageSize);

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            ModelState.Clear();
            ViewBag.LanguageId = model.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CitySetting/CityList.cshtml", result)
                })
            };
        }
        [AjaxOnly, HttpGet]
        public ActionResult Add(long languageId)
        {

            ViewData["Countries"] = _countryService.GetCountryList();
            var model = new CityAddViewModel()
            {
                LanguageId= languageId,
                Active = false,

            };
            return PartialView("~/Areas/Admin/Views/CitySetting/_CityAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CityAddViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _cityService.AddCityAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CityListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CitySetting/DisplayTemplates/CityListViewModel.cshtml", viewModel),
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
            ViewData["Countries"] = _countryService.GetCountryList();
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CitySetting/_CityAdd.cshtml", model)
                });

        }

        public async Task<ActionResult> Edit(int cityId)
        {
            var model = await _cityService.GetCityEditViewModelAsync(cityId);
            if (model != null)
            {
                ViewData["Countries"] = _countryService.GetCountryList();
                return PartialView("~/Areas/Admin/Views/CitySetting/_CityEdit.cshtml", model);
            }

            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Şehir sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CityEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var callResult = await _cityService.EditCityAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CityListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CitySetting/DisplayTemplates/CityListViewModel.cshtml", viewModel),
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
            ViewData["Countries"] = _countryService.GetCountryList();
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CitySetting/_CityEdit.cshtml", model)
                });

        }

        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int cityId)
        {
            var callResult = await _cityService.DeleteCityAsync(cityId);
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
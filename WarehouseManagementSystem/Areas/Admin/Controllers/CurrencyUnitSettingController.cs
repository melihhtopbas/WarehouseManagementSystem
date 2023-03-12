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
    public class CurrencyUnitSettingController : AdminBaseController
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly CurrencyUnitService _currencyUnitService;  
        private readonly LanguageService _languageService;

        public CurrencyUnitSettingController(WarehouseManagementSystemEntities1 context, CurrencyUnitService currencyUnitService, LanguageService languageService)
        {
            _context = context;
            _currencyUnitService = currencyUnitService;
            _languageService = languageService;
        }
        public async Task<ActionResult> Index()
        {

            ViewBag.Title = "Para Birimleri";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();
          
            return View("~/Areas/Admin/Views/CurrencyUnitSetting/Index.cshtml");

        }


        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CurrencyUnitList(CurrencyUnitSearchViewModel searchViewModel, int? page)
        {


            var currentPageIndex = page - 1 ?? 0;

            var result = _currencyUnitService.GetCurrencyUnitListIQueryable(searchViewModel)
                .OrderBy(x => x.Name)
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CurrencyUnitSetting/CurrencyUnitList.cshtml", result)
                })
            };
        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add(long languageId)
        {



            var model = new CurrencyUnitViewModel
            {

                LanguageId = languageId,
            };
            return PartialView("~/Areas/Admin/Views/CurrencyUnitSetting/_CurrencyUnitAdd.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CurrencyUnitViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _currencyUnitService.AddCurrencyUnitAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CurrencyUnitListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CurrencyUnitSetting/DisplayTemplates/CurrencyUnitListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CurrencyUnitSetting/_CurrencyUnitAdd.cshtml", model)
                    
                });

        }
        public async Task<ActionResult> Edit(int currencyUnitId)
        {

            var model = await _currencyUnitService.GetCurrencyUnitEditViewModelAsync(currencyUnitId);
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/CurrencyUnitSetting/_CurrencyUnitEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Servis sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CurrencyUnitViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _currencyUnitService.EditCurrencyUnitAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CurrencyUnitListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CurrencyUnitSetting/DisplayTemplates/CurrencyUnitListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CurrencyUnitSetting/_CurrencyUnitEdit.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int currencyUnitId)
        {
            var callResult = await _currencyUnitService.DeleteCurrencyUnitAsync(currencyUnitId);
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
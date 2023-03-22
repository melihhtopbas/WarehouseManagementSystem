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
using WarehouseManagementSystem.Areas.Security;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [CustomAuthorize("admin")]
    public class CargoServiceSettingController : AdminBaseController
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly CargoServiceTypeService _cargoService;
        private readonly LanguageService _languageService;

        public CargoServiceSettingController(WarehouseManagementSystemEntities1 context, CargoServiceTypeService cargoService, LanguageService languageService)
        {
            _context = context;
            _cargoService = cargoService;
            _languageService = languageService; 
        }
        public async Task<ActionResult> Index()
        {

            ViewBag.Title = "Kargo Servisleri";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            return View("~/Areas/Admin/Views/CargoServiceSetting/Index.cshtml");

        }


        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CargoServiceList(CargoServiceSearchViewModel searchViewModel, int? page)
        {


            var currentPageIndex = page - 1 ?? 0;

            var result = _cargoService.GetCargoServiceListIQueryable(searchViewModel)
                .OrderBy(x => x.Name)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCargoServicePageSize);

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            ModelState.Clear();
            ViewBag.LanguageId = searchViewModel.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CargoServiceSetting/CargoServiceList.cshtml", result)
                })
            };
        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add(long languageId)
        {



            var model = new CargoServiceTypeViewModel
            {
                 
                LanguageId = languageId,
            };
            return PartialView("~/Areas/Admin/Views/CargoServiceSetting/_CargoServiceAdd.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(CargoServiceTypeViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _cargoService.AddCargoServiceAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CargoServiceTypeListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CargoServiceSetting/DisplayTemplates/CargoServiceTypeListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CargoServiceSetting/_CargoServiceAdd.cshtml", model)
                });

        }
        public async Task<ActionResult> Edit(int cargoServiceId)
        {

            var model = await _cargoService.GetCargoServiceEditViewModelAsync(cargoServiceId);
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/CargoServiceSetting/_CargoServiceEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Servis sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CargoServiceTypeViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _cargoService.EditCargoServiceAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CargoServiceTypeListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CargoServiceSetting/DisplayTemplates/CargoServiceTypeListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CargoServiceSetting/_CargoServiceEdit.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int cargoServiceId)
        {
            var callResult = await _cargoService.DeleteCargoServiceAsync(cargoServiceId);
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
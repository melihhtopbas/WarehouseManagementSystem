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
using WarehouseManagementSystem.Areas.Admin.Controllers;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class ShippingPriceSettingController : AdminBaseController
    {
        private readonly ShippingPriceService _shippingPriceService;
        private readonly LanguageService _languageService;
        private readonly OrderService _orderService;

        public ShippingPriceSettingController(ShippingPriceService shippingPriceService, LanguageService languageService, OrderService orderService)
        {
            _shippingPriceService = shippingPriceService;
            _languageService = languageService;
            _orderService = orderService;
        }

        public async Task<ActionResult> Index()
        {

            ViewBag.Title = "Ülkeler";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            return View("~/Areas/Admin/Views/ShippingPriceSetting/Index.cshtml");

        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> ShippingPriceList(ShippingPriceSearchViewModel searchViewModel, int? page)
        {


            var currentPageIndex = page - 1 ?? 0;

            var result = _shippingPriceService.GetShippingPriceListIQueryable(searchViewModel)
                .OrderBy(x => x.CountryName)
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/ShippingPriceSetting/ShippingPriceList.cshtml", result)
                })
            };
        }
        [HttpGet,AjaxOnly]
        public async Task<ActionResult> Edit(int shipPriceId)
        {
            var countryShippingPrice = _shippingPriceService.ShippingPriceViewModel(shipPriceId);
            var model = new ShippingPriceViewModel
            {
                CountryShippingPriceViewModels = _shippingPriceService.GetShippingPriceEditViewModelAsync(shipPriceId),
                CountryName = countryShippingPrice.CountryName,
                Id = countryShippingPrice.Id,
                LanguageId = countryShippingPrice.LanguageId
                
            };
            if (model != null)
            {
                
                return PartialView("~/Areas/Admin/Views/ShippingPriceSetting/_ShippingPriceEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Servis sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ShippingPriceViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _shippingPriceService.EditShippingPriceAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (ShippingPriceListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/ShippingPriceSetting/DisplayTemplates/ShippingPriceListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/ShippingPriceSetting/_ShippingPriceEdit.cshtml", model)
                });

        }

    }
}
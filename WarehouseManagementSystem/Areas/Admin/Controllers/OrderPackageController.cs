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
    public class OrderPackageController : AdminBaseController
    {
        private readonly OrderPackageService _orderPackageService;
        private readonly LanguageService _languageService;
        public OrderPackageController(OrderPackageService orderPackageService, LanguageService languageService)
        {
            _orderPackageService = orderPackageService;
            _languageService = languageService;
        }

        public async Task<ActionResult> Index(long? packageId, long? searchId)
        {

            ViewBag.Title = "Sipariş Paketleri";

            var model = new OrderPackageSearchViewModel
            {
                SearchId = searchId,
                PackageId = packageId

            };
            return View("~/Areas/Admin/Views/OrderPackage/Index.cshtml", model);

        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> OrderPackageList(OrderPackageSearchViewModel searchViewModel, int? page)
        {


            var currentPageIndex = page - 1 ?? 0;

            var result = _orderPackageService.GetOrderPackageListIQueryable(searchViewModel)
                .OrderBy(x => x.Id)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultCountryPageSize);



            ModelState.Clear();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/OrderPackage/OrderPackageList.cshtml", result)
                })
            };
        }

        public async Task<ActionResult> OrderPackage(long? packageId, long? searchId)
        {

            ViewBag.Title = "Sipariş Paketleri";

            var model = new OrderPackageSearchViewModel
            {
                SearchId = searchId,
                PackageId = packageId

            };
            return View("~/Areas/Admin/Views/OrderPackage/OrderPackage.cshtml", model);

        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> PackageList(OrderPackageSearchViewModel searchViewModel, int? page)
        {


            

            var result = _orderPackageService.GetOrderPackageListIQueryable(searchViewModel)
                .OrderBy(x => x.Id);
            var model = _orderPackageService.GetPackageList();



            ModelState.Clear();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/OrderPackage/PackageList.cshtml", result)
                })
            };
        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int packageId)
        {
            var callResult = await _orderPackageService.DeleteOrderPackageAsync(packageId);
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
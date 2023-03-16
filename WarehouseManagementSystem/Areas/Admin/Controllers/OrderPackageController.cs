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


            var currentPageIndex = page - 1 ?? 0;

            var result = _orderPackageService.GetOrderPackageListIQueryable(searchViewModel)
                .OrderBy(x => x.Id)
                .ToPagedList(currentPageIndex,SystemConstants.DefaultOrderPageSize);


           

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
        public async Task<ActionResult> Edit(int packageId)
        {

            var model = await _orderPackageService.GetPackageEditViewModelAsync(packageId);
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/OrderPackage/_PackageEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Paket sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderPackageProductEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _orderPackageService.EditPackageAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (OrderPackageListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/OrderPackage/DisplayTemplates/OrderPackageListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/OrderPackage/_PackageEdit.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> PackageProductDelete(int packageProductId)
        {
            var callResult = await _orderPackageService.DeleteOrderPackageProductAsync(packageProductId);
            if (callResult.Success)
            {

                ModelState.Clear();

                return Json(
                    new
                    {
                        success = true,
                        warningMessages = callResult.WarningMessages,
                        successMessages = callResult.SuccessMessages,
                        item = callResult.Item,
                    });
            }

            return Json(
                new
                {
                    success = false,
                    errorMessages = callResult.ErrorMessages
                });

        }
        [AjaxOnly,HttpGet]
        public async Task<ActionResult> Add(long orderId , long orderPackageId)
        {



            var model = await _orderPackageService.GetPackageProductAddViewModelAsync(orderId, orderPackageId);
            if (model.ProductGroupsEditViewModels.Count() != 0)
            {

                return PartialView("~/Areas/Admin/Views/OrderPackage/_PackageProductAdd.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Eklenecek ürün bulunamadı! Lütfen paket içeriğini kontrol edin.");
           

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(OrderPackageIntoAddProductViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _orderPackageService.PackageProductAddAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (OrderPackageListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/OrderPackage/DisplayTemplates/OrderPackageListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/OrderPackage/_PackageProductAdd.cshtml", model),
                  
                });

        }
    }
}
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
    public class CustomerOrderSettingController : AdminBaseController
    {
        private readonly CustomerOrderSettingService _customerOrderSettingService;
        private readonly OrderService _orderService;
        private readonly WarehouseManagementSystemEntities1 _context;

        public CustomerOrderSettingController(CustomerOrderSettingService customerOrderSettingService, OrderService orderService, WarehouseManagementSystemEntities1 context)
        {
            _customerOrderSettingService = customerOrderSettingService;
            _orderService = orderService;
            _context = context;
        }
        // GET: Admin/CustomerOrderSetting
        public async Task<ActionResult> Index(long customerId)
        {
           var model =  _customerOrderSettingService.Detail(customerId);
            ViewBag.Languages = await _customerOrderSettingService.GetLanguageListViewAsync();
            ViewBag.CustomerId = await _customerOrderSettingService.GetCustomerListViewAsync(customerId);
            ViewBag.CustomerUserName = _context.Users.FirstOrDefault(x=>x.Id == customerId).UserName;
            ViewBag.CustomerName = _context.Users.FirstOrDefault(a => a.Id == customerId).Name+ " " + _context.Users.FirstOrDefault(a => a.Id == customerId).Surname;


            return View("~/Areas/Admin/Views/CustomerOrderSetting/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CustomerOrderList(CustomerOrderSearchViewModel model, int? page)
        {




            var currentPageIndex = page - 1 ?? 0;

            var result = _customerOrderSettingService.GetOrderListIQueryable(model)
                .OrderBy(p => p.DateTime).OrderBy(x => x.isPackage)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultOrderPageSize);
            ViewBag.Languages = await _customerOrderSettingService.GetLanguageListViewAsync();
            ViewBag.CustomerId = model.CustomerId;

            ModelState.Clear();
            ViewBag.LanguageId = model.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/CustomerOrderList.cshtml", result)
                })
            };
        }
        [AjaxOnly]
        [HttpGet]
        //Sipariş ekleme sayfası
        public ActionResult CustomerOrderAdd(long customerId)
        {

            ViewData["Cities"] = new List<CityListViewModel>();
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();

            var model = new CustomerOrderAddViewModel
            {
                ProductTransactionGroup = new List<ProductTransactionGroupViewModel>() {new ProductTransactionGroupViewModel {
                   Content = null,
                   GtipCode = null,
                   SKU = null,
                   
               } },
                CustomerId = customerId,



            };
            return PartialView("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerOrderAdd(CustomerOrderAddViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _customerOrderSettingService.AddOrderAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CustomerOrderListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/DisplayTemplates/CustomerOrderListViewModel.cshtml", viewModel),
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


            ViewData["Cities"] = _orderService.GetOrderCityList(model.Country.CountryId).ToList();
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderAdd.cshtml", model)
                });

        }
        [HttpGet]
        public async Task<ActionResult> CustomerOrderEdit(int orderId)
        {

            var model = await _customerOrderSettingService.GetOrderEditViewModelAsync(orderId);
            if (model != null)
            {
                ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
                ViewData["Cities"] = _orderService.GetOrderCityList(model.Country.CountryId).ToList();
                ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
                ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
                return PartialView("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerOrderEdit(CustomerOrderEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var callResult = await _customerOrderSettingService.EditOrderAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CustomerOrderListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/DisplayTemplates/CustomerOrderListViewModel.cshtml", viewModel),
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
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["Cities"] = _orderService.GetOrderCityList(model.Country.CountryId).ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();

            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderEdit.cshtml", model)
                });

        }
        [HttpGet]
        //Sipariş paketle modal'i 
        public async Task<ActionResult> OrderPackageAdd(int orderId)
        {
            var readOnlyProduct = _context.ProductTransactionGroup.Where(x => x.OrderId == orderId).ToList();
            //foreach (var item in readOnlyProduct)
            //{
            //    if (item.isPackage == null || item.isPackage == false)
            //    {
            //        item.isPackagedCount = item.Count;
            //    }
            //}
            foreach (var item in readOnlyProduct)
            {
                item.isPackagedCount = item.isPackagedCount2;
                if (item.isReadOnly == false || item.isReadOnly == null)
                {
                    item.isPackagedCount = item.Count;
                }
            }
            _context.SaveChanges();
            var model = new OrderPackageAddViewModel
            {
                OrderId = orderId,
                OrderProductGroups = await _orderService.GetOrderProductGroup(orderId),

            };
            return PartialView("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderPackageAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> OrderPackageAdd(OrderPackageAddViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _customerOrderSettingService.AddOrderPackageAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CustomerOrderListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/DisplayTemplates/CustomerOrderListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderPackageAdd.cshtml", model)
                });
        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(long customerOrderId)
        {
            var callResult = await _customerOrderSettingService.DeleteOrderAsync(customerOrderId);
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
using Castle.Core.Resource;
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
        private readonly OrderPackageService _orderPackageService;

        public CustomerOrderSettingController(CustomerOrderSettingService customerOrderSettingService, OrderService orderService, WarehouseManagementSystemEntities1 context, OrderPackageService orderPackageService)
        {
            _customerOrderSettingService = customerOrderSettingService;
            _orderService = orderService;
            _context = context;
            _orderPackageService = orderPackageService; 
        }
        // GET: Admin/CustomerOrderSetting
        public async Task<ActionResult> Index(long customerId)
        {
           
            ViewBag.Languages = await _customerOrderSettingService.GetLanguageListViewAsync();
            ViewBag.CustomerId = await _customerOrderSettingService.GetCustomerListViewAsync(customerId);
            ViewBag.CustomerListId = await _customerOrderSettingService.GetCustomerIdListViewAsync();
            
 
            return View("~/Areas/Admin/Views/CustomerOrderSetting/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CustomerOrderList(CustomerOrderSearchViewModel model, int? page)
        {
            ViewData["CustomerUserName"] = _context.Users.FirstOrDefault(x => x.Id == model.CustomerId).UserName;
            ViewData["CustomerName"] = _context.Users.FirstOrDefault(a => a.Id == model.CustomerId).Name + " " + _context.Users.FirstOrDefault(a => a.Id == model.CustomerId).Surname;
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
        [HttpGet]
        //koli adetine basınca kolilenmiş ürünleri gösterme
        public async Task<ActionResult> OrderPackageGroupShow(int orderId)
        {

            ViewData["ProductList"] = _context.ProductTransactionGroup.Where(x => x.OrderId == orderId).Select(a => new ProductGroupShowViewModel
            {
                Content = a.Content,
                Count = a.Count,
                GtipCode = a.GtipCode,
                Id = a.Id,
                isPackagedCount = a.isPackagedCount,
                QuantityPerUnit = a.QuantityPerUnit,
                SKU = a.SKU,
            }).ToList();
            ViewData["OrderId"] = orderId;
            
            
            var model = await _customerOrderSettingService.GetOrderPackageGroup(orderId);
            int? count = 0;
            foreach (var item in model)
            {

                foreach (var prd in item.OrderPackageProductGroups)
                {
                    count += prd.Count;
                }

                item.CountProductsInThePackage = count;
                count = 0;
            }



            if (model != null && model.Count() > 0)
            {

                return PartialView("~/Areas/Admin/Views/CustomerOrderSetting/_CustomerOrderPackageGroupShow.cshtml", model);
            }
            else if (model != null && model.Count() <= 0)
            {
                return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş paketlenmedi!");
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }
        public async Task<ActionResult> CustomerOrderPackages(long? packageId, long? searchId)
        {

            ViewBag.Title = "Sipariş Paketleri";

            var model = new OrderPackageSearchViewModel
            {
                SearchId = searchId,
                PackageId = packageId

            };
            return View("~/Areas/Admin/Views/CustomerOrderSetting/CustomerOrderPackages.cshtml", model);

        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> CustomerPackageList(OrderPackageSearchViewModel searchViewModel, int? page)
        {

            var currentPageIndex = page - 1 ?? 0;


            var result = _customerOrderSettingService.GetOrderPackageListIQueryable(searchViewModel)
                .OrderBy(x => x.Id)
                .ToPagedList(currentPageIndex,SystemConstants.DefaultOrderPageSize);

            


            ModelState.Clear();

            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/CustomerPackageList.cshtml", result)
                })
            };
        }
        public async Task<ActionResult> CustomerPackageEdit(int packageId)
        {

            var model = await _orderPackageService.GetPackageEditViewModelAsync(packageId);
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/CustomerOrderSetting/_PackageEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Paket sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerPackageEdit(OrderPackageProductEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _customerOrderSettingService.EditPackageAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CustomerOrderPackageListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/DisplayTemplates/CustomerOrderPackageListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/_PackageEdit.cshtml", model)
                });

        }
        [AjaxOnly, HttpGet]
        public async Task<ActionResult> CustomerPackageProductAdd(long orderId, long orderPackageId)
        {



            var model = await _orderPackageService.GetPackageProductAddViewModelAsync(orderId, orderPackageId);
            if (model.ProductGroupsEditViewModels.Count() != 0)
            {

                return PartialView("~/Areas/Admin/Views/CustomerOrderSetting/_PackageProductAdd.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Eklenecek ürün bulunamadı! Lütfen paket içeriğini kontrol edin.");


        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerPackageProductAdd(OrderPackageIntoAddProductViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _customerOrderSettingService.PackageProductAddAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CustomerOrderPackageListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/DisplayTemplates/CustomerOrderPackageListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/_PackageProductAdd.cshtml", model),

                });

        }
        public async Task<ActionResult> AllCustomerOrder()
        {

            ViewBag.Languages = await _customerOrderSettingService.GetLanguageListViewAsync();        
            return View("~/Areas/Admin/Views/CustomerOrderSetting/AllCustomerOrder.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AllCustomerOrderList(CustomerOrderSearchViewModel model, int? page)
        {
             
            var currentPageIndex = page - 1 ?? 0;

            var result = _customerOrderSettingService.GetAllOrderListIQueryable(model)
                .OrderBy(p => p.DateTime).OrderBy(x => x.isPackage)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultServicePageSize);
            ViewBag.Languages = await _customerOrderSettingService.GetLanguageListViewAsync();
           
            ModelState.Clear();
            ViewBag.LanguageId = model.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CustomerOrderSetting/AllCustomerOrderList.cshtml", result)
                })
            };
        }



    }
}
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Warehouse.Data;
using Warehouse.Service;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.Controllers.Abstract;

namespace WarehouseManagementSystem.Controllers
{
    public class OrderController : AdminBaseController
    {
        private readonly OrderService _orderService;
        WarehouseManagementSystemEntities1 _context = new WarehouseManagementSystemEntities1();

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;

        }
        // GET: Order

        public async Task<ActionResult> Index()
        {

            ViewBag.Languages = await _orderService.GetLanguageListViewAsync();
            return View("~/Views/Order/Index.cshtml");
        }


        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> OrderList(OrderSearchViewModel model)
        {

            var result = _orderService.GetOrderListIQueryable(model)
                .OrderBy(p => p.Id);
            ViewBag.Languages = await _orderService.GetLanguageListViewAsync();

            ModelState.Clear();
            ViewBag.LanguageId = model.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Views/Order/OrderList.cshtml", result)
                })
            };
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {


            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();

            var model = new OrderAddViewModel
            {
                ProductTransactionGroup = new List<ProductTransactionGroupViewModel>() {new ProductTransactionGroupViewModel {
                   Content = null,
                   GtipCode = null,
                   SKU = null,
               } },



            };
            return PartialView("~/Views/Order/_OrderAdd.cshtml", model);
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(OrderAddViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _orderService.AddOrderAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (OrderListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/Order/DisplayTemplates/OrderListViewModel.cshtml", viewModel),
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
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Views/Order/_OrderAdd.cshtml", model)
                });

        }
        public PartialViewResult ProductTransactionGroupRow()
        {
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            return PartialView("~/Views/Order/_OrderProductTransactionGroupAdd.cshtml", new ProductTransactionGroupViewModel());
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int orderId)
        {

            var model = await _orderService.GetOrderEditViewModelAsync(orderId);
            if (model != null)
            {
                ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
                ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
                ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
                return PartialView("~/Views/Order/_OrderEdit.cshtml", model);
            }
            return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }
        [HttpGet]
        public async Task<ActionResult> ProductGroupShow(int orderId)
        {

            var model = await _orderService.GetOrderProductGroup(orderId);
            int? counter = 0;
            foreach (var item in model)
            {
                counter += item.Count;
            }
            if (model != null)
            {
                ViewData["TotalCount"] = counter;
                return PartialView("~/Views/Order/_OrderProductGroupShow.cshtml", model);
            }
            return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }
        [HttpGet]
        public async Task<ActionResult> OrderPackageGroupShow(int orderId)
        {

            //var result = _orderService.GetOrderListIQueryable(model)
            //   .OrderBy(p => p.SenderName);

            var model = await _orderService.GetOrderPackageGroup(orderId);

            if (model != null && model.Count() > 0)
            {

                return PartialView("~/Views/Order/_OrderPackageGroupShow.cshtml", model);
            }
            else if (model != null && model.Count() <= 0)
            {
                return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş paketlenmedi!");
            }
            return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }

        public PartialViewResult PackageTransactionGroupRow()
        {

            return PartialView("~/Views/Order/_OrderPackageGroupAdd.cshtml", new OrderPackageGroupViewModel());
        }
        [HttpGet]
        public async Task<ActionResult> OrderPackageAdd(int orderId)
        {
            var readOnlyProduct = _context.ProductTransactionGroup.Where(x=>x.OrderId==orderId).ToList();
            foreach (var item in readOnlyProduct)
            {
                item.isReadOnly = false;
            }
            _context.SaveChanges();
            var model = new OrderPackageAddViewModel
            {
                OrderId = orderId,
                OrderProductGroups = await _orderService.GetOrderProductGroup(orderId),

            };
            return PartialView("~/Views/Order/_OrderPackageAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public ActionResult OrderPackageAdd(OrderPackageAddViewModel model)
        {

            //var resultModel = new PackageListViewModel()
            //{
            //    Count = model.Count,
            //    Height = model.Height,
            //    Length = model.Length,
            //    Weight = model.Weight,
            //    Width = model.Width,
            //    Desi = ((decimal)((model.Height * model.Length * model.Width) / 3000.00)),
            //};



            var jsonResult = Json(
                new
                {
                    success = true,


                });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public async Task<ActionResult> OrderPackageProductAdd(int orderId)
        {
            var resultModel = new OrderPackageProductAddViewModel()
            {
                OrderId = orderId,
                OrderProductGroups = await _orderService.GetOrderProductIsPackageGroup(orderId),
            };
            bool? isReadOnly = false;
            foreach (var item in resultModel.OrderProductGroups)
            {
                if (item.isReadOnly==true)
                {
                    isReadOnly = true;
                }
                else if (item.isReadOnly==false)
                {
                    isReadOnly = false;
                }
            }
            if (isReadOnly==true)
            {
                return PartialView("~/Views/Shared/_ItemNotFoundPartial.cshtml", "Siparişte paketlenecek ürün kalmadı!");
            }
            

            return PartialView("~/Views/Order/_OrderPackageProductAdd.cshtml", resultModel);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public ActionResult OrderPackageProductAdd(OrderPackageProductAddViewModel model)
        {
             
            var resultModel = new OrderPackageProductListViewModel()
            {
                Count = model.Count,
                Desi = model.Desi,
                Height = model.Height,
                Length = model.Height,
                OrderPackagedProductGroups = model.OrderProductGroups.Where(x=>x.isChecked==true),
                Weight = model.Weight,
                Width = model.Width,
                
            };
            foreach (var item in model.OrderProductGroups)
            {
                var readOnlyProduct = _context.ProductTransactionGroup.Find(item.Id);
                if (item.isChecked==true)
                {
                    readOnlyProduct.isReadOnly = true;
                }
            }
            _context.SaveChanges();

            var jsonResult = Json(
              new
              {
                  success = true,
                  responseText = RenderPartialViewToString("~/Views/Order/DisplayTemplates/OrderPackageProductListViewModel.cshtml", resultModel),
                  item = resultModel
              });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var callResult = await _orderService.EditOrderAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (OrderListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Views/Order/DisplayTemplates/OrderListViewModel.cshtml", viewModel),
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
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();

            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Views/Order/_OrderEdit.cshtml", model)
                });

        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var callResult = await _orderService.DeleteOrderAsync(id);
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
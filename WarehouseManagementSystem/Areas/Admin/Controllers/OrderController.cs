using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Warehouse.Data;
using Warehouse.Service.Admin;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin; 

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class OrderController : AdminBaseController
    {
        private readonly OrderService _orderService;
        WarehouseManagementSystemEntities1 _context = new WarehouseManagementSystemEntities1();


        public OrderController(OrderService orderService)
        {
            _orderService = orderService;

        }
        // GET: Order
        //Anasayfa
        public async Task<ActionResult> Index()
        { 

            string userName = User.Identity.Name; 
            var user = _context.Users.FirstOrDefault(x => x.UserName == userName);
            ViewData["Ad"] = user.Name;
            ViewData["Soyad"] = user.Surname;
            ViewData["UserId"] = user.Id;

            ViewBag.Languages = await _orderService.GetLanguageListViewAsync();
            return View("~/Areas/Admin/Views/Order/Index.cshtml");
        }

        //Anasayfa'da gözüken sipariş listesi
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> OrderList(OrderSearchViewModel model, int? page)
        {

            


            var currentPageIndex = page - 1 ?? 0;

            var result = _orderService.GetOrderListIQueryable(model)
                .OrderBy(p => p.DateTime).OrderBy(x => x.isPackage)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultOrderPageSize);
            ViewBag.Languages = await _orderService.GetLanguageListViewAsync();

            ModelState.Clear();
            ViewBag.LanguageId = model.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/OrderList.cshtml", result)
                })
            };
        }

        [AjaxOnly]
        [HttpGet]
        //Sipariş ekleme sayfası
        public ActionResult Add()
        {

            ViewData["Cities"] = new List<CityListViewModel>();
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
           

            var model = new OrderAddViewModel
            {
                ProductTransactionGroup = new List<ProductTransactionGroupViewModel>() {new ProductTransactionGroupViewModel {
                   Content = null,
                   GtipCode = null,
                   SKU = null,
               } },



            };
            return PartialView("~/Areas/Admin/Views/Order/_OrderAdd.cshtml", model);
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
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/DisplayTemplates/OrderListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/_OrderAdd.cshtml", model)
                });

        }
        //Sipariş ekleme sayfasında yeni ürün ekle partial'i
        public PartialViewResult ProductTransactionGroupRow()
        {
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            return PartialView("~/Areas/Admin/Views/Order/_OrderProductTransactionGroupAdd.cshtml", new ProductTransactionGroupViewModel());
        }
      
        [HttpGet]
        //Siparişteki ürünleri görüntüle modal'i
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
                return PartialView("~/Areas/Admin/Views/Order/_OrderProductGroupShow.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }

        [HttpGet]
        //Fiyat hesaplama modal'i
        public async Task<ActionResult> OrderPriceCalculator()
        {
            var model = new OrderPriceCalculateViewModel
            {
                Country = new OrderCountryIdSelectViewModel
                {
                    
                }
                
            };
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = new List<CargoServiceTypeListViewModel>();
            return PartialView("~/Areas/Admin/Views/Order/_OrderPriceCalculator.cshtml", model);



        }
        [HttpPost]
        //Fiyat hesaplama modal'i
        public async Task<ActionResult> OrderPriceCalculator(OrderPriceCalculateViewModel model)
        {
            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList(model.Country.CountryId).ToList();

            if (ModelState.IsValid)
            {
                var callResult = await _orderService.OrderPriceCalculate(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = model;
                    var jsonResult = Json(
                        new
                        {

                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/_OrderPriceCalculator.cshtml", viewModel),
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
                   responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/_OrderPriceCalculator.cshtml", model)
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
            var model = await _orderService.GetOrderPackageGroup(orderId);
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

                return PartialView("~/Areas/Admin/Views/Order/_OrderPackageGroupShow.cshtml", model);
            }
            else if (model != null && model.Count() <= 0)
            {
                return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş paketlenmedi!");
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
        }


        [HttpGet]
        //Sipariş paketle modal'i 
        public async Task<ActionResult> OrderPackageAdd(int orderId)
        {
            var readOnlyProduct = _context.ProductTransactionGroup.Where(x => x.OrderId == orderId).ToList();
            
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
            return PartialView("~/Areas/Admin/Views/Order/_OrderPackageAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> OrderPackageAdd(OrderPackageAddViewModel model)
        {

            if (ModelState.IsValid)
            {
                var callResult = await _orderService.AddOrderPackageAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (OrderListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/DisplayTemplates/OrderListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/_OrderPackageAdd.cshtml", model)
                });
        }
        [HttpGet]
        //Sipariş paketle modal'inde ürünleri paketle modal'i
        public async Task<ActionResult> OrderPackageProductAdd(int orderId)
        {
            var resultModel = new OrderPackageProductAddViewModel()
            {
                OrderId = orderId,
                OrderProductGroups = await _orderService.GetOrderProductIsPackageGroup(orderId),
            };

            if (resultModel.OrderProductGroups.Count() == 0)
            {
                return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Siparişte paketlenecek ürün kalmadı!");
            }


            return PartialView("~/Areas/Admin/Views/Order/_OrderPackageProductAdd.cshtml", resultModel);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]

        public async Task<ActionResult> OrderPackageProductAdd(OrderPackageProductAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var callResult = await _orderService.AddOrderPackageProductAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (OrderPackageProductListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/DisplayTemplates/OrderPackageProductListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/_OrderPackageProductAdd.cshtml", model)
                });
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int orderId)
        {

            var model = await _orderService.GetOrderEditViewModelAsync(orderId);
            if (model != null)
            {
                ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
                ViewData["Cities"] = _orderService.GetOrderCityList(model.Country.CountryId).ToList();
                ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();

                return PartialView("~/Areas/Admin/Views/Order/_OrderEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Sipariş sistemde bulunamadı!");
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
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/DisplayTemplates/OrderListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/Order/_OrderEdit.cshtml", model)
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

        public JsonResult NestedCity(int id)
        {
            var cities = (from x in _context.Cities
                          join y in _context.Countries on x.Countries.Id equals y.Id
                          where x.Countries.Id == id
                          select new
                          {
                              Id = x.Id,
                              Name = x.Name,
                          }).ToList();

            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NestedCargo(int id)
        {
            var cargoService = (from x in _context.ShippingPrices
                               where x.CountryId == id && x.Active == true
                          select new
                          {
                              Id = x.Id,
                              Name = x.CargoServiceTypes.Name,
                          }).ToList();

            return Json(cargoService, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly,HttpPost]
        public ActionResult SelectAllPackageProduct(int selectId)
        {
            var result = (from x in _context.ProductTransactionGroup
                                where x.Id == selectId
                                select new
                                {
                                    selectCount = x.isPackagedCount,
                                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SelectPackageCount(int selectId)
        {
            var model = _orderService.SelectPackageCount(selectId);
           
            return PartialView("~/Areas/Admin/Views/Order/_OrderSelectedPackageProduct.cshtml", model);


        }

    }
}
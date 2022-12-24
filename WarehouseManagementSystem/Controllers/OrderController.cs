using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Warehouse.Data;
using Warehouse.Service;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.Controllers.Abstract;

namespace WarehouseManagementSystem.Controllers
{
    public class OrderController : AdminBaseController
    {
        private readonly OrderService _orderService;
        WarehouseManagementSystemEntities db = new WarehouseManagementSystemEntities();

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
           
        }
        // GET: Order
        public async Task<ActionResult> Index()
        {
            var result = _orderService.GetOrderList();

            return View("~/Views/Order/Index.cshtml",result);
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
                   //Count = 0,
                   GtipCode = null,
                   //QuantityPerUnit = 0,
                   SKU = null,
                   CurrenyUnit = new OrderCurrencyUnitIdSelectViewModel(),


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
                    var countryName = db.Countries.Find(model.Country.CountryId);
                    var currencyUnitName = db.CurrencyUnits.Find(model.CurrenyUnit.CurrencyUnitId);
                    var cargoName = db.CargoServiceTypes.Find(model.CargoService.CargoServiceId);
                    var viewModel = new OrderListViewModel
                    {
                        SenderName = model.SenderName,
                        RecipientAddress = model.RecipientAddress,
                        RecipientCity = model.RecipientCity,
                        RecipientName = model.RecipientName,
                        RecipientCountry = countryName.Name,
                        CurrencyUnit = currencyUnitName.Name,
                        RecipientZipCode = model.RecipientZipCode,
                        CargoService = cargoName.Name,
                        PackageCount = model.PackageCount
                    };
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = View("Index",viewModel),

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


    }
}
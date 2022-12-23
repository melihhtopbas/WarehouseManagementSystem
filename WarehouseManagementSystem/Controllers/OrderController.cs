using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Warehouse.Service;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.Controllers.Abstract;

namespace WarehouseManagementSystem.Controllers
{
    public class OrderController : AdminBaseController
    {
        private readonly OrderService _orderService;
         
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
        public ActionResult Add()
        {


            ViewData["Countries"] = _orderService.GetOrderCountryList().ToList();
            ViewData["CargoServiceTypes"] = _orderService.GetOrderCargoServiceTypeList().ToList();
            ViewData["CurrencyUnits"] = _orderService.GetOrderCurrencyUnitList().ToList();
            //Personels = new List<NewPersonelVM>() { new NewPersonelVM { Ad = null,Cinsiyet=false,DepartmanId=null,EvliMi=false,DogumTarihi=null,
            //   Maas=null,Id=0,PersonelGorsel=null,Soyad=null,Resim=null,DepartmanlarListesi=db.Departman.ToList()} },
            var model = new OrderAddViewModel
            {
               ProductTransactionGroup = new List<ProductTransactionGroupViewModel>() {new ProductTransactionGroupViewModel {
                   Content = null,
                   Count = 0,
                   GtipCode = null,
                   QuantityPerUnit = 0,
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
                    var viewModel = (OrderListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = View("Index", viewModel),

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


    }
}
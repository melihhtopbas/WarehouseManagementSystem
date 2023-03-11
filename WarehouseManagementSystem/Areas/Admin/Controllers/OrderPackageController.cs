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

        public async Task<ActionResult> Index()
        {

            ViewBag.Title = "Sipariş Paketleri";
       

            return View("~/Areas/Admin/Views/OrderPackage/Index.cshtml");

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
    }
}
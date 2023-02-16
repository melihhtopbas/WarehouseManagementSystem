using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.Areas.Admin.Controllers;

namespace WarehouseManagementSystem.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly SettingService _settingService;
        public UserController(SettingService settingService)
        {
            _settingService = settingService;
        }
    
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            return PartialView("~/Views/User/Register.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var callResult = await _settingService.RegisterAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();

                    var jsonResult = Json(
                        new
                        {
                            success = true,

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
                    responseText = RenderPartialViewToString("~/Views/User/Register.cshtml", model)
                });

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Warehouse.ViewModels.Admin;
using Warehouse.Service.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class UserRegisterController : AdminBaseController
    {
        private readonly SettingService _settingService;
        public UserRegisterController(SettingService settingService)
        {
            _settingService = settingService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Register()
        {

            var model = new RegisterViewModel();
            return View("~/Areas/Admin/Views/UserRegister/Register.cshtml", model);
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                var callResult = await _settingService.RegisterAsync(model);
                if (callResult.Success)
                {

                    ViewBag.Title = "Hakkımızda";
                    TempData["registeredMsg"] = "İşlem başarılı! Giriş sayfasına yönlendiriliyorsunuz";
                    return View("~/Areas/Admin/Views/UserRegister/Register.cshtml", model);



                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }



            return View("Register");
        }
    }
}
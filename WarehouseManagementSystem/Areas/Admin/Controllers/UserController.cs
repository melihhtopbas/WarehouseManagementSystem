using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks; 
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Warehouse.Service.Admin;
using Warehouse.ViewModels.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
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
            return View("~/Areas/Admin/Views/User/Register.cshtml", model);
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
             
            if (ModelState.IsValid)
            {
                 
                var callResult = await _settingService.RegisterAsync(model);
                if (callResult.Success)
                {
                    ViewData["Message"] = "Kayıt başarılı!";
                    ViewData["RedirectLogin"] = true;

                    ModelState.Clear(); 
                    Thread.Sleep(1000);
                    return View();


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
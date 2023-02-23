using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Warehouse.Data;
using Warehouse.Service;
using Warehouse.Service.Admin;
using Warehouse.ViewModels.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class LoginController : AdminBaseController
    {
        // GET: Admin/Login
        
        private readonly SettingService _settingService;

        WarehouseManagementSystemEntities1 _context = new WarehouseManagementSystemEntities1();

        public LoginController(SettingService settingService)
        {
            _settingService = settingService;
        }
        
        // GET: Admin/Login

        public ActionResult Index(string returnUrl)
        {
            
            ViewBag.Title = "Giriş";
            return View();
        }

        //
        // POST: /User/Login
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName); 
            if (ModelState.IsValid)
            {
                var loginResult = _settingService.Login(model);
                var kullaniciInDb = _context.Users.FirstOrDefault(x => x.UserName ==model.UserName&& x.Password == model.Password);
                if (loginResult)
                {
                   
                    
                    FormsAuthentication.SetAuthCookie(kullaniciInDb.UserName, false);

                    return RedirectToLocalOr(returnUrl, () => RedirectToAction("Index", "Order", new { Area = "Admin" }));

                }
                



            }
            else
            {
                Response.Write("<script language='javascript'>alert(\"Geçersiz kullanıcı adı veya şifre!\")</script>");


                
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new {Area = String.Empty});
        }
         
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            return PartialView("~/Areas/Admin/Views/Login/Register.cshtml", model);
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/Login/Register.cshtml", model)
                });

        }
    }
}
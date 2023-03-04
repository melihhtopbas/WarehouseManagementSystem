using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Warehouse.ViewModels.Admin;
using Warehouse.Service.Admin;
using Microsoft.Web.Mvc;
using System.Net;
using System.Net.Mail;
using Warehouse.Data;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class UserRegisterController : AdminBaseController
    {
        private readonly SettingService _settingService;
        private readonly UserService _userService;
        private readonly WarehouseManagementSystemEntities1 _context;
        public UserRegisterController(SettingService settingService, UserService userService, WarehouseManagementSystemEntities1 context)
        {
            _settingService = settingService;
            _userService = userService;
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Register()
        {
            ViewData["Cities"] = _userService.GetUserCityList(20002);
            ViewData["Countries"] = _userService.GetUserCountryList().ToList();

            var model = new RegisterViewModel
            {
                City = new OrderCityIdSelectViewModel
                {

                },
                Country = new OrderCountryIdSelectViewModel
                {
                    CountryId = 20002
                },

            };
            
            return View("~/Areas/Admin/Views/UserRegister/Register.cshtml", model);
        }

        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewData["Cities"] = _userService.GetUserCityList(model.Country.CountryId).ToList();
            ViewData["Countries"] = _userService.GetUserCountryList().ToList();
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

        [HttpGet, AjaxOnly]
        public ActionResult ForgotPassword()
        {
            var model = new UserForgotPasswordViewModel();
            return PartialView("~/Areas/Admin/Views/UserRegister/ForgotPassword.cshtml", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult ForgotPassword(UserForgotPasswordViewModel model)
        {

            // var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            var user =  _userService.GetUserForgottenPassword(model.Mail);

            if (user.Message!=null)
            {
                ViewData["ErrorMessage"] = "Sistemimizde kayıtlı böyle bir mail adresi bulunmamaktadır.";
                model.Message = user.Message;
                return Json(
             new
             {
                 success = false,
                 responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserRegister/ForgotPassword.cshtml", model)
             });

            }


            string senderEmail = "topbas_melih_70_70@hotmail.com";
            string senderPassword = "Topbas1907";
            string receiverEmail = user.Mail;
            string subject = "Şifre Değişikliği";
            string body = "Şifreniz aşağıda verilmiştir.\n" + user.Password + "\n" + "Kullanıcı Adınız: "+ user.UserName;

            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage mailMessage = new MailMessage(senderEmail, receiverEmail, subject, body);
             
            mailMessage.IsBodyHtml = true;

            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                 
            }

            return Json(
              new
              {
                  success = true,
                  responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserRegister/ForgotPassword.cshtml", model)
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
    }
}
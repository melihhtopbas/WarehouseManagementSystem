using DocumentFormat.OpenXml.EMMA;
using ImageResizer;
using Microsoft.Ajax.Utilities;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.Service.Admin;
using Warehouse.ViewModels.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : AdminBaseController
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly UserService _userService;
        public UserController(WarehouseManagementSystemEntities1 context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

         // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
       
        public async Task<ActionResult> UserProfile()
        {
            ViewBag.Title = "Profil";
            ViewData["Cities"] = _userService.GetUserCityList(20002);
            ViewData["Countries"] = _userService.GetUserCountryList().ToList();

            var model = await _userService.GetUserProfileViewModel().ConfigureAwait(false);
            model.Country.CountryId = 20002;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> UserProfile(UserViewModel model)
       {
            ViewData["Cities"] = _userService.GetUserCityList(model.Country.CountryId).ToList();
            ViewData["Countries"] = _userService.GetUserCountryList().ToList();
            if (ModelState.IsValid)
            {

                var callResult = await _userService.AddorEditUser(model);
                if (callResult.Success)
                {
                    ViewBag.Title = "Profil";
                    TempData["Msg"] = "İşlem başarılı!";
                    return View("~/Areas/Admin/Views/User/UserProfile.cshtml", model);

                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }

       

            ViewBag.Title = "Profil";
            return View("~/Areas/Admin/Views/User/UserProfile.cshtml", model);
        }
        [HttpGet,AjaxOnly]
        public ActionResult ChangePassword()
        {
            return PartialView("~/Areas/Admin/Views/User/ChangePassword.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> ChangePassword(UserChangePasswordViewModel model)
        {

            
            if (ModelState.IsValid)
            {
                var callResult = await _userService.ChangePasswordAsync(model);
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/User/ChangePassword.cshtml", model)
                });
        }
        [HttpGet, AjaxOnly]
        public ActionResult ForgotPassword()
        {
            var model = new UserForgotPasswordViewModel();
            return PartialView("~/Areas/Admin/Views/User/ForgotPassword.cshtml",model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult ForgotPassword(UserForgotPasswordViewModel model)
        {
             
            var user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            model.Subject = "Şifre Değişikliği";
            model.Message = "Şifreniz aşağıda verilmiştir.\n" + user.Password;
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("topbas_melih_70_70@hotmail.com", "PraSoft");
                    var receiverEmail = new MailAddress(model.Mail, "Şifre");
                    var password = "Topbas1907";
                    var sub = model.Subject;
                    var body = model.Message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = model.Subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return Json(
                new
                {
                    success = true,
                    
                });
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/User/ForgotPassword.cshtml", model)
                });
        }


    }
}
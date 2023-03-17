using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.WebSite;

namespace WarehouseManagementSystem.Controllers
{
    public class ContactController : BaseController
    {

        private readonly ContactService _contactService;
        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }
    
        // GET: Contact
        public ActionResult Index(string lang)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ContactViewModel model, string lang)
        {
            var result = _contactService.AddContactForm(model);
            if (result.Success)
            {
                //ViewData[StringConstants.SuccessMessage] = "Formunuz Başarıyla Kaydedilmiştir.";
                //return View();
                TempData["message"] = "İletişim formunuz başarıyla alınmıştır.";
                return View();
                //return Json(new { success = result.Success, item = result.ErrorMessages.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(new { success = result.Success, item = result.ErrorMessages.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}
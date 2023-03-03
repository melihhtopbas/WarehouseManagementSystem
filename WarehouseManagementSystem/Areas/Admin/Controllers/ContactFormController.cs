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
    [Authorize]
    public class ContactFormController : AdminBaseController
    {
        private readonly ContactService _contactService;


        public ContactFormController(ContactService contactService)
        {
            _contactService = contactService;

        }

        public ActionResult Index()
        {
            ViewBag.Title = "İletişim Formu";

            return View("~/Areas/Admin/Views/ContactForm/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public ActionResult ContactList(ContactViewModel contactViewModel, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _contactService.GetContactListIQueryable(contactViewModel).OrderByDescending(a => a.Id)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultServicePageSize);


            ModelState.Clear();
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/ContactForm/ContactList.cshtml", result)
                })
            };
        }

        [AjaxOnly]
        public async Task<ActionResult> ContactDetailAsync(int contactId)
        {
            var model = await _contactService.GetContactDetailModelAsync(contactId).ConfigureAwait(false);
            return PartialView("~/Areas/Admin/Views/ContactForm/_MessageView.cshtml", model);
        }
    }
}
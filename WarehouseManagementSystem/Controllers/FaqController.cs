using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;

namespace WarehouseManagementSystem.Controllers
{
    public class FaqController : Controller
    {
        // GET: Faq
        private readonly FaqService _faqService;

        public FaqController(FaqService faqService)
        {
            _faqService = faqService;
        }

        public ActionResult Index(string lang)
        {
            var model = _faqService.GetFaqListIQueryable(lang).OrderBy(a => a.Id).ToList();
            return View(model);
        }
    }
}
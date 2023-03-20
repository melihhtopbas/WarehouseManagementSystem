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
using Warehouse.Service.WebSite;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class TicketBoxController : AdminBaseController
    {
        private readonly TicketBoxService _ticketBoxService;
        public TicketBoxController(TicketBoxService ticketBoxService)
        {
            _ticketBoxService = ticketBoxService;
        }
    
        public ActionResult Index()
        {
            ViewBag.Title = "Oluşturduğum Ticketlar";
            return View("~/Areas/Admin/Views/TicketBox/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public ActionResult TicketList(TicketBoxViewModel contactViewModel, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _ticketBoxService.GetTicketListIQueryable(contactViewModel).OrderBy(a => a.isAnswer).OrderByDescending(a=>a.Date)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultServicePageSize);


            ModelState.Clear();
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/TicketList.cshtml", result)
                })
            };
        }

        [AjaxOnly]
        public async Task<ActionResult> TicketBoxDetailAsync(int ticketId)
        {
            var model = await _ticketBoxService.GetTicketBoxDetailModelAsync(ticketId).ConfigureAwait(false);
            return PartialView("~/Areas/Admin/Views/TicketBox/_TicketView.cshtml", model);
        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(long ticketId)
        {
            var callResult = await _ticketBoxService.DeleteTicketBoxAsync(ticketId);
            if (callResult.Success)
            {

                ModelState.Clear();

                return Json(
                    new
                    {
                        success = true,
                        warningMessages = callResult.WarningMessages,
                        successMessages = callResult.SuccessMessages,
                    });
            }

            return Json(
                new
                {
                    success = false,
                    errorMessages = callResult.ErrorMessages
                });

        }
        [AjaxOnly]
        [HttpGet]
        public ActionResult Add()
        {

            
            var model = new TicketBoxAddViewModel
            {
                 
            };
            return PartialView("~/Areas/Admin/Views/TicketBox/_TicketAdd.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(TicketBoxAddViewModel model)
        {
             

            if (ModelState.IsValid)
            {
                var callResult = await _ticketBoxService.AddTicketAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (TicketBoxViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/DisplayTemplates/TicketBoxViewModel.cshtml", viewModel),
                            item = viewModel
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/_TicketAdd.cshtml", model)
                });

        }
        public ActionResult TicketBoxShow()
        {
            ViewBag.Title = "Kullanıcıdan gelen ticketlar";
            return View("~/Areas/Admin/Views/TicketBox/TicketBoxShow.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public ActionResult TicketListShow(TicketBoxShowViewModel contactViewModel, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _ticketBoxService.GetTicketListShowIQueryable(contactViewModel).OrderBy(a => a.isAnswer).OrderByDescending(a=>a.Date)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultServicePageSize);


            ModelState.Clear();
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/TicketListShow.cshtml", result)
                })
            };
        }
      


    }
}
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.Admin;
using Warehouse.ViewModels.Common;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    public class TicketBoxAnswerController : AdminBaseController
    {
        private readonly TicketBoxAnswerService _ticketBoxService;
        public TicketBoxAnswerController(TicketBoxAnswerService ticketBoxService)
        {
            _ticketBoxService = ticketBoxService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [AjaxOnly]
        public async Task<ActionResult> TicketBoxAnswerShow(int ticketId)
        {
            var model = await _ticketBoxService.GetTicketAnswerShowModelAsync(ticketId).ConfigureAwait(false);
            if (model != null)
            {
                return PartialView("~/Areas/Admin/Views/TicketBox/_TicketAnswerView.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Destek talebiniz henüz yanıtlanmadı!");

        }
        [AjaxOnly,HttpGet]
        public async Task<ActionResult> TicketBoxAnswerAsync(int ticketId)
        {
            var result = await _ticketBoxService.GetTicketAnswerShowModelAsync(ticketId).ConfigureAwait(false);
            var model = await _ticketBoxService.GetTicketAnswerViewModelAsync(ticketId);
            if (model.isAnswer == true)
            {
                return PartialView("~/Areas/Admin/Views/TicketBox/_TicketAnswerView.cshtml", result);
            }
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/TicketBoxAnswer/_TicketAnswer.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Ticket sistemde bulunamadı!");
           

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> TicketBoxAnswerAsync(TicketBoxAnswerAddViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _ticketBoxService.AnswerTicketAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (TicketBoxShowViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/DisplayTemplates/TicketBoxShowViewModel.cshtml", viewModel),
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
        [AjaxOnly, HttpGet]
        public async Task<ActionResult> TicketBoxAnswerEditAsync(int ticketId)
        {

            var model = await _ticketBoxService.GetTicketAnswerEditViewModelAsync(ticketId);
           
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/TicketBoxAnswer/_TicketAnswerEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Ticket henüz yanıtlanmadı");


        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> TicketBoxAnswerEditAsync(TicketBoxAnswerEditViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _ticketBoxService.EditTicketAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (TicketBoxShowViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/DisplayTemplates/TicketBoxShowViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/TicketBox/_TicketAnswerEdit.cshtml", model)
                });

        }
        [AjaxOnly]
        public async Task<ActionResult> AnsweredTicketBox(int ticketId)
        {
            var model = await _ticketBoxService.GetTicketAnswerShowModelAsync(ticketId).ConfigureAwait(false);
            if (model != null)
            {
                return PartialView("~/Areas/Admin/Views/TicketBox/_TicketAnswerView.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Destek talebiniz henüz yanıtlanmadı!");

        }
    }
}
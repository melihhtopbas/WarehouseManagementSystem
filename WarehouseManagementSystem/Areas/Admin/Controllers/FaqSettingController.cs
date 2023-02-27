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
    public class FaqSettingController : AdminBaseController
    {
        private readonly FaqService _faqService;
        private readonly LanguageService _languageService;
        private readonly CategoryService _categoryService;

        public FaqSettingController(FaqService faqService
            , LanguageService languageService,
            CategoryService categoryService)
        {
            _faqService = faqService;
            _languageService = languageService;
            _categoryService = categoryService;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "SSS";
            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            return View("~/Areas/Admin/Views/FaqSetting/Index.cshtml");
        }
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> FaqList(FaqSearchViewModel model, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _faqService.GetFaqsListIQueryable(model)
                .OrderBy(p => p.Name)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultPropertyPageSize);

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            ModelState.Clear();
            ViewBag.LanguageId = model.LanguageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/FaqSetting/FaqList.cshtml", result)
                })
            };
        }
        [AjaxOnly]
        public ActionResult Add(long languageId)
        {
             
            ViewData["Categories"] = _categoryService.GetFaqCategoryListIQueryable(languageId).ToList();
            var model = new FaqAddViewModel()
            {
                LanguageId = languageId,

            };
            return PartialView("~/Areas/Admin/Views/FaqSetting/_FaqAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FaqAddViewModel model)
        {


            if (ModelState.IsValid)
            {
                var callResult = await _faqService.AddFaqAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (FaqListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/FaqSetting/DisplayTemplates/FaqListViewModel.cshtml", viewModel),
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
            ViewData["Categories"] = _categoryService.GetFaqCategoryListIQueryable(model.LanguageId).ToList();
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/FaqSetting/_FaqAdd.cshtml", model)
                });

        }

        public async Task<ActionResult> Edit(int faqId)
        {
            var model = await _faqService.GetFaqEditViewModelAsync(faqId);
            if (model != null)
            {
                ViewData["Categories"] = _categoryService.GetFaqCategoryListIQueryable(model.LanguageId).ToList(); 
                return PartialView("~/Areas/Admin/Views/FaqSetting/_FaqEdit.cshtml", model);
            }

            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "SSS sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FaqEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var callResult = await _faqService.EditFaqAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (FaqListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/FaqSetting/DisplayTemplates/FaqListViewModel.cshtml", viewModel),
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
            ViewData["Categories"] = _categoryService.GetFaqCategoryListIQueryable(model.LanguageId).ToList(); 
            return Json(
                new
                {
                    success = false,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/FaqSetting/_FaqEdit.cshtml", model)
                });

        }

        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int faqId)
        {
            var callResult = await _faqService.DeleteFaqAsync(faqId);
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
    }
}
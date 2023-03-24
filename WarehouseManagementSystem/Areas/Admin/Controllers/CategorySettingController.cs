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
using WarehouseManagementSystem.Areas.Security;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [CustomAuthorize("admin")]
    public class CategorySettingController : AdminBaseController
    {
        private readonly CategoryService _categoryService;
        private readonly LanguageService _languageService;


        public CategorySettingController(CategoryService categoryService
            , LanguageService languageService)
        {
            _categoryService = categoryService;
            _languageService = languageService;
        }
        public async Task<ActionResult> FaqCategory()
        {
            ViewBag.Title = "Kategoriler";

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

            //     var model = new ServiceListSearchViewModel() { Code = stockCode, ServiceSearchVariantOption = ServiceSearchVariantEnum.All };
            return View("~/Areas/Admin/Views/CategorySetting/FaqCategory.cshtml");
        }

        public async Task<ActionResult> FaqCategoryList(long languageId, int? page)
        
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _categoryService.GetFaqCategoryListIQueryable(languageId)
            .OrderBy(p => p.Id)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultPropertyPageSize);

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();



            ModelState.Clear();
            ViewBag.LanguageId = languageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CategorySetting/FaqCategoryList.cshtml", result)
                })
            };
        }
        [AjaxOnly, HttpGet]
        public ActionResult FaqCategoryAdd(long languageId)
        {
            ViewData["Categories"] = _categoryService.GetFaqCategoryListIQueryable(languageId).ToList(); 
            var model = new FaqCategoryCrudViewModel
            {
                LanguageId = languageId,

            };
            return PartialView("~/Areas/Admin/Views/CategorySetting/_FaqCategoryAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> FaqCategoryAdd(FaqCategoryCrudViewModel model)
        {
            var dd = Request.Form;

            if (ModelState.IsValid)
            {
                var callResult = await _categoryService.AddFaqCategoryAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CategoryListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CategorySetting/DisplayTemplates/CategoryListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CategorySetting/_FaqCategoryAdd.cshtml", model)
                });

        }
        public async Task<ActionResult> FaqCategoryEdit(long categoryId)
        {
            var model = await _categoryService.GetFaqCategoryEditViewModelAsync(categoryId);
            if (model != null)
            {

                ViewData["Categories"] = _categoryService.GetFaqCategoryListIQueryable(model.LanguageId).ToList(); 
                return PartialView("~/Areas/Admin/Views/CategorySetting/_FaqCategoryEdit.cshtml", model);
            }

            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Servis sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> FaqCategoryEdit(FaqCategoryCrudViewModel model)
        {
            List<string> tags = new List<string>();


            if (ModelState.IsValid)
            {
                var callResult = await _categoryService.EditFaqCategoryAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (CategoryListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/CategorySetting/DisplayTemplates/CategoryListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/CategorySetting/_FaqCategoryEdit.cshtml", model)
                });

        }
    }
}
using ImageResizer;
using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class SliderSettingController : AdminBaseController
    {
        private readonly SliderService _sliderService;
        private readonly LanguageService _languageService;


        public SliderSettingController(SliderService sliderService
            , LanguageService languageService)
        {
            _sliderService = sliderService;
            _languageService = languageService;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Slider";

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();

           
            return View("~/Areas/Admin/Views/SliderSetting/Index.cshtml");
        }
        // GET: Admin/BlogSetting
        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> SliderList(long languageId, int? page)
        {
            var currentPageIndex = page - 1 ?? 0;

            var result = _sliderService.GetSliderListIQueryable(languageId)
                .OrderBy(p => p.Id)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultBlogPageSize);

            ViewBag.Languages = await _languageService.GetLanguageListViewAsync();



            ModelState.Clear();
            ViewBag.LanguageId = languageId;
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/SliderList.cshtml", result)
                })
            };
        }
        [AjaxOnly,HttpGet]
        public ActionResult Add(long languageId)
        {
             
            var model = new SliderCrudViewModel
            {
                LanguageId = languageId,

            };
            return PartialView("~/Areas/Admin/Views/SliderSetting/_SliderAdd.cshtml", model);
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(SliderCrudViewModel model)
        {
            HttpFileCollectionBase Files = Request.Files;
            if (Files == null)
            {
                ModelState.AddModelError("", "Resim boş olamaz");
                return Json(
                    new
                    {
                        success = false,
                        responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/_SliderAdd.cshtml", model)
                    });
            }

            HttpPostedFileBase ImageFile = Files[0];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                if (Path.GetExtension(ImageFile.FileName) == ".mp4")
                {
                    var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SliderImagePath));
                    var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SliderImageThumbPath));

                    if (!System.IO.Directory.Exists(tempImageDirectory))
                        System.IO.Directory.CreateDirectory(tempImageDirectory);

                    if (!System.IO.Directory.Exists(tempImageThumbDirectory))
                        System.IO.Directory.CreateDirectory(tempImageThumbDirectory);

                    string fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(ImageFile.FileName)}";
                    fileName = Path.GetFileName(ImageFile.FileName);
                    var extention = Path.GetExtension(ImageFile.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(ImageFile.FileName);

                    ImageFile.SaveAs(Server.MapPath("/uploads/sliders/images/" + fileName));

                    model.FileName = fileName;

                }
                else
                {
                    var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SliderImagePath));
                    var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SliderImageThumbPath));

                    if (!System.IO.Directory.Exists(tempImageDirectory))
                        System.IO.Directory.CreateDirectory(tempImageDirectory);

                    if (!System.IO.Directory.Exists(tempImageThumbDirectory))
                        System.IO.Directory.CreateDirectory(tempImageThumbDirectory);

                    string fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(ImageFile.FileName)}";

                    string pathImage = System.IO.Path.Combine(tempImageDirectory, fileName);
                    string pathImageThumb = System.IO.Path.Combine(tempImageThumbDirectory, fileName);

                    ImageBuilder.Current.Build(ImageFile, pathImage, new ResizeSettings(SystemConstants.ImageResizerServiceImageSettings));
                    ImageBuilder.Current.Build(ImageFile, pathImageThumb, new ResizeSettings(SystemConstants.ImageResizerServiceThumbImageSettings));
                    model.FileName = fileName;
                }
            }
            if (ModelState.IsValid)
            {
                var callResult = await _sliderService.AddSliderAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (SliderListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/DisplayTemplates/SliderListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/_SliderAdd.cshtml", model)
                });

        }
        public async Task<ActionResult> Edit(int sliderId)
        {
            var model = await _sliderService.GetSliderViewModelAsync(sliderId);
            if (model != null)
            {
                 
                return PartialView("~/Areas/Admin/Views/SliderSetting/_SliderEdit.cshtml", model);
            }

            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Servis sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SliderCrudViewModel model)
        {

            HttpFileCollectionBase Files = Request.Files;
            if (Files == null && string.IsNullOrWhiteSpace(model.FileName))
            {
                ModelState.AddModelError("", "Resim boş olamaz");
                return Json(
                    new
                    {
                        success = false,
                        responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/_SliderAdd.cshtml", model)
                    });
            }

            HttpPostedFileBase ImageFile = Files[0];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SliderImagePath));
                var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SliderImageThumbPath));

                if (!System.IO.Directory.Exists(tempImageDirectory))
                    System.IO.Directory.CreateDirectory(tempImageDirectory);

                if (!System.IO.Directory.Exists(tempImageThumbDirectory))
                    System.IO.Directory.CreateDirectory(tempImageThumbDirectory);

                string fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(ImageFile.FileName)}";

                string pathImage = System.IO.Path.Combine(tempImageDirectory, fileName);
                string pathImageThumb = System.IO.Path.Combine(tempImageThumbDirectory, fileName);

                ImageBuilder.Current.Build(ImageFile, pathImage, new ResizeSettings(SystemConstants.ImageResizerServiceImageSettings));
                ImageBuilder.Current.Build(ImageFile, pathImageThumb, new ResizeSettings(SystemConstants.ImageResizerServiceThumbImageSettings));
                model.FileName = fileName;
            }


            if (ModelState.IsValid)
            {
                var callResult = await _sliderService.EditSliderCategoryAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (SliderListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/DisplayTemplates/SliderListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/SliderSetting/_SliderEdit.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int sliderId)
        {
            var callResult = await _sliderService.DeleteSliderAsync(sliderId);
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
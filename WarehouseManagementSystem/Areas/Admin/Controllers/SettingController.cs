using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.Admin;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.Areas.Security;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    [CustomAuthorize("admin")]
    public class SettingController : AdminBaseController
    {
        private readonly SettingService _settingService;
        public SettingController(SettingService settingService)
        {
            _settingService = settingService;
        }

        // GET: Admin/Setting
        public async Task<ActionResult> Index(long id)
        {
            ViewBag.Title = "Ayarlar";

            var model = await _settingService.GetSettingViewModel(id).ConfigureAwait(false);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SettingViewModel model, HttpPostedFileBase logoFile, HttpPostedFileBase faviconFile)
        {
            if (logoFile != null)
            {
                if (!System.IO.Directory.Exists(System.IO.Path.Combine(Server.MapPath("/uploads/"))))
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Server.MapPath("/uploads/")));
                var fileName = Path.GetFileName(logoFile.FileName);
                var extention = Path.GetExtension(logoFile.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(logoFile.FileName);
                logoFile.SaveAs(Server.MapPath("/uploads/" + logoFile.FileName));
                model.Logo = "/uploads/" + logoFile.FileName;
            }
            if (faviconFile != null)
            {
                if (!System.IO.Directory.Exists(System.IO.Path.Combine(Server.MapPath("/uploads/"))))
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Server.MapPath("/uploads/")));
                var fileName = Path.GetFileName(faviconFile.FileName);
                var extention = Path.GetExtension(faviconFile.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(faviconFile.FileName);
                faviconFile.SaveAs(Server.MapPath("/uploads/" + faviconFile.FileName));
                model.Favicon = "/uploads/" + faviconFile.FileName;
            }
            if (ModelState.IsValid)
            {

                var callResult = await _settingService.AddOrEditSetting(model);
                if (callResult.Success)
                {
                    ViewBag.Title = "Ayarlar";
                    ViewData[StringConstants.SuccessMessage] = "Ayarlar Başarıyla Kaydedilmiştir.";
                    TempData["Msg"] = "İşlem başarılı!";
                    return View("~/Areas/Admin/Views/Setting/Index.cshtml", model);

                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.Title = "Ayarlar";
            return View("~/Areas/Admin/Views/Setting/Index.cshtml", model);
        }
        public async Task<ActionResult> About(long id)
        {
            ViewBag.Title = "Hakkımızda";

            var model = await _settingService.GetAboutViewModel(id).ConfigureAwait(false);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> About(AboutViewModel model)
        {
            HttpFileCollectionBase Files = Request.Files;
            HttpPostedFileBase ImageFile = Files[0];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImagePath));
                var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImageThumbPath));

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
            HttpPostedFileBase ImageFile2 = Files[1];
            if (ImageFile2 != null && ImageFile2.ContentLength > 0)
            {
                var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImagePath));
                var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImageThumbPath));

                if (!System.IO.Directory.Exists(tempImageDirectory))
                    System.IO.Directory.CreateDirectory(tempImageDirectory);

                if (!System.IO.Directory.Exists(tempImageThumbDirectory))
                    System.IO.Directory.CreateDirectory(tempImageThumbDirectory);

                string fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(ImageFile2.FileName)}";

                string pathImage = System.IO.Path.Combine(tempImageDirectory, fileName);
                string pathImageThumb = System.IO.Path.Combine(tempImageThumbDirectory, fileName);

                ImageBuilder.Current.Build(ImageFile2, pathImage, new ResizeSettings(SystemConstants.ImageResizerServiceImageSettings));
                ImageBuilder.Current.Build(ImageFile2, pathImageThumb, new ResizeSettings(SystemConstants.ImageResizerServiceThumbImageSettings));
                model.FileName2 = fileName;
            }

            if (ModelState.IsValid)
            {

                var callResult = await _settingService.AddorEditAbout(model);
                if (callResult.Success)
                {
                    ViewBag.Title = "Hakkımızda";
                    TempData["Msg"] = "İşlem başarılı!";
                    return View("~/Areas/Admin/Views/Setting/About.cshtml", model);

                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.Title = "Hakkımızda";
            return View("~/Areas/Admin/Views/Setting/About.cshtml", model);
        }
        public async Task<ActionResult> Blog(long id)
        {
            ViewBag.Title = "Blogumuz";

            var model = await _settingService.GetBlogViewModel(id).ConfigureAwait(false);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public async Task<ActionResult> Blog(BlogViewModel model)
        {
            HttpFileCollectionBase Files = Request.Files;
            HttpPostedFileBase ImageFile = Files[0];
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImagePath));
                var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImageThumbPath));

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
            HttpPostedFileBase ImageFile2 = Files[1];
            if (ImageFile2 != null && ImageFile2.ContentLength > 0)
            {
                var tempImageDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImagePath));
                var tempImageThumbDirectory = System.IO.Path.Combine(Server.MapPath(SystemConstants.SettingImageThumbPath));

                if (!System.IO.Directory.Exists(tempImageDirectory))
                    System.IO.Directory.CreateDirectory(tempImageDirectory);

                if (!System.IO.Directory.Exists(tempImageThumbDirectory))
                    System.IO.Directory.CreateDirectory(tempImageThumbDirectory);

                string fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(ImageFile2.FileName)}";

                string pathImage = System.IO.Path.Combine(tempImageDirectory, fileName);
                string pathImageThumb = System.IO.Path.Combine(tempImageThumbDirectory, fileName);

                ImageBuilder.Current.Build(ImageFile2, pathImage, new ResizeSettings(SystemConstants.ImageResizerServiceImageSettings));
                ImageBuilder.Current.Build(ImageFile2, pathImageThumb, new ResizeSettings(SystemConstants.ImageResizerServiceThumbImageSettings));
                model.FileName2 = fileName;
            }

            if (ModelState.IsValid)
            {

                var callResult = await _settingService.AddorEditBlog(model);
                if (callResult.Success)
                {
                    ViewBag.Title = "Hakkımızda";
                    TempData["Msg"] = "İşlem başarılı!";
                    return View("~/Areas/Admin/Views/Setting/Blog.cshtml", model);

                }
                foreach (var error in callResult.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.Title = "Hakkımızda";
            return View("~/Areas/Admin/Views/Setting/Blog.cshtml", model);
        }
    }
}
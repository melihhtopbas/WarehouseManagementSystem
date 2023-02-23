﻿using ImageResizer;
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

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
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
                    ViewData[StringConstants.SuccessMessage] = "Hakkımızda Başarıyla Kaydedilmiştir.";
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
    }
}
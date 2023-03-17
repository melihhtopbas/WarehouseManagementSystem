using Microsoft.Web.Mvc;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Warehouse.Data;
using Warehouse.Service.Admin;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;

namespace WarehouseManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class UserSettingController : AdminBaseController
    {
        private readonly UserSettingService _userSettingService;
        public UserSettingController(UserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
        }
    
        public async Task<ActionResult> Index(string userName)
        {

            ViewBag.Title = "Kullanıcılar";

            var model = new UserSearchViewModel
            {
                Name = userName,
            };
            return View("~/Areas/Admin/Views/UserSetting/Index.cshtml",model);

        }


        [AjaxOnly, HttpPost, ValidateInput(false)]
        public async Task<ActionResult> UserList(UserSearchViewModel searchViewModel, int? page)
        {


            var currentPageIndex = page - 1 ?? 0;

            var result = _userSettingService.GetUserListIQueryable(searchViewModel)
            .OrderBy(x => x.Name)
                .ToPagedList(currentPageIndex, SystemConstants.DefaultServicePageSize);

            

            ModelState.Clear();
          
            return new ContentResult
            {
                ContentType = "application/json",
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(new
                {
                    success = true,
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserSetting/UserList.cshtml", result)
                })
            };
        }
        [HttpGet,AjaxOnly]
        public async Task<ActionResult> UserEdit(int userId)
        {
            ViewData["Cities"] = _userSettingService.GetUserCityList(20002);
            ViewData["Countries"] = _userSettingService.GetUserCountryList().ToList();

            var model = await _userSettingService.GetUserEditViewModelAsync(userId);
            model.Country.CountryId = 20002;
            if (model != null)
            {

                return PartialView("~/Areas/Admin/Views/UserSetting/_UserEdit.cshtml", model);
            }
            return PartialView("~/Areas/Admin/Views/Shared/_ItemNotFoundPartial.cshtml", "Müşteri sistemde bulunamadı!");
        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(UserEditViewModel model)
        {
            ViewData["Cities"] = _userSettingService.GetUserCityList(model.Country.CountryId).ToList();
            ViewData["Countries"] = _userSettingService.GetUserCountryList().ToList();

            if (ModelState.IsValid)
            {
                var callResult = await _userSettingService.EditUserAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (UserListViewModel)callResult.Item;


                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserSetting/DisplayTemplates/UserListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserSetting/_UserEdit.cshtml", model)
                });

        }
        [AjaxOnly,HttpGet]
        public ActionResult Add()
        {

            ViewData["Cities"] = _userSettingService.GetUserCityList(20002);
            ViewData["Countries"] = _userSettingService.GetUserCountryList().ToList();

            var model = new UserAddViewModel
            {
                City = new OrderCityIdSelectViewModel
                {

                },
                Country = new OrderCountryIdSelectViewModel
                {
                    CountryId = 20002
                },

            };
            return PartialView("~/Areas/Admin/Views/UserSetting/_UserAdd.cshtml", model);

        }
        [HttpPost, ValidateInput(false), ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(UserAddViewModel model)
        {
            ViewData["Cities"] = _userSettingService.GetUserCityList(model.Country.CountryId).ToList();
            ViewData["Countries"] = _userSettingService.GetUserCountryList().ToList();

            if (ModelState.IsValid)
            {
                var callResult = await _userSettingService.AddUserAsync(model);
                if (callResult.Success)
                {

                    ModelState.Clear();
                    var viewModel = (UserListViewModel)callResult.Item;
                    var jsonResult = Json(
                        new
                        {
                            success = true,
                            responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserSetting/DisplayTemplates/UserListViewModel.cshtml", viewModel),
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
                    responseText = RenderPartialViewToString("~/Areas/Admin/Views/UserSetting/_UserAdd.cshtml", model)
                });

        }
        [AjaxOnly, HttpPost]
        public async Task<ActionResult> Delete(int userId)
        {
            var callResult = await _userSettingService.DeleteUserAsync(userId);
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
                    warningMessages = callResult.WarningMessages,
                    infoMessages = callResult.InfoMessages,
                });

        }

        

    }
}
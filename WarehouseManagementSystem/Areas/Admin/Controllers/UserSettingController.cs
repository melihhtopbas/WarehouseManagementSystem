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
    
        public async Task<ActionResult> Index()
        {

            ViewBag.Title = "Kullanıcılar";
            

            return View("~/Areas/Admin/Views/UserSetting/Index.cshtml");

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
    }
}
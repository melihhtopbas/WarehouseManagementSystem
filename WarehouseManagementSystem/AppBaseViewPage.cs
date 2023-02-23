using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.ViewModels.WebSite;
using WarehouseManagementSystem.Controllers;

namespace WarehouseManagementSystem
{
    public abstract class AppBaseViewPage<TModel> : WebViewPage<TModel>
    {
        protected SettingViewModel SettingViewModel
        {
            get
            {
                if (ViewContext.Controller is BaseController baseController)
                    return baseController.SettingViewModel ?? new SettingViewModel();
                return null;
            }
        }


    }

    public abstract class AppBaseViewPage : AppBaseViewPage<dynamic>
    {
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;
using Warehouse.ViewModels.WebSite;
using WarehouseManagementSystem.Areas.Admin.Controllers;
using WarehouseManagementSystem.Controllers;

namespace WarehouseManagementSystem
{
    public abstract class AppBaseViewPage<TModel> : WebViewPage<TModel>
    {
        protected Warehouse.ViewModels.WebSite.SettingViewModel SettingViewModel
        {
            get
            {
                if (ViewContext.Controller is BaseController baseController)
                    return baseController.SettingViewModel ?? new Warehouse.ViewModels.WebSite.SettingViewModel();
                return null;
            }
        }

        protected CurrentUserViewModel CurrentUserViewModel
        {
            get
            {
                if (ViewContext.Controller is AdminBaseController adminBaseController)
                    return adminBaseController.CurrentUserViewModel ?? new CurrentUserViewModel();
                return null;
            }
        }
        protected List<IncomingMessageViewModel> IncomingMessageViewModel
        {
            get
            {
                if (ViewContext.Controller is AdminBaseController adminBaseController)
                    return adminBaseController.IncomingMessageViewModel ?? new List<IncomingMessageViewModel>();
                return null;
            }
        }
        protected List<TicketMessageViewModel> TicketMessageViewModel
        {
            get
            {
                if (ViewContext.Controller is AdminBaseController adminBaseController)
                    return adminBaseController.TicketMessageViewModel ?? new List<TicketMessageViewModel>();
                return null;
            }
        }


    }

    public abstract class AppBaseViewPage : AppBaseViewPage<dynamic>
    {
    }
}
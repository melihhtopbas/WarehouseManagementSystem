using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Warehouse.Service.WebSite;
using Warehouse.ViewModels.WebSite;

namespace WarehouseManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            // Modify current thread's cultures  
            var culture = new CultureInfo(requestContext.RouteData.Values["lang"].ToString());
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;


            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentCulture.ToString();
            ViewBag.CurrentCultureTwoLetter = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            base.Initialize(requestContext);
        }

        public SettingViewModel SettingViewModel;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SettingViewModel = DependencyResolver.Current.GetService<SettingService>().GetSettingViewModel(filterContext.RouteData.Values["lang"].ToString());
            base.OnActionExecuting(filterContext);
        }
        public string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
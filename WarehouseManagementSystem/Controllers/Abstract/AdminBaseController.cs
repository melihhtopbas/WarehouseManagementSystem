using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace WarehouseManagementSystem.Controllers.Abstract
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class AdminBaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            // Modify current thread's cultures  
            var culture = new System.Globalization.CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;


            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentCulture.ToString();
            ViewBag.CurrentCultureTwoLetter = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            base.Initialize(requestContext);
        }





        public ActionResult RedirectToLocalOr(string returnUrl, Func<ActionResult> action)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return action();

        }

        public ActionResult RedirectToPreviousOr(Func<ActionResult> action)
        {
            var httpContext = ControllerContext.HttpContext;

            var previousUrl = httpContext.Request.UrlReferrer?.ToString();

            return Url.IsLocalUrl(previousUrl) ? new RedirectResult(previousUrl) : action();
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
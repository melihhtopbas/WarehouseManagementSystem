using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using Warehouse.ViewModels.Common;
using Warehouse.Service.Admin;

namespace WarehouseManagementSystem.Areas.Security
{
    public class CustomAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        private readonly CurrentUserViewModel _currentUser;
        public CustomAuthenticationFilter()
        {

            _currentUser = DependencyResolver.Current.GetService<CurrentUserService>().GetCurrentUserViewModel(HttpContext.Current.User.Identity.Name);
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.User.Identity.Name)))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //Redirecting the user to the Login View of Account Controller  
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "Login" },
                     { "action", "Index" }, 
                     
                });
            }
        }
    }
}
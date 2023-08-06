using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Warehouse.Data;

namespace WarehouseManagementSystem.Areas.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        private readonly WarehouseManagementSystemEntities1 _context = new WarehouseManagementSystemEntities1();
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
      

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userId = Convert.ToString(HttpContext.Current.User.Identity.Name);
            if (!string.IsNullOrEmpty(userId))
                using (var context = new WarehouseManagementSystemEntities1())
                {
                    var userRole = (from u in context.Users
                                    join ur in context.UserRoles on u.Id equals ur.UserId
                                    join r in context.Roles on ur.RoleId equals r.Id
                                    where u.UserName == userId && ur.Active == true
                                    select new
                                    {
                                        r.Name
                                    }).ToList();
                    foreach (var role in allowedroles)
                    {
                        foreach (var item in userRole)
                        {
                            if (role == item.Name) return true;
                        }
                         
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            if (action != "Index" && action != "Blog" && action != "FaqCategory" && action != "CustomerOrderPackages" && action != "AllCustomerOrder" && action != "About")
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                      
                      { "controller", "Authentication" },
                      { "action", "NotAuthorized" },
                      
                   });
            }
            else
                filterContext.Result = new RedirectToRouteResult(
                      new RouteValueDictionary
                      {
                      { "controller", "Authentication" },
                      { "action", "AccessDenied" },

                      });
        }
      


    }
}
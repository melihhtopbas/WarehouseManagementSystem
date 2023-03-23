﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
                                    join r in context.Roles on u.RoleId equals r.Id
                                    where u.UserName == userId
                                    select new
                                    {
                                        r.Name
                                    }).FirstOrDefault();
                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.Name) return true;
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            filterContext.Result = new RedirectToRouteResult(
       new RouteValueDictionary
       {
                      { "controller", "Authentication" },
                      { "action", "AccessDenied" }
       });
        }


    }
}
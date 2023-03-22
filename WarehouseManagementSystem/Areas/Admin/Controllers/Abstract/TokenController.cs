using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using WarehouseManagementSystem.App_Start;
using WarehouseManagementSystem.Areas.Security;

namespace WarehouseManagementSystem.Areas.Admin.Controllers.Abstract
{
    [CustomAuthenticationFilter]
    public class TokenController : Controller
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public TokenController(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        [CustomAuthorize("Normal", "SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GenerateToken(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                 
            }

            var user = await _context.Users.FindAsync(model.UserName, model.Password);

            if (user == null)
            {
                 
            }

            var identity = new ClaimsIdentity(AuthenticationStartup.OAuthBearerOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
           
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

            var ticket = new AuthenticationTicket(identity, null);
            var accessToken = AuthenticationStartup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            return Json(new
            {
                access_token = accessToken,
                token_type = "bearer",
                expires_in = TimeSpan.FromDays(1).TotalSeconds
            });
        }

    }
}
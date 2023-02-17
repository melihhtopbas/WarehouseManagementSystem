using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Infrastructure;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;
using Warehouse.ViewModels.WebSite;

namespace Warehouse.Service.WebSite
{
    public class SettingService
    {
        private readonly WarehouseManagementSystemEntities1 _context; 
        public SettingService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
            
        }
        public AboutViewModel GetAboutViewModel(string languageCode)
        {
            var model = (from a in _context.About
                         where a.Languages.ShortName == languageCode
                         select new AboutViewModel()
                         {
                             Description = a.Description,
                             Title = a.Title,
                             Vision = a.Vision,
                             Mission = a.Mission,
                             MainFile = a.FileName,
                             MainFile2 = a.FileName2
                         }).FirstOrDefault();

            return model;

        }

         
        public bool Login(LoginViewModel login)
        {
            return _context.Users.Any(x => x.Password == login.Password && x.UserName == login.UserName);
        }

        public async Task<ServiceCallResult> RegisterAsync(RegisterViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool response = _context.Users.Any(x => x.UserName == model.UserName);
            if (response)
            {
                callResult.ErrorMessages.Add("Sistemde bu kullanıcı mevcut");

                return callResult;
            }

            var register = new Users
            {
                UserName = model.UserName,
                Password = model.Password,
            };
            

            _context.Users.Add(register);
        
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                 

                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public BlogViewModel GetBlogViewModel(string languageCode)
        {
            var model = (from a in _context.Blog
                         where a.Languages.ShortName == languageCode
                         select new BlogViewModel()
                         {
                             Description = a.Description,
                             Title = a.Title,
                             Vision = a.Vision,
                             Mission = a.Mission,
                             MainFile = a.FileName,
                             MainFile2 = a.FileName2
                         }).FirstOrDefault();

            return model;

        }

    }
}

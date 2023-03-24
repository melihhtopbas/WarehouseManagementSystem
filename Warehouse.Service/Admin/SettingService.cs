using Microsoft.VisualBasic.Logging;
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
using AboutViewModel = Warehouse.ViewModels.Admin.AboutViewModel;

namespace Warehouse.Service.Admin
{
    public class SettingService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public SettingService(WarehouseManagementSystemEntities1 context)
        {
            _context = context; 
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
                Mail = model.Mail,
                Name= model.Name,   
                Phone = model.Phone,
                Surname = model.Surname,
                Date = DateTime.Now,
                CityId = model.City.CityId,
                CountryId = model.Country.CountryId,
                
            };

           

            _context.Users.Add(register);
            _context.SaveChanges();

            var roles = _context.Roles.Where(x => x.Name != "admin").ToList();
            foreach (var item in roles)
            {
                _context.UserRoles.Add(new UserRoles
                {
                    Active= true,
                    RoleId = item.Id,
                    UserId = register.Id,
                });
            }

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
        public async Task<AboutViewModel> GetAboutViewModel(long languageId)
        {
            var model = await (from a in _context.About
                               where a.LanguageId == languageId
                               select new AboutViewModel()
                               {
                                   Description = a.Description,
                                   Title = a.Title,
                                   Vision = a.Vision,
                                   Mission = a.Mission,
                                   FileName = a.FileName,
                                   FileName2 = a.FileName2,
                                   LanguageId = a.LanguageId
                               }).SingleOrDefaultAsync().ConfigureAwait(false);
            if (model == null)
            {
                model = new AboutViewModel()
                {
                    LanguageId = languageId
                };

            }
            return model;

        }

        public async Task<ServiceCallResult> AddorEditAbout(AboutViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            var about = await _context.About.FirstOrDefaultAsync(a => a.LanguageId == model.LanguageId);

            if (about == null)
            {
                about = new About()
                {
                    LanguageId = model.LanguageId,
                    Description = model.Description,
                    Vision = model.Vision,
                    Mission = model.Mission,
                    FileName = model.FileName,
                    FileName2 = model.FileName2,
                    Title = model.Description

                };
                _context.About.Add(about);
            }
            else
            {
                about.Description = model.Description;
                about.Vision = model.Vision;
                about.Mission = model.Mission;
                about.FileName = string.IsNullOrWhiteSpace(model.FileName) ? about.FileName : model.FileName;
                about.FileName2 = string.IsNullOrWhiteSpace(model.FileName2) ? about.FileName2 : model.FileName2;
                about.Title = model.Title;
            }
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
        public async Task<ViewModels.Admin.BlogViewModel> GetBlogViewModel(long languageId)
        {
            var model = await (from a in _context.Blog
                               where a.LanguageId == languageId
                               select new ViewModels.Admin.BlogViewModel()
                               {
                                   Description = a.Description,
                                   Title = a.Title,
                                   Vision = a.Vision,
                                   Mission = a.Mission,
                                   FileName = a.FileName,
                                   FileName2 = a.FileName2,
                                   LanguageId = a.LanguageId
                               }).SingleOrDefaultAsync().ConfigureAwait(false);
            if (model == null)
            {
                model = new ViewModels.Admin.BlogViewModel()
                {
                    LanguageId = languageId
                };

            }
            return model;

        }

        public async Task<ServiceCallResult> AddorEditBlog(ViewModels.Admin.BlogViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            var blog = await _context.Blog.FirstOrDefaultAsync(a => a.LanguageId == model.LanguageId);

            if (blog == null)
            {
                blog = new Blog()
                {
                    LanguageId = model.LanguageId,
                    Description = model.Description,
                    Vision = model.Vision,
                    Mission = model.Mission,
                    FileName = model.FileName,
                    FileName2 = model.FileName2,
                    Title = model.Description

                };
                _context.Blog.Add(blog);
            }
            else
            {
                blog.Description = model.Description;
                blog.Vision = model.Vision;
                blog.Mission = model.Mission;
                blog.FileName = string.IsNullOrWhiteSpace(model.FileName) ? blog.FileName : model.FileName;
                blog.FileName2 = string.IsNullOrWhiteSpace(model.FileName2) ? blog.FileName2 : model.FileName2;
                blog.Title = model.Title;
            }
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
        public async Task<ViewModels.Admin.SettingViewModel> GetSettingViewModel(long languageId)
        {
            var model = await (from a in _context.Settings
                               where a.LanguageId == languageId
                               select new ViewModels.Admin.SettingViewModel()
                               {
                                   Adress = a.Adress,
                                   Analytics = a.Analytics,
                                   Email = a.Email,
                                   Facebook = a.Facebook,
                                   Gplus = a.Gplus,
                                   Instagram = a.Instagram,
                                   Maps = a.Maps,
                                   Meta = a.Meta,
                                   Phone = a.Phone,
                                   Phone2 = a.Phone2,
                                   SeoDescription = a.Description,
                                   SeoKeywords = a.Keywords,
                                   SeoTitle = a.Title,
                                   Twitter = a.Twitter,
                                   Youtube = a.Youtube,
                                   LanguageId = languageId,
                                   Logo = a.Logo,
                                   Favicon = a.Favicon

                               }).SingleOrDefaultAsync().ConfigureAwait(false);

            if (model == null)
            {
                model = new ViewModels.Admin.SettingViewModel()
                {
                    LanguageId = languageId
                };

            }
            return model;

        }
        public async Task<ServiceCallResult> AddOrEditSetting(ViewModels.Admin.SettingViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            var setting = await _context.Settings.FirstOrDefaultAsync(a => a.LanguageId == model.LanguageId);
            if (setting == null)
            {
                setting = new Settings()
                {
                    LanguageId = model.LanguageId,
                    Description = model.SeoDescription,
                    Title = model.SeoTitle,
                    Twitter = model.Twitter,
                    Youtube = model.Youtube,
                    Adress = model.Adress,
                    Analytics = model.Analytics,
                    Email = model.Email,
                    Facebook = model.Facebook,
                    Gplus = model.Gplus,
                    Instagram = model.Instagram,
                    Keywords = model.SeoKeywords,
                    Maps = model.Maps,
                    Phone = model.Phone,
                    Phone2 = model.Phone2,
                    Meta = model.Meta,
                    Logo = model.Logo,
                    Favicon = model.Favicon


                };
                _context.Settings.Add(setting);
            }
            else
            {
                setting.Description = model.SeoDescription;
                setting.Title = model.SeoDescription;
                setting.Twitter = model.Twitter;
                setting.Youtube = model.Youtube;
                setting.Adress = model.Adress;
                setting.Analytics = model.Analytics;
                setting.Email = model.Email;
                setting.Facebook = model.Facebook;
                setting.Gplus = model.Gplus;
                setting.Instagram = model.Instagram;
                setting.Keywords = model.SeoKeywords;
                setting.Maps = model.Maps;
                setting.Phone = model.Phone;
                setting.Phone2 = model.Phone2;
                setting.Meta = model.Meta;
                setting.Logo = string.IsNullOrWhiteSpace(model.Logo) ? setting.Logo : model.Logo;
                setting.Favicon = string.IsNullOrWhiteSpace(model.Favicon) ? setting.Favicon : model.Favicon;
            }
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

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Infrastructure; 
using Warehouse.ViewModels.Common;
using Warehouse.ViewModels.WebSite;

namespace Warehouse.Service.WebSite
{
    public class SettingService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly ICacheService _cacheService;
        public SettingService(WarehouseManagementSystemEntities1 context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;   
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
        public SettingViewModel GetSettingViewModel(string languageCode)
        {

            var model = _cacheService.Get("setting", () => (from a in _context.Settings
                                                            where a.Languages.ShortName == languageCode
                                                            select new SettingViewModel()
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
                                                                Logo = a.Logo,
                                                                Favicon = a.Favicon

                                                            }).FirstOrDefault());

            return model;

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
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
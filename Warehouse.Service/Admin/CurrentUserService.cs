using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Infrastructure;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class CurrentUserService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly ICacheService _cacheService;

        public CurrentUserService(WarehouseManagementSystemEntities1 context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;   
        }
        public CurrentUserViewModel GetCurrentUserViewModel(string userName)
        {

            var model = _cacheService.Get("setting", () => (from a in _context.Users
                                                            .Where (u => u.UserName == userName)    
                                                            
                                                            select new CurrentUserViewModel()
                                                            {
                                                                
                                                                Name= a.Name,   
                                                                City = a.Cities.Name,
                                                                Country = a.Countries.Name,
                                                                Id= a.Id,
                                                                Mail= a.Mail,
                                                                Password= a.Password,
                                                                Phone= a.Phone,
                                                                Role= a.Role,
                                                                Surname= a.Surname,
                                                                UserName = a.UserName,
                                                                MessageCount = _context.Contact.Where(x=>x.isShow != true).Count()

                                                            }).FirstOrDefault());

            return model;

        }

        public List<IncomingMessageViewModel> GetIncomingMessageViewModel()
        {
            var model = _cacheService.Get("setting", () => (from a in _context.Contact
                                                           .Where(x=>x.isShow != true)

                                                            select new IncomingMessageViewModel()
                                                            {

                                                                FullName = a.FullName,  
                                                                Id = a.Id,
                                                                Message = a.Message,
                                                                Subject = a.Subject,
                                                                
                                                                

                                                            }).ToList());

            return model;
        }
    }
}

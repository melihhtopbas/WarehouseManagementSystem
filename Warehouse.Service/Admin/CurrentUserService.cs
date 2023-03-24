using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
            var roleList = (from a in _context.Users
                            join ur in _context.UserRoles
                            on a.Id equals ur.UserId
                            join r in _context.Roles
                            on ur.RoleId equals r.Id
                            where a.UserName == userName
                            select new RoleViewModel()
                            {
                                Id = ur.Id,
                                Name = r.Name
                            }



                          );
            
           
            var model = _cacheService.Get("setting", () => (from a in _context.Users
                                                            join ur in _context.UserRoles
                                                            on a.Id equals ur.UserId
                                                            join r in _context.Roles
                                                            on ur.RoleId equals r.Id
                                                            where a.UserName == userName


                                                            select new CurrentUserViewModel()
                                                            {

                                                                Name = a.Name,
                                                                City = a.Cities.Name,
                                                                Country = a.Countries.Name,
                                                                Id = a.Id,
                                                                Mail = a.Mail,
                                                                Password = a.Password,
                                                                Phone = a.Phone,
                                                                Role = a.Role,
                                                                Surname = a.Surname,
                                                                UserName = a.UserName,
                                                                MessageCount = _context.Contact.Where(x => x.isShow != true).Count(),
                                                                Roles = roleList.ToList(),
                                                                

                                                            }).FirstOrDefault());
            if (model!= null && model.Roles.Any(x=>x.Name=="admin"))
            {
                model.IsMainUser = true;
            }

            return model;

        }

        public List<IncomingMessageViewModel> GetIncomingMessageViewModel()
        {

            var model = _cacheService.Get("setting", () => (from a in _context.Contact.AsEnumerable()
                                                           .Where(x => x.isShow != true)

                                                            select new IncomingMessageViewModel()
                                                            {

                                                                FullName = a.FullName,
                                                                Id = a.Id,
                                                                Message = a.Message,
                                                                Subject = a.Subject,
                                                                TimeHour = (int)(DateTime.Now - a.Date).TotalHours,
                                                                TimeDay = (int)(DateTime.Now - a.Date).TotalDays,
                                                                TimeMinute = (int)(DateTime.Now - a.Date).TotalMinutes,







                                                            }).ToList());

            return model;
        }
        public List<TicketMessageViewModel> GetTicketMessageShowViewModel()
        {

            var model = _cacheService.Get("setting", () => (from a in _context.Tickets.AsEnumerable()
                                                           .Where(x => x.isAnswer != true)

                                                            select new TicketMessageViewModel()
                                                            {

                                                                FullName = a.FullName,
                                                                Id = a.Id,
                                                                Message = a.Message,
                                                                Subject = a.Subject,
                                                                TimeHour = (int)(DateTime.Now - a.Date).TotalHours,
                                                                TimeDay = (int)(DateTime.Now - a.Date).TotalDays,
                                                                TimeMinute = (int)(DateTime.Now - a.Date).TotalMinutes,
                                                                TicketId = a.Id,







                                                            }).ToList());

            return model;
        }
        public List<TicketMessageViewModel> GetTicketMessageViewModel(string name)
        {
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
            var model = _cacheService.Get("setting", () => (from a in _context.TicketAnswers.AsEnumerable()
                                                           .Where(x => x.isShow != true && x.UserId == user.Id)

                                                            select new TicketMessageViewModel()
                                                            {

                                                                FullName = a.AnsweringPerson,
                                                                Id = a.Id,
                                                                Message = a.Message,
                                                                Subject = a.Subject,
                                                                TimeHour = (int)(DateTime.Now - a.Date).TotalHours,
                                                                TimeDay = (int)(DateTime.Now - a.Date).TotalDays,
                                                                TimeMinute = (int)(DateTime.Now - a.Date).TotalMinutes,
                                                                TicketId = (long)a.TicketId,







                                                            }).ToList());

            return model;
        }
    }

}

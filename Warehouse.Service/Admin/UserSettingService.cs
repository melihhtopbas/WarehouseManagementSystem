using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Web.UI;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;
using Microsoft.Win32;

namespace Warehouse.Service.Admin
{
    public class UserSettingService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly CurrentUserViewModel _currentUser;
        public UserSettingService(WarehouseManagementSystemEntities1 context)
        {

            _context = context;
            _currentUser = DependencyResolver.Current.GetService<CurrentUserService>().GetCurrentUserViewModel(HttpContext.Current.User.Identity.Name);
        }
        private IQueryable<UserListViewModel> _getUserListIQueryable(Expression<Func<Data.Users, bool>> expr)
        {



            return (from b in _context.Users.AsExpandable().Where(expr)
                    select new UserListViewModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Mail = b.Mail,
                        Password = b.Password,
                        Phone = b.Phone,
                        Surname = b.Surname,
                        City = b.Cities.Name,
                        Country = b.Countries.Name,
                        UserName = b.UserName,
                        CustomerOrderCount = b.Orders.Count()


                    });
        }

        public IQueryable<UserListViewModel> GetUserListIQueryable(UserSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Users>(true);/*AND*/
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                predicate.And(a => a.UserName.Contains(model.Name));

            }
            return _getUserListIQueryable(predicate);
        }


        public async Task<UserListViewModel> GetUserListViewAsync(long userId)
        {

            var predicate = PredicateBuilder.New<Data.Users>(true);/*AND*/
            predicate.And(a => a.Id == userId);
            var user = await _getUserListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return user;
        }

        public async Task<UserEditViewModel> GetUserEditViewModelAsync(int userId)
        {
            var user = await (from p in _context.Users

                              where p.Id == userId
                              select new UserEditViewModel()
                              {
                                  Id = p.Id,
                                  Role = p.Role,
                                  Mail = p.Mail,
                                  Surname = p.Surname,
                                  Name = p.Name,
                                  UserName = p.UserName,
                                  Password = p.Password,
                                  Phone = p.Phone,
                                  City = new OrderCityIdSelectViewModel()
                                  {
                                      CityId = p.CityId
                                  },
                                  Country = new OrderCountryIdSelectViewModel()
                                  {
                                      CountryId = p.CountryId
                                  },
                                  Date = p.Date,




                              }).FirstOrDefaultAsync();

            return user;
        }
        public async Task<ServiceCallResult> EditUserAsync(UserEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.Users.AnyAsync(a => a.Id != model.Id && a.UserName == model.UserName).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu kullanıcı adı kullanılmaktadır.");
                return callResult;
            }

            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (user == null)
            {
                callResult.ErrorMessages.Add("Böyle bir kullanıcı bulunamadı.");
                return callResult;
            }


            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Password = model.Password;
            user.Surname = model.Surname;
            user.Mail = model.Mail;
            user.Phone = model.Phone;
            user.Role = model.Role;
            user.CityId = model.City.CityId;
            user.CountryId = model.Country.CountryId;









            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetUserListViewAsync(user.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> AddUserAsync(UserAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.Users.AnyAsync(a => a.UserName == model.UserName).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu kullanıcı adı kullanılmaktadır.");
                return callResult;
            }

            var user = new Users()
            {

                CityId = model.City.CityId,
                CountryId = model.Country.CountryId,
                Date = DateTime.Now,
                Mail = model.Mail,
                Name = model.Name,
                Password = model.Password,
                Phone = model.Phone, 
                Surname = model.Surname,
                UserName = model.UserName,




            };




            _context.Users.Add(user);
            _context.SaveChanges();

            var roles = _context.Roles.Where(x => x.Name != "admin").ToList();
            foreach (var item in roles)
            {
                _context.UserRoles.Add(new UserRoles
                {
                    Active = true,
                    RoleId = item.Id,
                    UserId = user.Id,
                });
            }
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetUserListViewAsync(user.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<ServiceCallResult> DeleteUserAsync(int userId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == userId).ConfigureAwait(false);
            if (user == null)
            {
                callResult.ErrorMessages.Add("Böyle bir kullanıcı bulunamadı.");
                return callResult;
            }
            var citiesAny = _context.Orders.Any(x => x.Users.Id == userId);
            if (citiesAny)
            {
                callResult.ErrorMessages.Add("Siparişi bulunan kullanıcıyı silemezsiniz.");
                callResult.WarningMessages.Add("Siparişi bulunan kullanıcıyı silemezsiniz.");
                callResult.InfoMessages.Add("Lütfen kullanıcı siparişlerini kontrol ediniz.");

                return callResult;
            }
            var userRoles = _context.UserRoles.Where(x=>x.UserId == userId).ToList();
            foreach (var item in userRoles)
            {
                _context.UserRoles.Remove(item);
            }

            _context.Users.Remove(user);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.SuccessMessages.Add("Kullanıcı başarıyla silindi.");
                    callResult.Success = true;
                    callResult.Item = await GetUserListViewAsync(user.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public List<CountryListViewModel> GetUserCountriesList()
        {

            var result = _context.Countries.Where(x => x.Active == true).Select(b => new CountryListViewModel
            {

                Id = b.Id,
                Name = b.Name


            }).ToList();
            return result;
        }
        public List<CityListViewModel> GetUserCitiesList()
        {

            var result = _context.Cities.Where(x => x.Active == true).Select(b => new CityListViewModel
            {

                Id = b.Id,
                Name = b.Name


            }).ToList();
            return result;
        }
        public List<CityListViewModel> GetUserCityList(long? id)
        {

            var result = _context.Cities.Where(x => x.CountryId == id).Select(b => new CityListViewModel
            {

                Id = b.Id,
                Name = b.Name


            }).ToList();
            return result;
        }
        public List<UserRolesViewModel> GetUserRolesViewModelAsync(int userId)
        {
            var userRoles =  (from p in _context.Users
                                   join ur in _context.UserRoles
                                   on p.Id equals ur.UserId
                                   join r in _context.Roles
                                   on ur.RoleId equals r.Id

                                   where p.Id == userId
                                   select new UserRolesViewModel()
                                   {
                                       Id = ur.Id,
                                       Name = r.Name,
                                       RoleId = r.Id,
                                       UserId = p.Id,
                                       Active = (bool)ur.Active
                                       

                                   }).ToList();

            return userRoles;
        }
        public async Task<ServiceCallResult> EditUserRolesAsync(RoleViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            foreach (var item in model.UserRolesViewModel)
            {
                var userRoles = _context.UserRoles.Where(x => x.Id == item.Id).FirstOrDefault();
                userRoles.Active = item.Active;
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

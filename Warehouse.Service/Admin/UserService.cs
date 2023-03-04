using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class UserService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        string name = System.Web.HttpContext.Current.User.Identity.Name;
        private readonly Users users;
        public UserService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
            users = _context.Users.FirstOrDefault(x => x.UserName == name);
        }
    
        public async Task<UserViewModel> GetUserProfileViewModel()
        {
             
            var model = await (from a in _context.Users
                               where a.UserName == name
                               select new UserViewModel()
                               {
                                   UserName= a.UserName,
                                   Surname=a.Surname,
                                   City = new OrderCityIdSelectViewModel()
                                   {
                                       CityId = a.CityId
                                   },
                                   Country = new OrderCountryIdSelectViewModel()
                                   {
                                       CountryId = a.CountryId  
                                   },
                                   Mail = a.Mail,
                                   Name= a.Name, 
                                   Phone= a.Phone,


                               }).SingleOrDefaultAsync().ConfigureAwait(false);
            
            return model;

        }

        public async Task<ServiceCallResult> AddorEditUser(UserViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == name);
            bool nameExist = await _context.Users.AnyAsync(a => a.Id != model.Id && a.UserName == model.UserName).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Böyle bir kullanıcı adı mevcut!");
                return callResult;
            }
            if (model.Password != user.Password)
            {
                callResult.ErrorMessages.Add("Hatalı şifre girdiniz!");
                model.Password = null; 
                return callResult;
            }
            user.Surname = model.Surname; 
            user.Name = model.Name; 
            user.Phone = model.Phone;   
            user.Mail = model.Mail;
            user.CityId = model.City.CityId;
            user.CountryId = model.Country.CountryId;
            

            

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
        public List<CountryListViewModel> GetUserCountryList()
        {

            var result = _context.Countries.Where(x => x.Active == true).Select(b => new CountryListViewModel
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
        public async Task<ServiceCallResult> ChangePasswordAsync(UserChangePasswordViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            if (model.OldPassword != users.Password)
            {
                callResult.ErrorMessages.Add("Eski şifrenizi hatalı girdiniz!");
                return callResult;
            }

            if (model.NewPassword != model.ReNewPassword)
            {
                callResult.ErrorMessages.Add("Tekrar girilen şifre hatalı!");
                return callResult;
            }

            if (model.OldPassword == model.NewPassword)
            {
                callResult.ErrorMessages.Add("Yeni şifre eski şifreyle aynı olamaz!");
                return callResult;
            }

         

            var userPassword = _context.Users.FirstOrDefault(x => x.UserName == users.UserName);
            userPassword.Password = model.NewPassword;
           



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

        public UserForgotPasswordViewModel GetUserForgottenPassword(string mail)
        {
            var model = new UserForgotPasswordViewModel();
            var user = _context.Users.FirstOrDefault(x=>x.Mail == mail);
            if (user == null)
            {
                model.Message = "Sistemde kayıtlı böyle bir mail adresi bulunamadı!";
            }
            else
            {
                model.Mail = mail;
                model.Password = user.Password;
                model.UserName = user.UserName;

            }
            return model;
        }

    }
}

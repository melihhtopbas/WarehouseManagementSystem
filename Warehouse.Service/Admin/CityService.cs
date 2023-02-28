using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Utils.Helpers;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class CityService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public CityService(WarehouseManagementSystemEntities1 context)
        {
            _context= context;
        }
        private IQueryable<CityListViewModel> _getCityListIQueryable(Expression<Func<Data.Cities, bool>> expr)
        {
            return (from b in _context.Cities.AsExpandable().Where(expr)
                    select new CityListViewModel()
                    {
                        
                        Name = b.Name,
                        CountryName = b.Countries.Name,
                        Id = b.Id, 
                        Active = b.Active,
                         


                    });
        }
        public IQueryable<CityListViewModel> GetCityListIQueryable(CitySearchViewModel citySearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.Cities>(true);/*AND*/
            predicate.And(a => a.LanguageId == citySearchViewModel.LanguageId);
            if (!string.IsNullOrWhiteSpace(citySearchViewModel.Name))
            {
                predicate.And(a => a.Name.Contains(citySearchViewModel.Name));
            }
            if (!string.IsNullOrWhiteSpace(citySearchViewModel.CountryName))
            {
                predicate.And(a => a.Countries.Name.Contains(citySearchViewModel.CountryName));
            }

            return _getCityListIQueryable(predicate);
        }
        public async Task<CityListViewModel> GetCityListViewAsync(long faqId)
        {

            var predicate = PredicateBuilder.New<Data.Cities>(true);/*AND*/
            predicate.And(a => a.Id == faqId);
            var city = await _getCityListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return city;
        }
        public async Task<ServiceCallResult> AddCityAsync(CityAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.Cities.AnyAsync(a => a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde şehir bulunmaktadır.");
                return callResult;
            }

            var city = new Cities()
            {
                Active = model.Active,
                LanguageId= model.LanguageId,
                Name = model.Name,  
                CountryId = model.Country.CountryId,
                

            };
            _context.Cities.Add(city);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetCityListViewAsync(city.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<CityEditViewModel> GetCityEditViewModelAsync(int cityId)
        {
            var city = await (from b in _context.Cities
                             where b.Id == cityId
                             select new CityEditViewModel()
                             {
                                 Name = b.Name,
                                 Id = b.Id,
                                 LanguageId = b.LanguageId,
                                 Country = new OrderCountryIdSelectViewModel()
                                 {
                                     CountryId = b.CountryId.Value
                                 },
                                 
                                 Active  = b.Active,
                                 

                             }).FirstOrDefaultAsync();
            return city;
        }
        public async Task<ServiceCallResult> EditCityAsync(CityEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.Cities.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Şehir bulunmaktadır.");
                return callResult;
            }

            var city = await _context.Cities.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (city == null)
            {
                callResult.ErrorMessages.Add("Böyle bir şehir bulunamadı.");
                return callResult;
            }
            city.Active = model.Active;
            city.CountryId = model.Country.CountryId;
            city.Name = model.Name;
            

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    callResult.Success = true;
                    callResult.Item = await GetCityListViewAsync(city.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteCityAsync(int cityId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var city = await _context.Cities.FirstOrDefaultAsync(a => a.Id == cityId).ConfigureAwait(false);
            if (city== null)
            {
                callResult.ErrorMessages.Add("Böyle bir şehir bulunamadı.");
                return callResult;
            }
            _context.Cities.Remove(city);
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

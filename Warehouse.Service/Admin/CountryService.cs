using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class CountryService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public CountryService(WarehouseManagementSystemEntities1 context)
        {
            _context = context; 
        }
        private IQueryable<CountryListViewModel> _getCountryListIQueryable(Expression<Func<Data.Countries, bool>> expr)
        {
            var model = _context.Countries.ToList();
            foreach (var item in model)
            {
                item.LanguageId = 1; 
            }
            _context.SaveChanges();

            return (from b in _context.Countries.AsExpandable().Where(expr)
                    select new CountryListViewModel()
                    {
                        Id= b.Id,
                        Name = b.Name,
                        LanguageId = b.LanguageId,
                        Active = (bool)b.Active

                    });
        }

        public IQueryable<CountryListViewModel> GetCountryListIQueryable(CountrySearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Name.Contains(model.SearchName));

            }
            return _getCountryListIQueryable(predicate);
        }

        public async Task<CountryListViewModel> GetCountryListViewAsync(long countryId)
        {

            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.Id == countryId);
            var country = await _getCountryListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return country;
        }
        public async Task<ServiceCallResult> AddCountryAsync(CountryViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.Countries.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Ülke bulunmaktadır.");
                return callResult;
            }

            var country = new Countries()
            {

                Name = model.Name,
                LanguageId = model.LanguageId,
                Active = model.Active
                
                
                
            };
             
            


            _context.Countries.Add(country);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetCountryListViewAsync(country.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<CountryViewModel> GetCountryEditViewModelAsync(int countryId)
        {
            var country = await (from p in _context.Countries
                                 where p.Id == countryId
                                 select new CountryViewModel()
                                 {
                                     Name = p.Name,
                                     Id = p.Id,
                                     LanguageId = p.LanguageId,
                                     Active = (bool)p.Active
                                     

                                 }).FirstOrDefaultAsync();
            return country;
        }
        public async Task<ServiceCallResult> EditCountryAsync(CountryViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.Countries.AnyAsync(a => a.Id != model.Id && a.Name == model.Name && a.LanguageId == model.LanguageId).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Ülke bulunmaktadır.");
                return callResult;
            }

            var country = await _context.Countries.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (country == null)
            {
                callResult.ErrorMessages.Add("Böyle bir ülke bulunamadı.");
                return callResult;
            }


            country.Name = model.Name;
            country.Active = model.Active;


           
            

 
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                   

                    callResult.Success = true;
                    callResult.Item = await GetCountryListViewAsync(country.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteCountryAsync(int countryId)
        {
            var callResult = new ServiceCallResult() { Success = false };

            var citiesAny =  _context.Cities.Any(x => x.CountryId == countryId);
            if (citiesAny)
            {
                callResult.ErrorMessages.Add("Şehri bulunan ülkeyi silemezsiniz.");
                return callResult;
            }

            var country = await _context.Countries.FirstOrDefaultAsync(a => a.Id == countryId).ConfigureAwait(false);
            if (country == null)
            {
                callResult.ErrorMessages.Add("Böyle bir ülke bulunamadı.");
                return callResult;
            }


            
            _context.Countries.Remove(country);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                     

                    callResult.Success = true;
                    callResult.Item = await GetCountryListViewAsync(country.Id).ConfigureAwait(false);
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

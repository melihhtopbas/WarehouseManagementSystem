using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class CurrencyUnitService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public CurrencyUnitService(WarehouseManagementSystemEntities1 context)
        {
            _context = context; 
        }
        private IQueryable<CurrencyUnitListViewModel> _getCurrencyUnitListIQueryable(Expression<Func<Data.CurrencyUnits, bool>> expr)
        {


            return (from b in _context.CurrencyUnits.AsExpandable().Where(expr)
                    select new CurrencyUnitListViewModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        LanguageId = b.LanguageId,
                        Description = b.Description,
                        Icon = b.Icon
                        


                    });
        }

        public IQueryable<CurrencyUnitListViewModel> GetCurrencyUnitListIQueryable(CurrencyUnitSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.CurrencyUnits>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Description.Contains(model.SearchName));

            }
            return _getCurrencyUnitListIQueryable(predicate);
        }


        public async Task<CurrencyUnitListViewModel> GetCurrencyUnitListViewAsync(long currencyId)
        {

            var predicate = PredicateBuilder.New<Data.CurrencyUnits>(true);/*AND*/
            predicate.And(a => a.Id == currencyId);
            var currencyUnit = await _getCurrencyUnitListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return currencyUnit;
        }
        public async Task<ServiceCallResult> AddCurrencyUnitAsync(CurrencyUnitViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            if (model.Name.Length > 5 )
            {
                callResult.ErrorMessages.Add("Para birimi 5 karakter uzunluğundan büyük olamaz.");
                return callResult;
            }

            bool nameExist = await _context.CurrencyUnits.AnyAsync(a => a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde para birimi bulunmaktadır.");
                return callResult;
            }

            var currencyUnit = new CurrencyUnits()
            {

                Name = model.Name,
                LanguageId = model.LanguageId,
                Description = model.Description,
                Icon = model.Icon,



            };




            _context.CurrencyUnits.Add(currencyUnit);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetCurrencyUnitListViewAsync(currencyUnit.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<CurrencyUnitViewModel> GetCurrencyUnitEditViewModelAsync(int currencyUnitId)
        {
            var currencyUnit = await (from p in _context.CurrencyUnits
                                      where p.Id == currencyUnitId
                                      select new CurrencyUnitViewModel()
                                      {
                                          Name = p.Name,
                                          Id = p.Id,
                                          LanguageId = p.LanguageId,
                                          Description = p.Description,
                                          Icon = p.Icon,


                                      }).FirstOrDefaultAsync();
            return currencyUnit;
        }
        public async Task<ServiceCallResult> EditCurrencyUnitAsync(CurrencyUnitViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            if (model.Name.Length > 5)
            {
                callResult.ErrorMessages.Add("Para birimi 5 karakter uzunluğundan büyük olamaz.");
                return callResult;
            }
            bool nameExist = await _context.CurrencyUnits.AnyAsync(a => a.Id != model.Id && a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde para birimi bulunmaktadır.");
                return callResult;
            }

            var currencyUnit = await _context.CurrencyUnits.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (currencyUnit == null)
            {
                callResult.ErrorMessages.Add("Böyle bir para birimi bulunamadı.");
                return callResult;
            }


            currencyUnit.Name = model.Name;
            currencyUnit.Description = model.Description;
            currencyUnit.Icon = model.Icon;






            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetCurrencyUnitListViewAsync(currencyUnit.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteCurrencyUnitAsync(int currencyUnitId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var currencyUnit = await _context.CurrencyUnits.FirstOrDefaultAsync(a => a.Id == currencyUnitId).ConfigureAwait(false);
            if (currencyUnit == null)
            {
                callResult.ErrorMessages.Add("Böyle bir para birimi bulunamadı.");
                return callResult;
            }
            bool country = _context.Countries.Any(x => x.CurrencyUnitId == currencyUnitId);
            if (country)
            {
                callResult.ErrorMessages.Add("Aktif olarak kullanılan para birimini silemezsiniz.");
                return callResult;
            }
           
           



            _context.CurrencyUnits.Remove(currencyUnit);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetCurrencyUnitListViewAsync(currencyUnit.Id).ConfigureAwait(false);
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

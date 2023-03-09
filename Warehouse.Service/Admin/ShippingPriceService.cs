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
    public class ShippingPriceService
    {

        private readonly WarehouseManagementSystemEntities1 _context;

        public ShippingPriceService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }

        private IQueryable<ShippingPriceListViewModel> _getShippingPriceListIQueryable(Expression<Func<Data.Countries, bool>> expr)
        {
         

            return (from b in _context.Countries.AsExpandable().Where(expr)
                    select new ShippingPriceListViewModel()
                    {
                        Id = b.Id,
                        CountryName = b.Name,
                        LanguageId = b.LanguageId, 
                        CurrencyUnitName = b.CurrencyUnits.Name

                    });
        }

        public IQueryable<ShippingPriceListViewModel> GetShippingPriceListIQueryable(ShippingPriceSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Name.Contains(model.SearchName));

            }
            return _getShippingPriceListIQueryable(predicate);
        }
        public List<CountryListViewModel> GetCountryList()
        {

            var result = _context.Countries.Select(b => new CountryListViewModel
            {

                Id = b.Id,
                Name = b.Name,
                Active = (bool)b.Active,
                LanguageId = b.LanguageId,
                CurrencyUnitName = b.CurrencyUnits.Name


            }).ToList();
            return result;
        }

        public async Task<ShippingPriceListViewModel> GetShippingPriceListViewAsync(long shipPriceId)
        {

            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.Id == shipPriceId);
            var country = await _getShippingPriceListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return country;
        }

        public  List<CountryShippingPriceViewModel> GetShippingPriceEditViewModelAsync(int shipPriceId)
        {
            var currencyUnitName = _context.Countries.Where(x => x.Id == shipPriceId).FirstOrDefault();
            var shippingPrice =  (from p in _context.ShippingPrices
                                       where p.CountryId == shipPriceId    
                               
                               
                               select new CountryShippingPriceViewModel()
                               {
                                   Active = p.Active, 
                                   CargoServiceName = p.CargoServiceTypes.Name,
                                   DeliveryTime = p.DeliveryTime,
                                   Price = p.Price,
                                   Id = p.Id,
                                   CountryId = shipPriceId,
                                   CurrencyUnitName = currencyUnitName.CurrencyUnits.Icon
                                   
                                   
                                   


                               }).ToList();
            return shippingPrice;
        }
        public ShippingPriceViewModel ShippingPriceViewModel(int shipPriceId)
        {
            var shippingPrice = (from p in _context.Countries
                                 where p.Id == shipPriceId


                                 select new ShippingPriceViewModel()
                                 {
                                     
                                     Id = shipPriceId,
                                   
                                     CountryName = p.Name,
                                     LanguageId = p.LanguageId
                                     




                                 }).FirstOrDefault();
            return shippingPrice;
        }
        public async Task<ServiceCallResult> EditShippingPriceAsync(ShippingPriceViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            var shippingPrice = _context.ShippingPrices.Where(x => x.CountryId == model.Id).ToList();

         

            for (int i = 0; i < model.CountryShippingPriceViewModels.Count(); i++)
            {
                shippingPrice[i].Active = model.CountryShippingPriceViewModels[i].Active;
                shippingPrice[i].Price = model.CountryShippingPriceViewModels[i].Price;
                shippingPrice[i].DeliveryTime = model.CountryShippingPriceViewModels[i].DeliveryTime;
            }
        
            
             
           

          
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetShippingPriceListViewAsync(model.Id).ConfigureAwait(false);
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

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
    public class CargoServiceTypeService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public CargoServiceTypeService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<CargoServiceTypeListViewModel> _getCargoServiceListIQueryable(Expression<Func<Data.CargoServiceTypes, bool>> expr)
        {


            return (from b in _context.CargoServiceTypes.AsExpandable().Where(expr)
                    select new CargoServiceTypeListViewModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        LanguageId = b.LanguageId,


                    });
        }

        public IQueryable<CargoServiceTypeListViewModel> GetCargoServiceListIQueryable(CargoServiceSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.CargoServiceTypes>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Name.Contains(model.SearchName));

            }
            return _getCargoServiceListIQueryable(predicate);
        }


        public async Task<CargoServiceTypeListViewModel> GetCargoServiceListViewAsync(long cargoId)
        {

            var predicate = PredicateBuilder.New<Data.CargoServiceTypes>(true);/*AND*/
            predicate.And(a => a.Id == cargoId);
            var cargoService = await _getCargoServiceListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return cargoService;
        }
        public async Task<ServiceCallResult> AddCargoServiceAsync(CargoServiceTypeViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.CargoServiceTypes.AnyAsync(a => a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde kargo servisi bulunmaktadır.");
                return callResult;
            }

            var cargoService = new CargoServiceTypes()
            {

                Name = model.Name,
                LanguageId = model.LanguageId,



            };

            var country = _context.Countries.ToList();

            foreach (var item in country)
            {
                _context.ShippingPrices.Add(new ShippingPrices
                {
                    Active = false,
                    CargoServiceId = cargoService.Id,
                    CountryId = item.Id,
                    LanguageId = cargoService.LanguageId,

                });
            }



            _context.CargoServiceTypes.Add(cargoService);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetCargoServiceListViewAsync(cargoService.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<CargoServiceTypeViewModel> GetCargoServiceEditViewModelAsync(int cargoServiceId)
        {
            var cargoService = await (from p in _context.CargoServiceTypes
                                      where p.Id == cargoServiceId
                                      select new CargoServiceTypeViewModel()
                                      {
                                          Name = p.Name,
                                          Id = p.Id,
                                          LanguageId = p.LanguageId,


                                      }).FirstOrDefaultAsync();
            return cargoService;
        }
        public async Task<ServiceCallResult> EditCargoServiceAsync(CargoServiceTypeViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.CargoServiceTypes.AnyAsync(a => a.Id != model.Id && a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde kargo servisi bulunmaktadır.");
                return callResult;
            }

            var cargoService = await _context.CargoServiceTypes.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (cargoService == null)
            {
                callResult.ErrorMessages.Add("Böyle bir kargo servisi bulunamadı.");
                return callResult;
            }


            cargoService.Name = model.Name;






            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetCargoServiceListViewAsync(cargoService.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteCargoServiceAsync(int cargoServiceId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var cargoService = await _context.CargoServiceTypes.FirstOrDefaultAsync(a => a.Id == cargoServiceId).ConfigureAwait(false);
            if (cargoService == null)
            {
                callResult.ErrorMessages.Add("Böyle bir kargo servisi bulunamadı.");
                return callResult;
            }
            var shippingCargo = _context.ShippingPrices.Where(x => x.CargoServiceId == cargoServiceId && x.Active == true).ToList();


            var shippingService = _context.ShippingPrices.Where(x => x.CargoServiceId == cargoServiceId).ToList();
            foreach (var item in shippingService)
            {
                _context.ShippingPrices.Remove(item);
            }




            _context.CargoServiceTypes.Remove(cargoService);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetCargoServiceListViewAsync(cargoService.Id).ConfigureAwait(false);
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

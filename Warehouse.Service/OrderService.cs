using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using LinqKit;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service
{
    public class OrderService
    {

        private readonly WarehouseManagementSystemEntities _context;

        public OrderService(WarehouseManagementSystemEntities context)
        {
            _context = context;
        }
        public List<OrderListViewModel> GetOrderList()
        {
            var result = _context.Orders.Select(b => new OrderListViewModel
            {

                Id = b.Id,
                PackageCount = b.PackageCount,
                RecipientAddress = b.RecipientAddress,
                RecipientCity = b.RecipientCity,
                RecipientName = b.RecipientName,
                RecipientPhone = b.RecipientPhone,
                RecipientZipCode = b.RecipientZipCode,
                SenderName = b.SenderName,
                SenderPhone = b.SenderPhone,
                RecipientCountry = b.Countries.Name,
                CurrencyUnit = b.CurrencyUnits.Name,
                CargoService = b.CargoServiceTypes.Name





            }).ToList();
            return result;
        }
        public List<CountryListViewModel> GetOrderCountryList()
        {

            var result = _context.Countries.Select(b => new CountryListViewModel
            {

                Id = b.Id,
                Name = b.Name


            }).ToList();
            return result;
        }
        public List<CurrencyUnitListViewModel> GetOrderCurrencyUnitList()
        {

            var result = _context.CurrencyUnits.Select(b => new CurrencyUnitListViewModel
            {

                Id = b.Id,
                Name = b.Name,

            }).ToList();
            return result;
        }
        public List<CargoServiceTypeListViewModel> GetOrderCargoServiceTypeList()
        {

            var result = _context.CargoServiceTypes.Select(b => new CargoServiceTypeListViewModel
            {

                Id =  b.Id,
                Name = b.Name


            }).ToList();
            return result;
        }






        public async Task<ServiceCallResult> AddOrderAsync(OrderAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            var order = new Orders
            {
                Id = model.Id,
                SenderIdentityNumber = model.SenderIdentityNumber,
                SenderMail = model.SenderMail,
                SenderName = model.SenderName,
                SenderPhone = model.SenderPhone,
                RecipientCity = model.RecipientCity,
                RecipientMail = model.RecipientMail,
                RecipientName = model.RecipientName,
                RecipientAddress = model.RecipientAddress,
                RecipientPhone = model.RecipientPhone,
                PackageCount = model.PackageCount,
                PackageHeight = model.PackageHeight,
                PackageLength = model.PackageLength,
                PackageWeight = model.PackageWeight,
                PackageWidth = model.PackageWidth,
                ProductOrderDescription = model.OrderDescription,
                RecipientIdentityNumber = model.RecipientIdentityNumber,
                RecipientZipCode = model.RecipientZipCode,
                CargoServiceTypeId = model.CargoService.CargoServiceId,
                ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId,
                RecipientCountryId = model.Country.CountryId
                
                
                
                
                
                



            };
            foreach (var productGroup in model.ProductTransactionGroup)
            {


                order.ProductTransactionGroup.Add(new ProductTransactionGroup()
                {
                    Content = productGroup.Content,
                    Count = productGroup.Count,
                    SKU = productGroup.SKU,
                    GtipCode = productGroup.GtipCode,
                    Id = productGroup.Id,
                    OrderId = productGroup.OrderId,
                    QuantityPerUnit = productGroup.QuantityPerUnit
                });
            }



            _context.Orders.Add(order);
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

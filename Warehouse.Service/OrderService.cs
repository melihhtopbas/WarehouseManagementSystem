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
                CargoServiceTypeId = b.CargoServiceTypeId,
                Id = b.Id,
                PackageCount = b.PackageCount,
                RecipientAddress = b.RecipientAddress,
                RecipientCity = b.RecipientCity,
                RecipientName = b.RecipientName,
                RecipientPhone = b.RecipientPhone,
                RecipientZipCode = b.RecipientZipCode,
                SenderName = b.SenderName,
                SenderPhone = b.SenderPhone,
                RecipientCountryId = b.RecipientCountryId,
                OrderDescription = b.ProductOrderDescription,
                RecipientCountry = b.Countries.Name,
                CurrencyUnit = b.CurrencyUnits.Name
                  


            }).ToList();
            return result;
        }
        public List<CountryListViewModel> GetOrderCountryList()
        {

            var result = _context.Orders.Select(b => new CountryListViewModel
            {
      
                Id = b.Countries.Id,
                Name = b.Countries.Name


            }).ToList();
            return result;
        }
        public List<CurrencyUnitListViewModel> GetOrderCurrencyUnitList()
        {

            var result = _context.Orders.Select(b => new CurrencyUnitListViewModel
            {

               Id = b.CurrencyUnits.Id,
               Name = b.CurrencyUnits.Name,

            }).ToList();
            return result;
        }
        public List<CargoServiceTypeListViewModel> GetOrderCargoServiceTypeList()
        {

            var result = _context.Orders.Select(b => new CargoServiceTypeListViewModel
            {

                Id = b.CargoServiceTypes.Id,
                Name = b.CargoServiceTypes.Name
                

            }).ToList();
            return result;
        }






        public async Task<ServiceCallResult> AddOrderAsync(OrderAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            

            var order = new Orders()
            {
                SenderPhone = model.SenderPhone,
                SenderIdentityNumber = model.SenderIdentityNumber,
                SenderName = model.SenderName,
                SenderMail = model.SenderMail,
                RecipientCountryId = model.RecipientCountryId,
                CargoServiceTypeId = model.CargoServiceTypeId,
                Id = model.Id,
                PackageCount = model.PackageCount,
                PackageHeight = model.PackageHeight,
                PackageLength = model.PackageLength,
                ProductCurrencyUnitId = model.ProductCurrencyUnitId,
                PackageWeight = model.PackageWeight,
                PackageWidth = model.PackageWidth,
                ProductOrderDescription = model.ProductOrderDescription,
                RecipientAddress = model.RecipientAddress,
                RecipientCity = model.RecipientCity,
                RecipientIdentityNumber = model.RecipientIdentityNumber,
                RecipientName = model.RecipientName,
                RecipientMail = model.RecipientMail,
                RecipientPhone = model.RecipientPhone,
                RecipientZipCode = model.RecipientPhone,

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

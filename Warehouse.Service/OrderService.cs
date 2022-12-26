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
using System.Data.Entity;

namespace Warehouse.Service
{
    public class OrderService
    {

        private readonly WarehouseManagementSystemEntities _context;

        public OrderService(WarehouseManagementSystemEntities context)
        {
            _context = context;
        }
        public IQueryable<OrderListViewModel> GetOrderList()
        {
            return (from b in _context.Orders/*.Where(expr)*/
                    select new OrderListViewModel()
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

                    });
        }
        private IQueryable<OrderListViewModel> _getServiceListIQueryable(Expression<Func<Data.Orders, bool>> expr)
        {
            return (from b in _context.Orders.AsExpandable().Where(expr)
                    select new OrderListViewModel()
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

                    });
        }
        public IQueryable<OrderListViewModel> GetServiceListIQueryable(OrderSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.SenderName.Contains(model.SearchName));

            }
            return _getServiceListIQueryable(predicate);
        }
        public async Task<OrderListViewModel> GetOrderListViewAsync(long orderId)
        {

            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.Id == orderId);
            var service = await _getServiceListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return service;
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

                Id = b.Id,
                Name = b.Name


            }).ToList();
            return result;
        }
        public async Task<List<LanguageListModel>> GetLanguageListViewAsync()
        {

            var result = _context.Languages.Select(x => new LanguageListModel
            {
                Id = x.Id,
                Name = x.Name,

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
                RecipientCountryId = model.Country.CountryId,
                LanguageId = 1









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
        public async Task<OrderEditViewModel> GetOrderEditViewModelAsync(int orderId)
        {
            var order = await (from p in _context.Orders
                                 where p.Id == orderId
                                 select new OrderEditViewModel()
                                 {
                                     
                                     Id = p.Id,
                                     SenderName = p.SenderName,
                                     SenderIdentityNumber = p.SenderIdentityNumber,
                                     SenderMail = p.SenderMail,
                                     SenderPhone = p.SenderPhone,
                                     RecipientAddress = p.RecipientAddress,
                                     RecipientCity = p.RecipientCity,
                                     RecipientIdentityNumber = p.RecipientIdentityNumber,
                                     RecipientMail = p.RecipientMail,
                                     RecipientName = p.RecipientName,
                                     RecipientPhone = p.RecipientPhone,
                                     RecipientZipCode = p.RecipientZipCode,
                                     CargoService = new OrderCargoServiceTypeIdSelectViewModel()
                                     {
                                         CargoServiceId = p.CargoServiceTypeId
                                     },
                                     Country = new OrderCountryIdSelectViewModel()
                                     {
                                         CountryId = p.RecipientCountryId
                                     },
                                     CurrenyUnit = new OrderCurrencyUnitIdSelectViewModel()
                                     {
                                         CurrencyUnitId = p.ProductCurrencyUnitId
                                     },
                                     PackageCount = p.PackageCount,
                                     PackageHeight = p.PackageHeight,
                                     PackageLength = p.PackageLength,
                                     PackageWeight = p.PackageWeight,
                                     PackageWidth = p.PackageWidth,
                                     OrderDescription = p.ProductOrderDescription,
                                     ProductTransactionGroup = from i in p.ProductTransactionGroup
                                                               select new ProductTransactionGroupViewModel
                                                               {
                                                                   Content = i.Content,
                                                                   Count = i.Count,
                                                                   QuantityPerUnit = i.QuantityPerUnit,
                                                                   GtipCode = i.GtipCode,
                                                                   SKU = i.SKU
                                                                   
                                                               },
 

                                 }).FirstOrDefaultAsync();
            return order;
        }
        public async Task<ServiceCallResult> EditOrderAsync(OrderEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            //foreach (var item in model.ProductTransactionGroup)
            //{
            //    bool nameExist = await _context.ProductTransactionGroup.AnyAsync(a => a.Id != item.Id && a.SKU == item.SKU).ConfigureAwait(false);
            //    if (nameExist)
            //    {
            //        callResult.ErrorMessages.Add("Bu stok kodu kullanılmaktadır!");
            //        return callResult;
            //    }
            //}
            


            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (order == null)
            {
                callResult.ErrorMessages.Add("Böyle bir sipariş bulunamadı.");
                return callResult;
            }

            order.SenderName = model.SenderName;
            order.SenderIdentityNumber = model.SenderIdentityNumber;
            order.SenderPhone = model.SenderPhone;
            order.SenderMail = model.SenderMail;
            order.RecipientAddress = model.RecipientAddress;
            order.RecipientPhone = model.RecipientPhone;
            order.RecipientCity = model.RecipientCity;
            order.RecipientIdentityNumber = model.RecipientIdentityNumber;
            order.RecipientMail = model.RecipientMail;
            order.RecipientZipCode = model.RecipientZipCode;
            order.RecipientName = model.RecipientName;
            order.RecipientCountryId = model.Country.CountryId;
            order.ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId;
            order.CargoServiceTypeId = model.CargoService.CargoServiceId;
            order.PackageLength = model.PackageLength;
            order.PackageHeight = model.PackageHeight;
            order.PackageWeight = model.PackageWeight;
            order.PackageWidth = model.PackageWidth;
            order.ProductOrderDescription = model.OrderDescription;
            order.PackageCount = model.PackageCount;
             
            

 
            var deletedProductGroupsList = new List<string>();
            foreach (var groupDb in order.ProductTransactionGroup.ToArray()
                .Where(groupDb => model.ProductTransactionGroup.All(x => x.SKU != groupDb.SKU)))
            {
                deletedProductGroupsList.Add(groupDb.SKU);
                order.ProductTransactionGroup.Remove(groupDb);
                _context.ProductTransactionGroup.Remove(groupDb);
            }




            byte imageOrder = 0;
            foreach (var groupViewModel in model.ProductTransactionGroup)
            {
                var skuExist = order.ProductTransactionGroup.Any(a => a.SKU == groupViewModel.SKU);
                if (!skuExist)
                {

                    order.ProductTransactionGroup.Add(new ProductTransactionGroup()
                    {
                        SKU = groupViewModel.SKU,
                        GtipCode = groupViewModel.GtipCode,
                        Count = groupViewModel.Count,
                        Content = groupViewModel.Content,
                        QuantityPerUnit = groupViewModel.QuantityPerUnit
                    });


                    imageOrder++;
                }
                

            }

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    if (deletedProductGroupsList.Any())
                    {


                         

                    }

                    callResult.Success = true;
                    callResult.Item = await GetOrderListViewAsync(order.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteOrderAsync(int orderId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == orderId).ConfigureAwait(false);
            if (order == null)
            {
                callResult.ErrorMessages.Add("Böyle bir servis bulunamadı.");
                return callResult;
            }


            var deletedProductGroupsList = new List<string>();
            foreach (var groupDb in order.ProductTransactionGroup.ToList())
            {
                deletedProductGroupsList.Add(groupDb.SKU);
                order.ProductTransactionGroup.Remove(groupDb);
                _context.ProductTransactionGroup.Remove(groupDb);
            }


            _context.Orders.Remove(order);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    

                    callResult.Success = true;
                    callResult.Item = await GetOrderListViewAsync(order.Id).ConfigureAwait(false);
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
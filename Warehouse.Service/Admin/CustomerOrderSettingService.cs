﻿using LinqKit;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;
using System.Web.Mvc;
using System.Data.Entity;

namespace Warehouse.Service.Admin
{
    public class CustomerOrderSettingService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public CustomerOrderSettingService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }

        public UserListViewModel Detail(long customerId)
        {
            var customer = _context.Users.FirstOrDefault(x=>x.Id == customerId);
            var model = new UserListViewModel
            {
                Name = customer.Name,
            };
            return model;
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
        public async Task<List<UserListViewModel>> GetCustomerListViewAsync(long customerId)
        {

            var result = _context.Users.Where(x=>x.Id == customerId).Select(x => new UserListViewModel
            {
                Id = x.Id,
                UserName = x.UserName,
                
            }).ToList();
            return result;
        }

        private IQueryable<CustomerOrderListViewModel> _getOrderListIQueryable(Expression<Func<Data.Orders, bool>> expr)
        {

            return (from b in _context.Orders.AsExpandable().Where(expr)
                    join sAd in _context.SenderAddresses
                    on b.Id equals sAd.OrderId
                    join rAd in _context.RecipientAddresses
                    on b.Id equals rAd.OrderId 
                    select new CustomerOrderListViewModel()
                    {
                        Id = b.Id,
                        PackageCount = _context.Packages.Where(x => x.OrderId == b.Id).Count(),
                        RecipientAddress = rAd.Name,
                        SenderAddress = sAd.Name,
                        RecipientCity = b.Cities.Name,
                        RecipientName = b.RecipientName,
                        RecipientPhone = b.RecipientPhone,
                        RecipientZipCode = b.RecipientZipCode,
                        SenderName = b.SenderName,
                        SenderPhone = b.SenderPhone,
                        RecipientCountry = b.Countries.Name,
                        CurrencyUnit = b.CurrencyUnits.Name,
                        CargoService = b.CargoServiceTypes.Name,
                        isPackage = b.isPackage,
                        DateTime = (DateTime)b.Date, 






                    });

        }
        public IQueryable<CustomerOrderListViewModel> GetOrderListIQueryable(CustomerOrderSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId && a.CustomerId == model.CustomerId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.SenderName.Contains(model.SearchName) || a.SenderPhone.Contains(model.SearchName));

            }
            return _getOrderListIQueryable(predicate);
        }
        public async Task<CustomerOrderListViewModel> GetOrderListViewAsync(long orderId)
        {

            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.Id == orderId);
            var order = await _getOrderListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return order;
        }
        public async Task<ServiceCallResult> AddOrderAsync(CustomerOrderAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            //eklenen her gruptaki skuyu tek tek kontrol eder. Bunlardan birisi bile VERİTABANINDA var ise hata verir.
            foreach (var item in model.ProductTransactionGroup)
            {
                bool skuExist = await _context.ProductTransactionGroup.AnyAsync(a => a.SKU == item.SKU).ConfigureAwait(false);
                if (skuExist)
                {
                    callResult.ErrorMessages.Add("Bu stok kodu kullanılmaktadır! " + "{" + item.SKU + "}");

                    return callResult;
                }
            }
            if (model.ProductTransactionGroup.Count() > 1)
            {
                for (int i = 0; i <= model.ProductTransactionGroup.Count() - 1; i++)
                {
                    for (int j = i + 1; j <= model.ProductTransactionGroup.Count() - 1; j++)
                    {
                        bool skuexist = model.ProductTransactionGroup.ElementAt(i).SKU.Equals(model.ProductTransactionGroup.ElementAt(j).SKU);
                        if (skuexist)
                        {
                            callResult.ErrorMessages.Add("Grup içinde benzersiz stok kodu olmalıdır! " + "{" + model.ProductTransactionGroup.ElementAt(i).SKU + "}");
                            return callResult;


                        }
                    }
                }
            }


            var order = new Orders();

            if (model.City == null)
            {

                order.Id = model.Id;
                order.SenderIdentityNumber = model.SenderIdentityNumber;
                order.SenderMail = model.SenderMail;
                order.SenderName = model.SenderName;
                order.SenderPhone = model.SenderPhone;
                order.SenderInvoiceNumber = model.SenderInvoiceNumber;
                order.RecipientMail = model.RecipientMail;
                order.RecipientName = model.RecipientName;
                order.RecipientPhone = model.RecipientPhone;
                order.RecipientInvoiceNumber = model.RecipientInvoiceNumber;
                order.ProductOrderDescription = model.OrderDescription;
                order.RecipientIdentityNumber = model.RecipientIdentityNumber;
                order.RecipientZipCode = model.RecipientZipCode;
                order.CargoServiceTypeId = model.CargoService.CargoServiceId;
                order.ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId;
                order.RecipientCountryId = model.Country.CountryId;
                order.LanguageId = 1; //türkçe dili olarak ayarlanır.
                order.Date = DateTime.Now.Date;
                order.CustomerId = model.CustomerId;




            }
            if (model.City != null)
            {

                order.Id = model.Id;
                order.SenderIdentityNumber = model.SenderIdentityNumber;
                order.SenderMail = model.SenderMail;
                order.SenderName = model.SenderName;
                order.SenderPhone = model.SenderPhone;
                order.SenderInvoiceNumber = model.SenderInvoiceNumber;
                order.RecipientCityId = model.City.CityId;
                order.RecipientMail = model.RecipientMail;
                order.RecipientName = model.RecipientName;
                order.RecipientPhone = model.RecipientPhone;
                order.RecipientInvoiceNumber = model.RecipientInvoiceNumber;
                order.ProductOrderDescription = model.OrderDescription;
                order.RecipientIdentityNumber = model.RecipientIdentityNumber;
                order.RecipientZipCode = model.RecipientZipCode;
                order.CargoServiceTypeId = model.CargoService.CargoServiceId;
                order.ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId;
                order.RecipientCountryId = model.Country.CountryId;
                order.LanguageId = 1; //türkçe dili olarak ayarlanır.
                order.Date = DateTime.Now.Date;
                order.CustomerId = model.CustomerId;




            }


            var sAddress = new SenderAddresses
            {
                OrderId = order.Id,
                Name = model.SenderAddress
            };
            var rAddress = new RecipientAddresses
            {
                OrderId = order.Id,
                Name = model.RecipientAddress
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
                    QuantityPerUnit = productGroup.QuantityPerUnit,
                    isPackagedCount = productGroup.Count
                });
            }

            _context.Orders.Add(order);
            _context.SenderAddresses.Add(sAddress);
            _context.RecipientAddresses.Add(rAddress);
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
        public async Task<ServiceCallResult> EditOrderAsync(CustomerOrderEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            foreach (var item in model.ProductTransactionGroup)
            {
                bool nameExist = await _context.ProductTransactionGroup.AnyAsync(a => a.Id != item.Id && a.SKU == item.SKU).ConfigureAwait(false);
                if (nameExist)
                {
                    callResult.ErrorMessages.Add("Bu stok kodu kullanılmaktadır!");
                    return callResult;
                }
            }

            if (model.ProductTransactionGroup.Count() > 1)
            {
                for (int i = 0; i <= model.ProductTransactionGroup.Count() - 1; i++)
                {
                    for (int j = i + 1; j <= model.ProductTransactionGroup.Count() - 1; j++)
                    {
                        bool skuexist = model.ProductTransactionGroup.ElementAt(i).SKU.Equals(model.ProductTransactionGroup.ElementAt(j).SKU);
                        if (skuexist)
                        {
                            callResult.ErrorMessages.Add("Grup içinde benzersiz stok kodu olmalıdır!");
                            return callResult;


                        }
                    }
                }
            }

            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            var senderAddress = await _context.SenderAddresses.FirstOrDefaultAsync(a => a.OrderId == model.Id).ConfigureAwait(false);
            var recipientAddress = await _context.RecipientAddresses.FirstOrDefaultAsync(a => a.OrderId == model.Id).ConfigureAwait(false);
            var productGroup = await _context.ProductTransactionGroup.FirstOrDefaultAsync(a => a.OrderId == model.Id).ConfigureAwait(false);
            if (order == null)
            {
                callResult.ErrorMessages.Add("Böyle bir sipariş bulunamadı.");
                return callResult;
            }
            if (model.City == null)
            {
                order.SenderName = model.SenderName;
                order.SenderIdentityNumber = model.SenderIdentityNumber;
                order.SenderPhone = model.SenderPhone;
                order.SenderMail = model.SenderMail;
                order.SenderInvoiceNumber = model.SenderInvoiceNumber;
                order.RecipientInvoiceNumber = model.RecipientInvoiceNumber;
                order.RecipientPhone = model.RecipientPhone;
                order.RecipientIdentityNumber = model.RecipientIdentityNumber;
                order.RecipientMail = model.RecipientMail;
                order.RecipientZipCode = model.RecipientZipCode;
                order.RecipientName = model.RecipientName;
                order.RecipientCountryId = model.Country.CountryId;
                order.ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId;
                order.CargoServiceTypeId = model.CargoService.CargoServiceId;
                order.ProductOrderDescription = model.OrderDescription;
                order.RecipientCityId = null;
                recipientAddress.Name = model.RecipientAddress;
                senderAddress.Name = model.SenderAddress;
            }
            else
            {
                order.SenderName = model.SenderName;
                order.SenderIdentityNumber = model.SenderIdentityNumber;
                order.SenderPhone = model.SenderPhone;
                order.SenderMail = model.SenderMail;
                order.SenderInvoiceNumber = model.SenderInvoiceNumber;
                order.RecipientInvoiceNumber = model.RecipientInvoiceNumber;
                order.RecipientPhone = model.RecipientPhone;
                order.RecipientCityId = model.City.CityId;
                order.RecipientIdentityNumber = model.RecipientIdentityNumber;
                order.RecipientMail = model.RecipientMail;
                order.RecipientZipCode = model.RecipientZipCode;
                order.RecipientName = model.RecipientName;
                order.RecipientCountryId = model.Country.CountryId;
                order.ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId;
                order.CargoServiceTypeId = model.CargoService.CargoServiceId;
                order.ProductOrderDescription = model.OrderDescription;
                recipientAddress.Name = model.RecipientAddress;
                senderAddress.Name = model.SenderAddress;
            }


            foreach (var prd in model.ProductTransactionGroup)
            {
                productGroup.isPackagedCount = prd.isPackagedCount;
                productGroup.isPackage = prd.isPackage;
                productGroup.isReadOnly = prd.isReadOnly;
            }








            foreach (var groupDb in order.ProductTransactionGroup.ToArray()
                .Where(groupDb => model.ProductTransactionGroup.All(x => x.SKU != groupDb.SKU)))
            {

                order.ProductTransactionGroup.Remove(groupDb);
                _context.ProductTransactionGroup.Remove(groupDb);
            }
            // bu işlem değişim var ise önceki stok koduna ait grubu siliyor.



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
                        QuantityPerUnit = groupViewModel.QuantityPerUnit,
                        isPackagedCount = productGroup.isPackagedCount,
                        isPackage = productGroup.isPackage,
                        Id = groupViewModel.Id,
                        isReadOnly = productGroup.isReadOnly,

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
        public async Task<CustomerOrderEditViewModel> GetOrderEditViewModelAsync(int orderId)
        {

            var order = await (from p in _context.Orders
                               where p.Id == orderId
                               join sAd in _context.SenderAddresses
                               on p.Id equals sAd.OrderId
                               join rAd in _context.RecipientAddresses
                               on p.Id equals rAd.OrderId
                               select new CustomerOrderEditViewModel()
                               {

                                   Id = p.Id,
                                   CustomerId = p.CustomerId,
                                   SenderName = p.SenderName,
                                   SenderIdentityNumber = p.SenderIdentityNumber,
                                   SenderMail = p.SenderMail,
                                   SenderPhone = p.SenderPhone,
                                   SenderInvoiceNumber = p.SenderInvoiceNumber,
                                   RecipientInvoiceNumber = p.RecipientInvoiceNumber,
                                   RecipientAddress = rAd.Name,
                                   SenderAddress = sAd.Name,
                                   City = new OrderCityIdSelectViewModel()
                                   {
                                       CityId = p.RecipientCityId
                                   },
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

                                   OrderDescription = p.ProductOrderDescription,
                                   ProductTransactionGroup = from i in p.ProductTransactionGroup
                                                             select new ProductTransactionGroupViewModel
                                                             {
                                                                 Id = i.Id,
                                                                 OrderId = i.OrderId,
                                                                 Content = i.Content,
                                                                 Count = i.Count,
                                                                 QuantityPerUnit = i.QuantityPerUnit,
                                                                 GtipCode = i.GtipCode,
                                                                 SKU = i.SKU,
                                                                 isPackage = i.isPackage,
                                                                 isPackagedCount = i.isPackagedCount,
                                                                 isReadOnly = i.isReadOnly,



                                                             },


                               }).FirstOrDefaultAsync();
            return order;
        }
        public async Task<ServiceCallResult> AddOrderPackageAsync(OrderPackageAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };


            if (model.OrderPackageProductListViewModels != null)
            {
                foreach (var item in model.OrderPackageProductListViewModels)
                {
                    var packages = new Packages
                    {
                        OrderId = model.OrderId,
                    };
                    packages.Id = item.Id;
                    packages.Width = item.Width;
                    packages.Height = item.Height;
                    packages.Weight = item.Weight;
                    packages.Length = item.Length;
                    packages.Desi = item.Desi;
                    packages.Count = item.Count;

                    foreach (var product in item.OrderPackagedProductGroups)
                    {
                        packages.PackagedProductGroups.Add(new PackagedProductGroups
                        {
                            Count = product.PackagedCount,
                            Content = product.Content,
                            GtipCode = product.GtipCode,
                            Id = product.Id,
                            QuantityPerUnit = product.QuantityPerUnit,
                            SKU = product.SKU,
                        });
                    }



                    _context.Packages.Add(packages);
                }
            }


            var isPackageProduct = _context.ProductTransactionGroup.Where(x => x.OrderId == model.OrderId).ToList();
            var isPackageProduct1 = _context.ProductTransactionGroup.Where(x => x.OrderId == model.OrderId && x.isPackagedCount == 0).ToList();
            var order = _context.Orders.Find(model.OrderId);
            if (isPackageProduct.Count() == isPackageProduct1.Count())
            {
                order.isPackage = true;
            }

            foreach (var prd1 in isPackageProduct)
            {
                prd1.isPackagedCount2 = prd1.isPackagedCount;
                if (prd1.Count > prd1.isPackagedCount && prd1.isPackagedCount == 0)
                {
                    prd1.isPackage = true;
                }
            }



            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetOrderListViewAsync(model.OrderId).ConfigureAwait(false);

                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        
        }
        public async Task<ServiceCallResult> DeleteOrderAsync(long customerOrderId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == customerOrderId).ConfigureAwait(false);
            var packages = _context.Packages.Where(x => x.OrderId == customerOrderId).ToList();
            var senderAddress = await _context.SenderAddresses.FirstOrDefaultAsync(a => a.OrderId == customerOrderId).ConfigureAwait(false);
            var recipientAddress = await _context.RecipientAddresses.FirstOrDefaultAsync(a => a.OrderId == customerOrderId).ConfigureAwait(false);

            if (order == null)
            {
                callResult.ErrorMessages.Add("Böyle bir sipariş bulunamadı.");
                return callResult;
            }



            foreach (var groupDb in order.ProductTransactionGroup.ToList())
            {

                order.ProductTransactionGroup.Remove(groupDb);
                _context.ProductTransactionGroup.Remove(groupDb);
            }
            foreach (var packageDb in packages)
            {
                foreach (var item in packageDb.PackagedProductGroups.ToList())
                {
                    _context.PackagedProductGroups.Remove(item);
                }
                _context.Packages.Remove(packageDb);
            }
            //siparişteki paketleri sil -> 1
            //paketlerdeki ürünleri sil -> 2

            _context.Orders.Remove(order);
            _context.RecipientAddresses.Remove(recipientAddress);
            _context.SenderAddresses.Remove(senderAddress);

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
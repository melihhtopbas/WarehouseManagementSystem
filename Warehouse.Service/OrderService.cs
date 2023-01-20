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

        private readonly WarehouseManagementSystemEntities1 _context;

        public OrderService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }

        private IQueryable<OrderListViewModel> _getOrderListIQueryable(Expression<Func<Data.Orders, bool>> expr)
        {
            return (from b in _context.Orders.AsExpandable().Where(expr)
                    join sAd in _context.SenderAddresses
                    on b.Id equals sAd.OrderId
                    join rAd in _context.RecipientAddresses
                    on b.Id equals rAd.OrderId
                    select new OrderListViewModel()
                    {
                        Id = b.Id,
                        PackageCount = _context.Packages.Where(x => x.OrderId == b.Id).Count(),
                        RecipientAddress = rAd.Name,
                        SenderAddress = sAd.Name,
                        RecipientCity = b.RecipientCity,
                        RecipientName = b.RecipientName,
                        RecipientPhone = b.RecipientPhone,
                        RecipientZipCode = b.RecipientZipCode,
                        SenderName = b.SenderName,
                        SenderPhone = b.SenderPhone,
                        RecipientCountry = b.Countries.Name,
                        CurrencyUnit = b.CurrencyUnits.Name,
                        CargoService = b.CargoServiceTypes.Name,
                        isPackage = b.isPackage,





                    });

        }
        public IQueryable<OrderListViewModel> GetOrderListIQueryable(OrderSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.SenderName.Contains(model.SearchName));

            }
            return _getOrderListIQueryable(predicate);
        }
        public async Task<OrderListViewModel> GetOrderListViewAsync(long orderId)
        {

            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.Id == orderId);
            var order = await _getOrderListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return order;
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
            //eklenen her gruptaki skuyu tek tek kontrol eder. Bunlardan birisi bile VERİTABANINDA var ise hata verir.
            foreach (var item in model.ProductTransactionGroup)
            {
                bool skuExist = await _context.ProductTransactionGroup.AnyAsync(a => a.SKU == item.SKU).ConfigureAwait(false);
                if (skuExist)
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





            var order = new Orders
            {
                Id = model.Id,
                SenderIdentityNumber = model.SenderIdentityNumber,
                SenderMail = model.SenderMail,
                SenderName = model.SenderName,
                SenderPhone = model.SenderPhone,
                SenderInvoiceNumber = model.SenderInvoiceNumber,
                RecipientCity = model.RecipientCity,
                RecipientMail = model.RecipientMail,
                RecipientName = model.RecipientName,
                RecipientPhone = model.RecipientPhone,
                RecipientInvoiceNumber = model.RecipientInvoiceNumber,
                ProductOrderDescription = model.OrderDescription,
                RecipientIdentityNumber = model.RecipientIdentityNumber,
                RecipientZipCode = model.RecipientZipCode,
                CargoServiceTypeId = model.CargoService.CargoServiceId,
                ProductCurrencyUnitId = model.CurrenyUnit.CurrencyUnitId,
                RecipientCountryId = model.Country.CountryId,
                LanguageId = 1,


            };
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
        public async Task<ServiceCallResult> AddOrderPackageAsync(OrderPackageAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };



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

            var isPackageProduct = _context.ProductTransactionGroup.Where(x => x.OrderId == model.OrderId).ToList();
            var isPackageProduct1 = _context.ProductTransactionGroup.Where(x => x.OrderId == model.OrderId && x.isPackagedCount == 0).ToList();
            var order = _context.Orders.Find(model.OrderId);
            if (isPackageProduct.Count() == isPackageProduct1.Count())
            {
                order.isPackage = true;
            }

            foreach (var prd1 in isPackageProduct)
            {
                if (prd1.Count > prd1.isPackagedCount)
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
        public async Task<ServiceCallResult> AddOrderPackageProductAsync(OrderPackageProductAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            var resultModel = new OrderPackageProductListViewModel()
            {
                Count = model.Count,
                Desi =((decimal)((model.Height * model.Width * model.Length)/3000.00)),
                Height = model.Height,
                Length = model.Length,
                OrderPackagedProductGroups = model.OrderProductGroups.Where(x => x.isChecked == true),
                Weight = model.Weight,
                Width = model.Width,

            };
            foreach (var item in model.OrderProductGroups)
            {
                var readOnlyProduct = _context.ProductTransactionGroup.Find(item.Id);
                if (item.isChecked == true)
                {
                    if (item.PackagedCount > item.isPackagedCount)
                    {
                        callResult.ErrorMessages.Add("Belirtilen adetten fazla girmeyiniz!");
                        return callResult;
                    }


                    readOnlyProduct.isReadOnly = true;
                    readOnlyProduct.isPackagedCount = readOnlyProduct.isPackagedCount - item.PackagedCount;
                }
            }



            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = resultModel;
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
                               join sAd in _context.SenderAddresses
                               on p.Id equals sAd.OrderId
                               join rAd in _context.RecipientAddresses
                               on p.Id equals rAd.OrderId
                               select new OrderEditViewModel()
                               {

                                   Id = p.Id,
                                   SenderName = p.SenderName,
                                   SenderIdentityNumber = p.SenderIdentityNumber,
                                   SenderMail = p.SenderMail,
                                   SenderPhone = p.SenderPhone,
                                   SenderInvoiceNumber = p.SenderInvoiceNumber,
                                   RecipientInvoiceNumber = p.RecipientInvoiceNumber,
                                   RecipientAddress = rAd.Name,
                                   SenderAddress = sAd.Name,
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
                                                                 SKU = i.SKU

                                                             },


                               }).FirstOrDefaultAsync();
            return order;
        }
        public async Task<ServiceCallResult> EditOrderAsync(OrderEditViewModel model)
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
            if (order == null)
            {
                callResult.ErrorMessages.Add("Böyle bir sipariş bulunamadı.");
                return callResult;
            }

            order.SenderName = model.SenderName;
            order.SenderIdentityNumber = model.SenderIdentityNumber;
            order.SenderPhone = model.SenderPhone;
            order.SenderMail = model.SenderMail;
            order.SenderInvoiceNumber = model.SenderInvoiceNumber;
            order.RecipientInvoiceNumber = model.RecipientInvoiceNumber;
            order.RecipientPhone = model.RecipientPhone;
            order.RecipientCity = model.RecipientCity;
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








            foreach (var groupDb in order.ProductTransactionGroup.ToArray()
                .Where(groupDb => model.ProductTransactionGroup.All(x => x.SKU != groupDb.SKU || x.SKU == groupDb.SKU)))
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
                        isPackagedCount = groupViewModel.Count

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

        public async Task<List<ProductGroupShowViewModel>> GetOrderProductGroup(int orderId)
        {

            var result = _context.ProductTransactionGroup.Where(p => p.OrderId == orderId).Select(p => new ProductGroupShowViewModel
            {
                Content = p.Content,
                Count = p.Count,
                GtipCode = p.GtipCode,
                Id = p.Id,
                OrderId = p.OrderId,
                QuantityPerUnit = p.QuantityPerUnit,
                SKU = p.SKU,
                isPackage = p.isPackage,
                isReadOnly = false,
                isPackagedCount = p.isPackagedCount


            }).ToList();
            return result;
        }
        public async Task<List<ProductGroupShowViewModel>> GetOrderProductIsPackageGroup(int orderId)
        {

            var result = _context.ProductTransactionGroup.Where(p => p.OrderId == orderId && (p.isPackagedCount != 0)).Select(p => new ProductGroupShowViewModel
            {
                Content = p.Content,
                Count = p.Count,
                GtipCode = p.GtipCode,
                Id = p.Id,
                OrderId = p.OrderId,
                QuantityPerUnit = p.QuantityPerUnit,
                SKU = p.SKU,
                isPackage = p.isPackage,
                isReadOnly = p.isReadOnly,
                isPackagedCount = p.isPackagedCount



            }).ToList();
            return result;
        }
        public async Task<List<OrderPackageListViewModel>> GetOrderPackageGroup(int orderId)
        {


            var result = _context.Packages.Where(p => p.OrderId == orderId).Select(p => new OrderPackageListViewModel
            {
                Id = p.Id,
                Count = p.Count,
                Height = p.Height,
                Length = p.Length,
                Weight = p.Weight,
                Width = p.Width,
                Desi = p.Desi,
                OrderPackageProductGroups = from i in p.PackagedProductGroups
                                            where i.PackageId == p.Id

                                            select new ProductGroupShowViewModel
                                            {
                                                Content = i.Content, //product transaction group'dan güncel olarak ürünün içeriğini çekiyoruz
                                                Id = i.Id,
                                                Count = i.Count,
                                                GtipCode = i.GtipCode,
                                                QuantityPerUnit = i.QuantityPerUnit,
                                                SKU = i.SKU,


                                            },
                



            }).ToList();
            
            return result.OrderBy(a => a.Count).ToList();
        }
        public async Task<ServiceCallResult> DeleteOrderAsync(int orderId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == orderId).ConfigureAwait(false);
            var packages = _context.Packages.Where(x => x.OrderId == orderId).ToList();
            var senderAddress = await _context.SenderAddresses.FirstOrDefaultAsync(a => a.OrderId == orderId).ConfigureAwait(false);
            var recipientAddress = await _context.RecipientAddresses.FirstOrDefaultAsync(a => a.OrderId == orderId).ConfigureAwait(false);

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
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
using System.Net.Http;
using System.Web.Mvc; 

namespace Warehouse.Service.Admin
{
    [Authorize]
    public class OrderService
    { 
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly Users users;
        string name = System.Web.HttpContext.Current.User.Identity.Name;
         



        public OrderService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
             users = _context.Users.FirstOrDefault(x=>x.UserName==name);
        }

        private IQueryable<OrderListViewModel> _getOrderListIQueryable(Expression<Func<Data.Orders, bool>> expr)
        { 
            return (from b in _context.Orders.AsExpandable().Where(expr)
                    join sAd in _context.SenderAddresses
                    on b.Id equals sAd.OrderId
                    join rAd in _context.RecipientAddresses
                    on b.Id equals rAd.OrderId
                    where b.CustomerId == users.Id
                    select new OrderListViewModel()
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
                        CustomerId = users.Id,






                    });

        }
        public IQueryable<OrderListViewModel> GetOrderListIQueryable(OrderSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Orders>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.SenderName.Contains(model.SearchName) || a.SenderPhone.Contains(model.SearchName));

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
        public List<CityListViewModel> GetOrderCityList(long? id)
        {

            var result = _context.Cities.Where(x => x.CountryId == id).Select(b => new CityListViewModel
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
                order.CustomerId = users.Id;




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
                order.CustomerId = users.Id;




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
        public async Task<ServiceCallResult> AddOrderPackageProductAsync(OrderPackageProductAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };


            bool isCheckedProducts = false;
            foreach (var item in model.OrderProductGroups)
            {
                var readOnlyProduct = _context.ProductTransactionGroup.Find(item.Id);
                if (item.isChecked == true)
                {
                    isCheckedProducts = true;
                    if (item.PackagedCount > item.isPackagedCount)
                    {
                        callResult.ErrorMessages.Add("Toplam ürün sayısından fazla adet girmeyiniz!");
                        return callResult;
                    }
                    if (item.PackagedCount == null || item.PackagedCount == null)
                    {
                        callResult.ErrorMessages.Add("Lütfen geçerli bir ürün adet değeri giriniz!");
                        return callResult;
                    }

                    readOnlyProduct.isPackagedCount2 = readOnlyProduct.isPackagedCount;
                    readOnlyProduct.isReadOnly = true;
                    readOnlyProduct.isPackagedCount = readOnlyProduct.isPackagedCount - item.PackagedCount;
                }
            }
            if (isCheckedProducts == false)
            {
                callResult.ErrorMessages.Add("Lütfen paketlemek istediğiniz ürünleri seçiniz ve adet miktarı giriniz!");
                return callResult;
            }
            var resultModel = new OrderPackageProductListViewModel()
            {
                Count = model.Count,

                Height = model.Height,
                Length = model.Length,
                OrderPackagedProductGroups = model.OrderProductGroups.Where(x => x.isChecked == true),
                Weight = model.Weight,
                Width = model.Width,

            };

            decimal desiDegeri = ((decimal)((model.Height * model.Width * model.Length) / 3000.00));
            if (model.Weight >= desiDegeri)
            {
                resultModel.Desi = (decimal)model.Weight;
            }
            else
            {
                resultModel.Desi = desiDegeri;
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
            var productGroup = await _context.ProductTransactionGroup.FirstOrDefaultAsync(a => a.OrderId == model.Id).ConfigureAwait(false);
            if (order == null)
            {
                callResult.ErrorMessages.Add("Böyle bir sipariş bulunamadı.");
                return callResult;
            }
            if (model.City==null)
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
                                                Content = i.Content,
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
        public async Task<ServiceCallResult> OrderPriceCalculate(OrderPriceCalculateViewModel model)
        {
            //chat.openaı.com
            var callResult = new ServiceCallResult() { Success = false };

            if (model.Width == null || model.Height == null || model.Weight == null || model.Length == null)
            {
                callResult.ErrorMessages.Add("Fiyat hesaplanamadı!");
                return callResult;
            }
            //Yurtdışı desi hesabı /5000
            //Yurtiçi  desi hesabı /3000
            model.Desi = null;
            model.TotalPrice = 0;
            decimal? number = 0;
            if (model.Desi != null && model.Desi > (decimal)model.Weight)
            {
                model.Desi = model.Desi;
            }
            else if (model.Desi != null && model.Desi < (decimal)model.Weight)
            {
                model.Desi = (decimal)model.Weight;
            }



            else if (model.Desi == null)
            {
                if (model.Country.CountryId == 20002) //türkiye ise 
                {
                    number = ((decimal?)((model.Width * model.Length * model.Height) / 3000.00));
                }
                else //yurt dışı ise
                {
                    number = ((decimal?)((model.Width * model.Length * model.Height) / 5000.00));
                }

                if (number > model.Weight)
                {
                    model.Desi = number;
                }
                else
                {
                    model.Desi = (decimal)model.Weight;
                }
            }
            decimal yuvarlananDesi = Math.Ceiling((decimal)(model.Desi)); // sayıyı bi üst tam sayıya yuvarlar. 11.1 -> 12 ** 19.99 -> 20 
            if (model.Desi >= 1 && model.Desi < 10)
            {
                if (model.Desi + 0.5m > yuvarlananDesi)
                {
                    model.Desi = yuvarlananDesi;
                }
                else
                {
                    model.Desi = yuvarlananDesi;
                    model.Desi = model.Desi - 0.5m;

                }

            }
            else
            {
                model.Desi = yuvarlananDesi;
            }



            var cargoService = _context.CargoServiceTypes.Find(model.CargoService.CargoServiceId);

            if (cargoService.Id == 1)
            {
                model.TotalPrice = (double)model.Desi * 7.362 * 4;
                model.Description = "Tahmini teslim süresi 1-3 iş günü arasındadır.";
            }
            else if (cargoService.Id == 2)
            {
                model.TotalPrice = (double)model.Desi * 7.362 * 2;
                model.Description = "Tahmini teslim süresi 4-6 iş günü arasındadır.";
            }
            else if (cargoService.Id == 10002)
            {
                model.TotalPrice = (double)model.Desi * 7.362 * 2.5;
                model.Description = "Tahmini teslim süresi 2-4 iş günü arasındadır.";
            }
            else if (cargoService.Id == 10006)
            {
                model.TotalPrice = (double)model.Desi * 7.362 * 3;
                model.Description = "Tahmini teslim süresi 3-5 iş günü arasındadır.";
            }

            model.Service = cargoService.Name;








            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = model;


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
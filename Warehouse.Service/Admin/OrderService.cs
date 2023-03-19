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
using System.Web;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;

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
            users = _context.Users.FirstOrDefault(x => x.UserName == name);
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
        private IQueryable<ProductGroupShowViewModel> _getOrderProductListIQueryable(Expression<Func<Data.ProductTransactionGroup, bool>> expr)
        {


            return (from b in _context.ProductTransactionGroup.AsExpandable().Where(expr)

                    select new ProductGroupShowViewModel()
                    {
                        Id = b.Id,
                        Count = b.Count,
                        Content = b.Content,
                        OrderId = b.OrderId,
                        GtipCode = b.GtipCode,
                        isPackage = b.isPackage,
                        isReadOnly = b.isReadOnly,
                        isPackagedCount = b.isPackagedCount,
                        QuantityPerUnit = b.QuantityPerUnit,
                        SKU = b.SKU,
                    });

        }
        public IQueryable<ProductGroupShowViewModel> GetOrderProductListIQueryable(ProductSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.ProductTransactionGroup>(true);/*AND*/
            predicate.And(a => a.OrderId == model.OrderId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Content.Contains(model.SearchName));

            }
            return _getOrderProductListIQueryable(predicate);
        }
        public async Task<ProductGroupShowViewModel> GetOrderProductListViewAsync(long productId)
        {

            var predicate = PredicateBuilder.New<Data.ProductTransactionGroup>(true);/*AND*/
            predicate.And(a => a.Id == productId);
            var order = await _getOrderProductListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return order;
        }
        public async Task<ServiceCallResult> AddOrderProductAsync(ProductGroupAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.ProductTransactionGroup.AnyAsync(a => a.SKU == model.SKU).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu stok kodunda ürün var!");
                return callResult;
            }

            var product = new ProductTransactionGroup()
            {
                SKU = model.SKU,
                Content = model.Content,
                Count = model.Count,
                GtipCode = model.GtipCode,
                QuantityPerUnit = model.QuantityPerUnit,
                OrderId = model.OrderId,
                isPackagedCount = model.Count,
                isPackagedCount2 = model.Count,


            };


            var order = _context.Orders.FirstOrDefault(x => x.Id == product.OrderId);
            order.isPackage = false;

            _context.ProductTransactionGroup.Add(product);


            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetOrderProductListViewAsync(product.Id).ConfigureAwait(false);

                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<ServiceCallResult> EditOrderProductAsync(ProductGroupEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.ProductTransactionGroup.AnyAsync(a => a.SKU == model.SKU && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu stok kodunda ürün var!");
                return callResult;
            }

            var product = _context.ProductTransactionGroup.FirstOrDefault(x => x.Id == model.Id);
            var order = _context.Orders.FirstOrDefault(x => x.Id == model.OrderId);
           
            var packageProduct = _context.PackagedProductGroups.Where(x => x.ProductId == product.Id).ToList();
            int? counter = 0;
             
            foreach (var item in packageProduct)
            {

                counter += item.Count;
                
            }
            if (model.Count < counter)
            {
                callResult.ErrorMessages.Add("Bu üründen paketlenmiş "+counter+" adet vardır."+counter+ " adetten daha az miktar giremezsiniz.<a href =\"/Admin/OrderPackage/OrderPackage?searchId="+product.OrderId+"\">Ürünlerin olduğu koli</a>");
                return callResult;
            }
            else if (model.Count > counter)
            {
                //product.ispackagedcount = model.Count - product.Count olacak
                //package product güncelle
                int? packagedCount = 0;
                if (packageProduct.Count()>0) // ürün paketlenmiş grupta varsa
                {
                    product.isPackage = false;
                    
                    product.SKU = model.SKU;
                    product.QuantityPerUnit = model.QuantityPerUnit;
                    product.Count = model.Count;
                    product.Content = model.Content;
                    product.GtipCode =model.GtipCode;
                    foreach (var package in packageProduct)
                    {
                       
                        packagedCount += package.Count;
                        package.SKU = model.SKU;
                        package.QuantityPerUnit = model.QuantityPerUnit;
                        package.Content = model.Content;
                        package.GtipCode = model.GtipCode;

                    }
                    product.isPackagedCount = model.Count - packagedCount;
                    product.isPackagedCount2 = model.Count - packagedCount;
                    order.isPackage = false;
                }
                else
                {
                    
                    product.isPackagedCount = model.Count;
                    product.isPackagedCount2 = model.Count;
                    product.SKU = model.SKU;
                    product.QuantityPerUnit = model.QuantityPerUnit;
                    product.Count = model.Count;
                    product.Content = model.Content;
                    product.GtipCode = model.GtipCode;
                }
          
            }
            else if (model.Count == counter)
            {
              
                product.SKU = model.SKU;
                product.QuantityPerUnit = model.QuantityPerUnit;
                product.Count = model.Count;
                product.Content = model.Content;
                product.GtipCode = model.GtipCode;
                product.isPackagedCount = 0;
                product.isPackagedCount2 = 0;
                product.isPackage = true;
                foreach (var package in packageProduct)
                {
                    package.SKU = model.SKU;
                    package.QuantityPerUnit = model.QuantityPerUnit;
                    package.Content = model.Content;
                    package.GtipCode = model.GtipCode;

                }
                _context.SaveChanges(); 
                var pt = _context.ProductTransactionGroup.Where(x => x.OrderId == model.OrderId).ToList();
                var o = _context.Orders.Where(x=>x.Id == model.OrderId).FirstOrDefault();
                int? isCount = 0;
                bool ispcgCount = false;
                foreach (var item in pt)
                {
                    ispcgCount = false;
                    isCount = 0;
                    var pcg = _context.PackagedProductGroups.Where(x => x.ProductId == item.Id).ToList();
                    foreach (var item1 in pcg)
                    {
                        isCount += item1.Count;
                    }
                    if (item.Count == isCount)
                    {
                        ispcgCount = true;
                    }
                }
                if (ispcgCount == true)
                {
                    o.isPackage = true;
                }
            }
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetOrderProductListViewAsync(product.Id).ConfigureAwait(false);

                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<ProductGroupEditViewModel> GetOrderProductEditViewModelAsync(int productId)
        {

            var product = await (from p in _context.ProductTransactionGroup
                                 where p.Id == productId

                                 select new ProductGroupEditViewModel()
                                 {

                                     Id = p.Id,
                                     SKU = p.SKU,
                                     Content = p.Content,
                                     Count = p.Count,
                                     GtipCode = p.GtipCode,
                                     OrderId = p.OrderId,
                                     QuantityPerUnit = p.QuantityPerUnit,


                                 }).FirstOrDefaultAsync();
            return product;
        }
        public async Task<ServiceCallResult> DeleteOrderProductAsync(int productId)
        {
            var callResult = new ServiceCallResult() { Success = false };

            var product = _context.ProductTransactionGroup.FirstOrDefault(x => x.Id == productId);
            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == product.OrderId).ConfigureAwait(false);
            var packageProduct = _context.PackagedProductGroups.Where(x => x.ProductId == productId).ToList();
            long? productOrderId = product.OrderId;

            if (product == null)
            {
                callResult.ErrorMessages.Add("Böyle bir ürün bulunamadı.");
                return callResult;
            }
            if (order.ProductTransactionGroup.Count() == 1)
            {
                callResult.ErrorMessages.Add("Siparişteki son ürünü silemezsiniz!");
                return callResult;
            }

            _context.ProductTransactionGroup.Remove(product);
            foreach (var item in packageProduct)
            {
                var packages = _context.Packages.Where(x => x.Id == item.PackageId).FirstOrDefault();
                if (packages.PackagedProductGroups.Count() <= 1)
                {
                    _context.Packages.Remove(packages);
                }


                _context.PackagedProductGroups.Remove(item);
            }

            _context.SaveChanges();


            var products = _context.ProductTransactionGroup.Where(x => x.OrderId == productOrderId).ToList();
            bool isPackage = false;
            foreach (var productTransaction in products)
            {
                int? counter = 0;
                var packagedProduct = _context.PackagedProductGroups.Where(a => a.ProductId == productTransaction.Id).ToList();
                foreach (var package in packagedProduct)
                {
                    counter += package.Count;
                }
                if (productTransaction.Count == counter)
                {
                    isPackage = true;
                }
                else isPackage = false;

                if (isPackage == true)
                {
                    order.isPackage = true;
                }
                else order.isPackage = false;
            }






            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetOrderProductListViewAsync(order.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public List<CountryListViewModel> GetOrderCountryList()
        {

            var result = _context.Countries.Where(x => x.Active == true).Select(b => new CountryListViewModel
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
        public List<CargoServiceTypeListViewModel> GetOrderCargoServiceTypeList(long? id)
        {

            var result = _context.ShippingPrices.Where(x => x.CountryId == id).Select(b => new CargoServiceTypeListViewModel
            {

                Id = b.Id,
                Name = b.CargoServiceTypes.Name



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
                order.RecipientCountryId = model.Country.CountryId;
                order.LanguageId = 1; //türkçe dili olarak ayarlanır.
                order.Date = DateTime.Now;
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
                order.RecipientCountryId = model.Country.CountryId;
                order.LanguageId = 1; //türkçe dili olarak ayarlanır.
                order.Date = DateTime.Now;
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
                    packages.Barcode = Convert.ToString(item.Id);

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
                            ProductId = product.Id,

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
            var productGroupList = _context.ProductTransactionGroup.Where(x => x.OrderId == model.Id).ToList();
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
                order.CargoServiceTypeId = model.CargoService.CargoServiceId;
                order.ProductOrderDescription = model.OrderDescription;
                recipientAddress.Name = model.RecipientAddress;
                senderAddress.Name = model.SenderAddress;
            }


            for (int i = 0; i < model.ProductTransactionGroup.Count(); i++)
            {
                productGroupList[i].isPackagedCount = model.ProductTransactionGroup.ElementAt(i).isPackagedCount;
                productGroupList[i].isPackage = model.ProductTransactionGroup.ElementAt(i).isPackage;
                productGroupList[i].isReadOnly = model.ProductTransactionGroup.ElementAt(i).isReadOnly;

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



            var shipping = _context.ShippingPrices.Where(x => x.Id == model.CargoService.CargoServiceId).FirstOrDefault();
            var country = _context.Countries.Where(x => x.Id == model.Country.CountryId).FirstOrDefault();
            var currency = _context.CurrencyUnits.Where(x => x.Id == country.CurrencyUnitId).FirstOrDefault();
            model.Service = shipping.CargoServiceTypes.Name;
            model.Icon = currency.Icon;
            model.TotalPrice = (double?)(model.Desi * shipping.Price);
            model.DeliveryTime = shipping.DeliveryTime;
            if (model.DeliveryTime != null)
            {

                model.Description = "Kargonuz " + model.Service + " kargo firması ile " + model.DeliveryTime + " iş günü içerisinde teslim edilecektir ";



            }
            else
            {
                model.Description = "Kargonuz " + model.Service + " kargo firması ile teslim edilecektir";
            }










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
        public ProductGroupShowViewModel SelectPackageCount(int selectId)
        {
            var package = _context.ProductTransactionGroup.FirstOrDefault(x => x.Id == selectId);
            var model = new ProductGroupShowViewModel
            {
                Id = selectId,
                Count = package.Count,
                Content = package.Content,
                GtipCode = package.GtipCode,
                isPackagedCount = package.isPackagedCount,
                isPackage = package.isPackage,
                isReadOnly = package.isReadOnly,
                OrderId = package.OrderId,
                QuantityPerUnit = package.QuantityPerUnit,
                SKU = package.SKU,
                PackagedCount = package.isPackagedCount

            };
            return model;
        }

    }
}
using LinqKit;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class OrderPackageService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        private readonly CurrentUserViewModel _currentUser;
        public OrderPackageService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
            _currentUser = DependencyResolver.Current.GetService<CurrentUserService>().GetCurrentUserViewModel(HttpContext.Current.User.Identity.Name);
        }
        private IQueryable<OrderPackageListViewModel> _getOrderPackageListIQueryable(Expression<Func<Data.Packages, bool>> expr)
        {

            return (from b in _context.Packages.AsExpandable().Where(expr)

                    join o in _context.Orders
                    on b.OrderId equals o.Id
                    where o.CustomerId == _currentUser.Id

                    select new OrderPackageListViewModel()
                    {
                        Id = b.Id,
                        Height = b.Height,
                        Weight = b.Weight,
                        Length = b.Length,
                        Width = b.Width,
                        Desi = b.Desi,
                        OrderId = b.OrderId,
                        Barcode = b.Barcode,
                        CountryName = o.Countries.Name,
                        ReceiverName = o.RecipientName,






                    });
        }
        public List<OrderPackageListViewModel> GetPackageList()
        {

            var result = _context.Packages.Select(b => new OrderPackageListViewModel
            {
                Desi = b.Desi,
                OrderId = b.OrderId,
                Barcode = b.Barcode,
                Count = b.Count,
                Height = b.Height,
                Id = b.Id,
                Length = b.Length,
                Weight = b.Weight,
                Width = b.Width,


            }).ToList();
            return result;
        }

        public IQueryable<OrderPackageListViewModel> GetOrderPackageListIQueryable(OrderPackageSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Packages>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Barcode.Contains(model.SearchName));

            }
            string name = Convert.ToString(model.SearchId);
            string packageName = Convert.ToString(model.PackageId);


            if (!string.IsNullOrWhiteSpace(name))
            {
                predicate.And(a => a.OrderId.Value.Equals(model.SearchId.Value));

            }
            if (!string.IsNullOrWhiteSpace(packageName))
            {
                predicate.And(a => a.Id.Equals(model.PackageId.Value));

            }
            return _getOrderPackageListIQueryable(predicate);
        }


        public async Task<OrderPackageListViewModel> GetOrderPackageListViewAsync(long packageId)
        {

            var predicate = PredicateBuilder.New<Data.Packages>(true);/*AND*/
            predicate.And(a => a.Id == packageId);
            var country = await _getOrderPackageListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return country;
        }
        public async Task<ServiceCallResult> DeleteOrderPackageAsync(int packageId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var package = await _context.Packages.FirstOrDefaultAsync(a => a.Id == packageId).ConfigureAwait(false);
            if (package == null)
            {
                callResult.ErrorMessages.Add("Böyle bir paket bulunamadı.");
                return callResult;
            }
            var order = _context.Orders.FirstOrDefault(x => x.Id == package.OrderId);
            order.isPackage = false;

            var packagedProducts = _context.PackagedProductGroups.Where(x => x.PackageId == packageId).ToList();
            foreach (var product in packagedProducts)
            {
                var productGroups = _context.ProductTransactionGroup.Where(x => x.Id == product.ProductId).FirstOrDefault();
                if (product.ProductId == productGroups.Id)
                {
                    productGroups.isPackagedCount += product.Count;
                    productGroups.isPackagedCount2 += product.Count;
                    productGroups.isPackage = false;
                    if (productGroups.Count == productGroups.isPackagedCount)
                    {
                        productGroups.isReadOnly = false;
                    }
                }
            }

            foreach (var product in packagedProducts)
            {
                _context.PackagedProductGroups.Remove(product);
            }
            _context.Packages.Remove(package);





            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetOrderPackageListViewAsync(package.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<OrderPackageProductEditViewModel> GetPackageEditViewModelAsync(long packageId)
        {
            var package = await (from p in _context.Packages
                                 where p.Id == packageId

                                 select new OrderPackageProductEditViewModel()
                                 {

                                     Id = p.Id,
                                     OrderId = (long)p.OrderId,
                                     Barcode = p.Barcode,



                                     ProductGroupsEditViewModels = from i in p.PackagedProductGroups
                                                                   join packaged in _context.Packages
                                                                   on i.PackageId equals packaged.Id
                                                                   join product in _context.ProductTransactionGroup
                                                                   on i.ProductId equals product.Id
                                                                   select new ProductGroupsEditViewModel
                                                                   {
                                                                       Id = i.Id,
                                                                       Content = i.Content,
                                                                       SKU = i.SKU,
                                                                       ProductCount = product.isPackagedCount,
                                                                       PackageId = (long)i.PackageId,
                                                                       ProductId = (long)i.ProductId,
                                                                       PackagedProductCount = i.Count,
                                                                       TotalCount = product.Count






                                                                   },


                                 }).FirstOrDefaultAsync();
            return package;
        }

        public async Task<ServiceCallResult> EditPackageAsync(OrderPackageProductEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var package = await _context.Packages.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            package.Barcode = model.Barcode;

            if (package == null)
            {
                callResult.ErrorMessages.Add("Böyle bir paket bulunamadı.");
                return callResult;
            }
            if (model.ProductGroupsEditViewModels != null)
            {
                foreach (var item in model.ProductGroupsEditViewModels)
                {
                    if (item.IsPackagedProductCount != null && item.IsPackagedProductCount != item.PackagedProductCount)
                    {
                        if (item.IsPackagedProductCount > (item.PackagedProductCount + item.ProductCount))
                        {
                            callResult.ErrorMessages.Add("Belirtilen adetten daha fazla giremezsiniz.");
                            return callResult;
                        }
                        else if (item.IsPackagedProductCount <= (item.PackagedProductCount + item.ProductCount) && item.IsPackagedProductCount > item.PackagedProductCount)
                        {
                            int counter = (int)(item.IsPackagedProductCount - item.PackagedProductCount);
                            var packagedProducts = _context.PackagedProductGroups.Where(x => x.PackageId == model.Id && x.ProductId == item.ProductId).FirstOrDefault();
                            var productTransaction = _context.ProductTransactionGroup.Where(x => x.Id == item.ProductId).FirstOrDefault();
                            packagedProducts.Count += counter;
                            productTransaction.isPackagedCount -= counter; productTransaction.isPackagedCount2 -= counter;
                            if (productTransaction.isPackagedCount == 0)
                            {
                                productTransaction.isPackage = true;
                            }
                        }
                        else if (item.IsPackagedProductCount < item.PackagedProductCount)
                        {
                            int counter = (int)(item.PackagedProductCount - item.IsPackagedProductCount);
                            var packagedProducts = _context.PackagedProductGroups.Where(x => x.PackageId == model.Id && x.ProductId == item.ProductId).FirstOrDefault();
                            var productTransaction = _context.ProductTransactionGroup.Where(x => x.Id == item.ProductId).FirstOrDefault();
                            packagedProducts.Count -= counter;
                            productTransaction.isPackagedCount += counter; productTransaction.isPackagedCount2 += counter;
                            if (productTransaction.Count == productTransaction.isPackagedCount)
                            {
                                productTransaction.isReadOnly = false;
                            }
                        }

                    }

                }
            }
            _context.SaveChanges();
            var productTransactionGroup = _context.ProductTransactionGroup.Where(x => x.OrderId == package.OrderId).ToList();
            var productTransactionGroup1 = _context.ProductTransactionGroup.Where(x => x.OrderId == package.OrderId && x.isPackagedCount == 0).ToList();
            var order = _context.Orders.Where(x => x.Id == package.OrderId).FirstOrDefault();
            if (productTransactionGroup.Count() == productTransactionGroup1.Count())
            {

                order.isPackage = true;
            }
            else
            {
                order.isPackage = false;
            }








            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetOrderPackageListViewAsync(package.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteOrderPackageProductAsync(int packageProductId)
        {
            var callResult = new ServiceCallResult() { Success = false };



            var productPackageGroup = _context.PackagedProductGroups.Where(a => a.Id == packageProductId).FirstOrDefault();
            long packadeId = (long)productPackageGroup.PackageId;
            if (productPackageGroup == null)
            {
                callResult.ErrorMessages.Add("Böyle bir paket ürünü bulunamadı.");
                return callResult;
            }
            var productTransactionGroup = _context.ProductTransactionGroup.Where(x => x.Id == productPackageGroup.ProductId).FirstOrDefault();
            var order = _context.Orders.FirstOrDefault(x => x.Id == productTransactionGroup.OrderId);
            order.isPackage = false;

            var packagedProducts = _context.PackagedProductGroups.Where(x => x.Id == packageProductId).ToList();
            foreach (var product in packagedProducts)
            {
                var productGroups = _context.ProductTransactionGroup.Where(x => x.Id == product.ProductId).FirstOrDefault();
                if (product.ProductId == productGroups.Id)
                {
                    productGroups.isPackagedCount += product.Count;
                    productGroups.isPackagedCount2 += product.Count;
                    productGroups.isPackage = false;
                    if (productGroups.Count == productGroups.isPackagedCount)
                    {
                        productGroups.isReadOnly = false;
                    }
                }
                _context.PackagedProductGroups.Remove(product);
            }

            _context.SaveChanges();
            var packagedProductGroup = _context.PackagedProductGroups.Where(x => x.PackageId == packadeId).ToList();

            if (packagedProductGroup.Count() == 0)
            {
                var package = _context.Packages.Where(x => x.Id == packadeId).FirstOrDefault();
                _context.Packages.Remove(package);
                callResult.Item = true;
            }



            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetPackageEditViewModelAsync((long)productPackageGroup.PackageId).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<OrderPackageIntoAddProductViewModel> GetPackageProductAddViewModelAsync(long orderId, long orderPackageId)
        {
            
            var model = new OrderPackageIntoAddProductViewModel
            {
                OrderId= orderId,
                PackageId = orderPackageId,
                ProductGroupsEditViewModels = _context.ProductTransactionGroup.Where(x=>x.OrderId == orderId && x.isPackagedCount != 0).Select(x=> new ProductGroupsEditViewModel
                {
                    Content= x.Content,
                    Id= x.Id,
                    ProductCount = x.isPackagedCount,
                    SKU = x.SKU,
                    ProductId = x.Id,
                    PackageId = orderPackageId,
                    
                }).ToList(),
            };
            return model;
        }
        public async Task<ServiceCallResult> PackageProductAddAsync(OrderPackageIntoAddProductViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var package = await _context.Packages.FirstOrDefaultAsync(a => a.Id == model.PackageId).ConfigureAwait(false);
            
            if (package == null)
            {
                callResult.ErrorMessages.Add("Böyle bir paket bulunamadı.");
                return callResult;
            }
            bool isCheckedProducts = false;
            foreach (var item in model.ProductGroupsEditViewModels)
            {
                var productTransaction = _context.ProductTransactionGroup.Where(x=>x.Id == item.ProductId).FirstOrDefault();
                var packageTransaction = _context.PackagedProductGroups.Where(x => x.PackageId == item.PackageId && x.ProductId == item.ProductId).FirstOrDefault();
                if (item.isChecked == true)
                {
                    isCheckedProducts = true;
                    if (item.PackagedProductCount > item.ProductCount)
                    {
                        callResult.ErrorMessages.Add("Toplam ürün sayısından fazla adet girmeyiniz!");
                        return callResult;
                    }
                    if (item.PackagedProductCount == null || item.PackagedProductCount == 0)
                    {
                        callResult.ErrorMessages.Add("Lütfen geçerli bir ürün adet değeri giriniz!");
                        return callResult;
                    }

                    productTransaction.isPackagedCount2 -= item.PackagedProductCount;
                    productTransaction.isReadOnly = true;
                    productTransaction.isPackagedCount -= item.PackagedProductCount;
                    if (productTransaction.isPackagedCount == 0)
                    {
                        productTransaction.isPackage = true;
                    }
                    if (packageTransaction != null)
                    {
                        packageTransaction.Count += item.PackagedProductCount;
                    }
                    else
                    {
                        _context.PackagedProductGroups.Add(new PackagedProductGroups
                        {
                            Content = item.Content,
                            SKU = item.SKU,
                            PackageId = item.PackageId, 
                            ProductId = item.ProductId,
                            QuantityPerUnit = item.QuantityPerUnit,
                            GtipCode = item.GtipCode,
                            Count = item.PackagedProductCount,
                        });
                    }
              

                }
            }
            if (isCheckedProducts == false)
            {
                callResult.ErrorMessages.Add("Lütfen paketlemek istediğiniz ürünleri seçiniz ve adet miktarı giriniz!");
                return callResult;
            }
               _context.SaveChanges();
            var productTransactionGroup = _context.ProductTransactionGroup.Where(x => x.OrderId == package.OrderId).ToList();
            var productTransactionGroup1 = _context.ProductTransactionGroup.Where(x => x.OrderId == package.OrderId && x.isPackagedCount == 0).ToList();
            var order = _context.Orders.Where(x => x.Id == package.OrderId).FirstOrDefault();
            if (productTransactionGroup.Count() == productTransactionGroup1.Count())
            {

                order.isPackage = true;
            }
            else
            {
                order.isPackage = false;
            }
           






            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetOrderPackageListViewAsync(package.Id).ConfigureAwait(false);
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
 
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
                         

                        
                        

                    });
        }
        public List<OrderPackageListViewModel> GetPackageList()
        {

            var result = _context.Packages.Select(b => new OrderPackageListViewModel
            {
                 Desi= b.Desi,
                 OrderId= b.OrderId,
                 Barcode= b.Barcode,
                 Count= b.Count,
                 Height= b.Height,
                 Id= b.Id,
                 Length= b.Length,
                 Weight= b.Weight,
                 Width= b.Width,
              

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
    }
}

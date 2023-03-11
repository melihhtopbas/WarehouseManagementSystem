using LinqKit;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
                        OrderId = (long)b.OrderId,

                        
                        

                    });
        }

        public IQueryable<OrderPackageListViewModel> GetOrderPackageListIQueryable(OrderPackageSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Packages>(true);/*AND*/
         
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                 

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

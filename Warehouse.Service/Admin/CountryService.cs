using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Admin;

namespace Warehouse.Service.Admin
{
    public class CountryService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public CountryService(WarehouseManagementSystemEntities1 context)
        {
            _context = context; 
        }
        private IQueryable<CountryListViewModel> _getCountryListIQueryable(Expression<Func<Data.Countries, bool>> expr)
        {
            return (from b in _context.Countries.AsExpandable().Where(expr)
                    select new CountryListViewModel()
                    {
                        Id= b.Id,
                        Name = b.Name

                    });
        }

        public IQueryable<CountryListViewModel> GetCountryListIQueryable(OrderSearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/ 
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Name.Contains(model.SearchName));

            }
            return _getCountryListIQueryable(predicate);
        }

        public async Task<CountryListViewModel> GetServiceListViewAsync(long countryId)
        {

            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.Id == countryId);
            var country = await _getCountryListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return country;
        }
    }
}

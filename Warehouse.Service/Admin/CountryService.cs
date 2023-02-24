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
            var model = _context.Countries.ToList();
            foreach (var item in model)
            {
                item.LanguageId = 1;
            }
            _context.SaveChanges();

            return (from b in _context.Countries.AsExpandable().Where(expr)
                    select new CountryListViewModel()
                    {
                        Id= b.Id,
                        Name = b.Name,
                        LanguageId = b.LanguageId

                    });
        }

        public IQueryable<CountryListViewModel> GetCountryListIQueryable(CountrySearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                predicate.And(a => a.Name.Contains(model.SearchName));

            }
            return _getCountryListIQueryable(predicate);
        }

        public async Task<CountryListViewModel> GetCountryListViewAsync(long countryId)
        {

            var predicate = PredicateBuilder.New<Data.Countries>(true);/*AND*/
            predicate.And(a => a.Id == countryId);
            var country = await _getCountryListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return country;
        }
    }
}

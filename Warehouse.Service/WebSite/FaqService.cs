using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.WebSite;

namespace Warehouse.Service.WebSite
{
    public class FaqService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public FaqService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<FaqListViewModel> _getFaqListIQueryable(Expression<Func<Data.FAQ, bool>> expr)
        {
            return (from b in _context.FAQ.AsExpandable().Where(expr)
                    .Where(b => b.FAQCategories.Active==true)  
                    select new FaqListViewModel()
                    {
                        CategoryName = b.FAQCategories.Name,
                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link,
                        Description = b.Description
                    });
        }
        public IQueryable<FaqListViewModel> GetFaqListIQueryable(string languageCode)
        {
            var predicate = PredicateBuilder.New<Data.FAQ>(true);/*AND*/
            predicate.And(a => a.Languages.ShortName == languageCode);
            return _getFaqListIQueryable(predicate);
        }

    }
}

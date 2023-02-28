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
    public class ReferenceService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public ReferenceService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<ReferenceListViewModel> _getReferenceListIQueryable(Expression<Func<Data.References, bool>> expr)
        {
            return (from b in _context.References.AsExpandable().Where(expr)
                    .Where(x=> x.Active==true)   
                    select new ReferenceListViewModel()
                    {
                        FileName = b.FileName,
                        Name = b.Name,
                        Id = b.Id,
                        Active = b.Active

                    });
        }
        public IQueryable<ReferenceListViewModel> GetHomePageReferencesListIQueryable()
        {
            var predicate = PredicateBuilder.New<Data.References>(true);/*AND*/
            return _getReferenceListIQueryable(predicate);
        }
    }
}

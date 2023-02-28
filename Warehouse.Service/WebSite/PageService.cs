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
    public class PageService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public PageService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<PageListViewModel> _getPageListIQueryable(Expression<Func<Data.Pages, bool>> expr)
        {
            return (from b in _context.Pages.AsExpandable().Where(expr)
                    .Where(x=>x.Active==true)
                    select new PageListViewModel()
                    {

                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link,
                        Active = b.Active

                    });
        }
        public IQueryable<PageListViewModel> GetPageListIQueryable(string languageCode)
        {
            var predicate = PredicateBuilder.New<Data.Pages>(true);/*AND*/
            predicate.And(a => a.Languages.ShortName == languageCode);
            return _getPageListIQueryable(predicate);
        }

        public IQueryable<PageListViewModel> GetHomePageListIQueryable(string languageCode)
        {
            var predicate = PredicateBuilder.New<Data.Pages>(true);/*AND*/
            predicate.And(a => a.Languages.ShortName == languageCode);
            return _getPageListIQueryable(predicate);
        }
        public PageDetailViewModel GetPageDetail(string languageCode, string link)
        {

            return (
                from p in _context.Pages
                where p.Link == link && p.Languages.ShortName == languageCode
                select new PageDetailViewModel()
                {
                    Description = p.Description,
                    Name = p.Name,
                    Link = p.Link
                }).FirstOrDefault();
        }
    }
}

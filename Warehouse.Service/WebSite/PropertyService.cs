using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.ViewModels.Common;
using Warehouse.ViewModels.WebSite;

namespace Warehouse.Service.WebSite
{
    public class PropertyService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public PropertyService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<PropertyListViewModel> _getPropertyListIQueryable(Expression<Func<Data.Properties, bool>> expr)
        {
            return (from b in _context.Properties.AsExpandable().Where(expr)
                    .Where(x=>x.Active==true)
                    select new PropertyListViewModel()
                    {

                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link,
                        Description = b.Description,
                        MainImage = new ImageListViewModel()
                        {
                            Title = string.Empty,
                            FileName = b.Icon,
                            Alt = string.Empty
                        },
                        Active = b.Active,
                        ShortDescription = b.ShortDescription
                    }).OrderBy(x=>x.Id);
        }
        public IQueryable<PropertyListViewModel> GetPropertyListIQueryable(string languageCode)
        {
            var predicate = PredicateBuilder.New<Data.Properties>(true);/*AND*/
            predicate.And(a => a.Languages.ShortName == languageCode);
            return _getPropertyListIQueryable(predicate);
        }

        public IQueryable<PropertyListViewModel> GetHomePagePropertyListIQueryable(string languageCode)
        {
            var predicate = PredicateBuilder.New<Data.Properties>(true);/*AND*/
            predicate.And(a => a.Languages.ShortName == languageCode);
            return _getPropertyListIQueryable(predicate);
        }
        public PropertyDetailViewModel GetPropertyDetail(string languageCode, string link)
        {

            return (
                from p in _context.Properties
                where p.Link == link && p.Languages.ShortName == languageCode
                select new PropertyDetailViewModel()
                {
                    Description = p.Description,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    Link = p.Link,
                    MainImage = new ImageListViewModel()
                    {
                        Title = string.Empty,
                        FileName = p.FileName,
                        Alt = string.Empty

                    }

                }).FirstOrDefault();
        }
    }
}

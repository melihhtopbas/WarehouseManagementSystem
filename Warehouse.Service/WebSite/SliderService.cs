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
    public class SliderService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public SliderService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }


        private IQueryable<SliderListViewModel> _getSliderListIQueryable(Expression<Func<Data.Sliders, bool>> expr)
        {
            return (from s in _context.Sliders.AsExpandable().Where(expr)
                    select new SliderListViewModel()
                    {
                        FileName = s.FileName,
                        ButtonLink1 = s.ButtonLink1,
                        ButtonLink2 = s.ButtonLink2,
                        ButtonLink3 = s.ButtonLink3,
                        ButtonLink4 = s.ButtonLink4,
                        ButtonText1 = s.ButtonText1,
                        ButtonText2 = s.ButtonText2,
                        ButtonText3 = s.ButtonText3,
                        ButtonText4 = s.ButtonText4
                    });
        }

        public IQueryable<SliderListViewModel> GetSliderListIQueryable()
        {
            var predicate = PredicateBuilder.New<Data.Sliders>(true);/*AND*/
            predicate.And(a => a.Languages.ShortName == "tr");
            predicate.And(a => a.Active);
            return _getSliderListIQueryable(predicate);
        }


    }
}

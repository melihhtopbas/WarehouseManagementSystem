using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Utils.Helpers;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class PageService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public PageService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<PageListViewModel> _getPagesListIQueryable(Expression<Func<Data.Pages, bool>> expr)
        {
            return (from b in _context.Pages.AsExpandable().Where(expr)
                    select new PageListViewModel()
                    {
                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link,
                        Description = b.Description,
                        Active = b.Active,

                    });
        }
        public IQueryable<PageListViewModel> GetPagesListIQueryable(PageSearchViewModel pageSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.Pages>(true);/*AND*/
            predicate.And(a => a.LanguageId == pageSearchViewModel.LanguageId);
            if (!string.IsNullOrWhiteSpace(pageSearchViewModel.Name))
            {
                predicate.And(a => a.Name.Contains(pageSearchViewModel.Name));
            }

            return _getPagesListIQueryable(predicate);
        }
        public async Task<PageListViewModel> GetPageListViewAsync(long pageId)
        {

            var predicate = PredicateBuilder.New<Data.Pages>(true);/*AND*/
            predicate.And(a => a.Id == pageId);
            var page = await _getPagesListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return page;
        }
        public async Task<ServiceCallResult> AddPageAsync(PageAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.Pages.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Sayfa bulunmaktadır.");
                return callResult;
            }

            var page = new Pages()
            {
                Description = model.Description,
                LanguageId = model.LanguageId,
                Name = model.Name,
                Link = HelperMethods.UrlFriendly(model.Name), //yazdığımız ismi küçük harfler ve aralarında '-' olacak şekilde linklendiriyor.
                Active = model.Active
            };
            _context.Pages.Add(page);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetPageListViewAsync(page.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<PageEditViewModel> GetPageEditViewModelAsync(int pageId)
        {
            var page = await (from b in _context.Pages
                              where b.Id == pageId
                              select new PageEditViewModel()
                              {
                                  Name = b.Name,
                                  Id = b.Id,
                                  LanguageId = b.LanguageId,

                                  Description = b.Description,
                                  Active = b.Active

                              }).FirstOrDefaultAsync();
            return page;
        }
        public async Task<ServiceCallResult> EditPageAsync(PageEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.Pages.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Sayfa bulunmaktadır.");
                return callResult;
            }

            var page = await _context.Pages.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (page == null)
            {
                callResult.ErrorMessages.Add("Böyle bir Sayfa bulunamadı.");
                return callResult;
            }

            page.Description = model.Description;
            page.Name = model.Name;
            page.Active = model.Active;

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    callResult.Success = true;
                    callResult.Item = await GetPageListViewAsync(page.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeletePageAsync(int pageId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var page = await _context.Pages.FirstOrDefaultAsync(a => a.Id == pageId).ConfigureAwait(false);
            if (page == null)
            {
                callResult.ErrorMessages.Add("Böyle bir Sayfa bulunamadı.");
                return callResult;
            }
            _context.Pages.Remove(page);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
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

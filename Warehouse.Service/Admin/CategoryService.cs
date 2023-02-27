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
    public class CategoryService
    {
        private readonly WarehouseManagementSystemEntities1 _context;

        public CategoryService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<CategoryListViewModel> _getFaqCategoryListIQueryable(Expression<Func<Data.FAQCategories, bool>> expr)
        {
            return (from b in _context.FAQCategories.AsExpandable().Where(expr)
                    select new CategoryListViewModel()
                    {

                        Name = b.Name,
                        Id = b.Id,
                        CategoryType = CategoryTypes.FAQ

                    });
        }

        public IQueryable<CategoryListViewModel> GetFaqCategoryListIQueryable(long languageId)
        {
            var predicate = PredicateBuilder.New<Data.FAQCategories>(true);/*AND*/
            predicate.And(a => a.LanguageId == languageId);
            return _getFaqCategoryListIQueryable(predicate);
        }
        public async Task<CategoryListViewModel> GetFaqCategoryListViewAsync(long faqId)
        {

            var predicate = PredicateBuilder.New<Data.FAQCategories>(true);/*AND*/
            predicate.And(a => a.Id == faqId);
            var faq = await _getFaqCategoryListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return faq;
        }

        public async Task<ServiceCallResult> AddFaqCategoryAsync(FaqCategoryCrudViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.FAQCategories.AnyAsync(a => a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Faq Kategorisi bulunmaktadır.");
                return callResult;
            }

            var faqCategory = new FAQCategories()
            {
                Name = model.Name,
                Active = model.Active,
                LanguageId = model.LanguageId,
                Link = HelperMethods.UrlFriendly(model.Name),
                SortOrder = model.SortOrder
            };
            _context.FAQCategories.Add(faqCategory);

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetFaqCategoryListViewAsync(faqCategory.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }

        public async Task<FaqCategoryCrudViewModel> GetFaqCategoryEditViewModelAsync(long faqCategoryId)
        {
            var faqCategory = await (from b in _context.FAQCategories
                                     where b.Id == faqCategoryId
                                     select new FaqCategoryCrudViewModel()
                                     {
                                         Name = b.Name,
                                         Id = b.Id,
                                         LanguageId = b.LanguageId,
                                         Active = b.Active,
                                         SortOrder = b.SortOrder

                                     }).FirstOrDefaultAsync();
            return faqCategory;
        }
        public async Task<ServiceCallResult> EditFaqCategoryAsync(FaqCategoryCrudViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.FAQCategories.AnyAsync(a => a.Id != model.Id && a.Name == model.Name && a.LanguageId == model.LanguageId).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Faq Kategorisi bulunmaktadır.");
                return callResult;
            }
            var faqCategory = await _context.FAQCategories.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (faqCategory == null)
            {
                callResult.ErrorMessages.Add("Böyle bir faq kategorisi bulunamadı.");
                return callResult;
            }

            faqCategory.Name = model.Name;
            faqCategory.Active = model.Active;
            faqCategory.SortOrder = model.SortOrder;


            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetFaqCategoryListViewAsync(faqCategory.Id).ConfigureAwait(false);
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

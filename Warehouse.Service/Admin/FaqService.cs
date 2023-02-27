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
    public class FaqService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public FaqService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<FaqListViewModel> _getFaqsListIQueryable(Expression<Func<Data.FAQ, bool>> expr)
        {
            return (from b in _context.FAQ.AsExpandable().Where(expr)
                    select new FaqListViewModel()
                    {
                        CategoryName = b.FAQCategories.Name,
                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link


                    });
        }
        public IQueryable<FaqListViewModel> GetFaqsListIQueryable(FaqSearchViewModel faqSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.FAQ>(true);/*AND*/
            predicate.And(a => a.LanguageId == faqSearchViewModel.LanguageId);
            if (!string.IsNullOrWhiteSpace(faqSearchViewModel.Name))
            {
                predicate.And(a => a.Name.Contains(faqSearchViewModel.Name));
            }

            return _getFaqsListIQueryable(predicate);
        }
        public async Task<FaqListViewModel> GetFaqListViewAsync(long faqId)
        {

            var predicate = PredicateBuilder.New<Data.FAQ>(true);/*AND*/
            predicate.And(a => a.Id == faqId);
            var faq = await _getFaqsListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return faq;
        }
        private IQueryable<FaqListViewModel> _getActiveFaqsListIQueryable(Expression<Func<Data.FAQ, bool>> expr)
        {
            return (from b in _context.FAQ.AsExpandable().Where(expr)
                    select new FaqListViewModel()
                    {
                        CategoryName = b.FAQCategories.Name,
                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link


                    });
        }
        public IQueryable<FaqListViewModel> GetActiveFaqsListIQueryable(FaqSearchViewModel faqSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.FAQ>(true);/*AND*/
            predicate.And(a => a.LanguageId == faqSearchViewModel.LanguageId);
            if (!string.IsNullOrWhiteSpace(faqSearchViewModel.Name))
            {
                predicate.And(a => a.Name.Contains(faqSearchViewModel.Name));
            }

            return _getFaqsListIQueryable(predicate);
        }
        public async Task<FaqListViewModel> GetActiveFaqListViewAsync(long faqId)
        {

            var predicate = PredicateBuilder.New<Data.FAQ>(true);/*AND*/
            predicate.And(a => a.Id == faqId);
            var faq = await _getFaqsListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return faq;
        }
        public async Task<ServiceCallResult> AddFaqAsync(FaqAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.FAQ.AnyAsync(a => a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde SSS bulunmaktadır.");
                return callResult;
            }

            var faq = new FAQ()
            {
                CategoryId = model.Category.CategoryId,
                Description = model.Description,
                LanguageId = model.LanguageId,
                Name = model.Name,
                Link = HelperMethods.UrlFriendly(model.Name)
            };
            _context.FAQ.Add(faq);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetActiveFaqListViewAsync(faq.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<FaqEditViewModel> GetFaqEditViewModelAsync(int faqId)
        {
            var faq = await (from b in _context.FAQ
                             where b.Id == faqId
                             select new FaqEditViewModel()
                             {
                                 Name = b.Name,
                                 Id = b.Id,
                                 LanguageId = b.LanguageId,
                                 Category = new FaqCategoryIdSelectModel()
                                 {
                                     CategoryId = b.CategoryId.Value
                                 },
                                 Description = b.Description

                             }).FirstOrDefaultAsync();
            return faq;
        }
        public async Task<ServiceCallResult> EditFaqAsync(FaqEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.FAQ.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde SSS bulunmaktadır.");
                return callResult;
            }

            var faq = await _context.FAQ.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (faq == null)
            {
                callResult.ErrorMessages.Add("Böyle bir faq bulunamadı.");
                return callResult;
            }

            faq.CategoryId = model.Category.CategoryId;
            faq.Description = model.Description;
            faq.Name = model.Name;

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    callResult.Success = true;
                    callResult.Item = await GetFaqListViewAsync(faq.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteFaqAsync(int faqId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var faq = await _context.FAQ.FirstOrDefaultAsync(a => a.Id == faqId).ConfigureAwait(false);
            if (faq == null)
            {
                callResult.ErrorMessages.Add("Böyle bir SSS bulunamadı.");
                return callResult;
            }
            _context.FAQ.Remove(faq);
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

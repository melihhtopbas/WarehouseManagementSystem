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
using Warehouse.ViewModels.Common;

namespace Warehouse.Service.Admin
{
    public class ReferenceService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public ReferenceService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<ReferenceListViewModel> _getReferencesListIQueryable(Expression<Func<Data.References, bool>> expr)
        {
            return (from b in _context.References.AsExpandable().Where(expr)
                    select new ReferenceListViewModel()
                    {
                        Name = b.Name,
                        FileName = b.FileName,
                        Id = b.Id,
                        Active= b.Active,
                    });
        }
        public IQueryable<ReferenceListViewModel> GetReferencesListIQueryable(ReferenceSearchViewModel referenceSearchView)
        {
            var predicate = PredicateBuilder.New<Data.References>(true);/*AND*/
            if (!string.IsNullOrWhiteSpace(referenceSearchView.Name))
            {
                predicate.And(a => a.Name.Contains(referenceSearchView.Name));
            }

            return _getReferencesListIQueryable(predicate);
        }
        public async Task<ReferenceListViewModel> GetReferenceListViewAsync(long referenceId)
        {

            var predicate = PredicateBuilder.New<Data.References>(true);/*AND*/
            predicate.And(a => a.Id == referenceId);
            var reference = await _getReferencesListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return reference;
        }
        public async Task<ServiceCallResult> AddReferenceAsync(ReferenceAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.References.AnyAsync(a => a.Name == model.Name).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Referans bulunmaktadır.");
                return callResult;
            }

            var reference = new References()
            {
                Name = model.Name,
                FileName = model.FileName,
                Active= model.Active,
            };
            _context.References.Add(reference);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetReferenceListViewAsync(reference.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<ReferenceEditViewModel> GetReferenceEditViewModelAsync(int referenceId)
        {
            var reference = await (from b in _context.References
                                   where b.Id == referenceId
                                   select new ReferenceEditViewModel()
                                   {
                                       Name = b.Name,
                                       Id = b.Id,
                                       Active= b.Active,

                                   }).FirstOrDefaultAsync();
            return reference;
        }
        public async Task<ServiceCallResult> EditReferenceAsync(ReferenceEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.References.AnyAsync(a => a.Name == model.Name && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Referans bulunmaktadır.");
                return callResult;
            }

            var reference = await _context.References.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (reference == null)
            {
                callResult.ErrorMessages.Add("Böyle bir Referans bulunamadı.");
                return callResult;
            }

            reference.FileName = string.IsNullOrWhiteSpace(model.FileName) ? reference.FileName : model.FileName;
            reference.Name = model.Name;
            reference.Active = model.Active;

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    callResult.Success = true;
                    callResult.Item = await GetReferenceListViewAsync(reference.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<ServiceCallResult> DeleteReferenceAsync(int referenceId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var reference = await _context.References.FirstOrDefaultAsync(a => a.Id == referenceId).ConfigureAwait(false);
            if (reference == null)
            {
                callResult.ErrorMessages.Add("Böyle bir Referans bulunamadı.");
                return callResult;
            }
            _context.References.Remove(reference);
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

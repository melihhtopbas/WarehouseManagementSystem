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
    public class PropertyService
    {
        private readonly WarehouseManagementSystemEntities1 _context;
        public PropertyService(WarehouseManagementSystemEntities1 context)
        {
            _context = context;
        }
        private IQueryable<PropertyListViewModel> _getPropertiesListIQueryable(Expression<Func<Data.Properties, bool>> expr)
        {
            return (from b in _context.Properties.AsExpandable().Where(expr)
                    select new PropertyListViewModel()
                    {
                        Name = b.Name,
                        Id = b.Id,
                        Link = b.Link,
                        MainImage = b.FileName,
                        MainIcon = b.Icon
                    });
        }
        public IQueryable<PropertyListViewModel> GetPropertiesListIQueryable(PropertySearchViewModel model)
        {
            var predicate = PredicateBuilder.New<Data.Properties>(true);/*AND*/
            predicate.And(a => a.LanguageId == model.LanguageId);
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                predicate.And(a => a.Name.Contains(model.Name));
            }

            return _getPropertiesListIQueryable(predicate);
        }
        public async Task<PropertyListViewModel> GetPropertiesListViewAsync(long propertyId)
        {

            var predicate = PredicateBuilder.New<Data.Properties>(true);/*AND*/
            predicate.And(a => a.Id == propertyId);
            var property = await _getPropertiesListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return property;
        }
        public async Task<ServiceCallResult> AddPropertiesAsync(PropertyAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.Properties.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Özellik bulunmaktadır.");
                return callResult;
            }

            var property = new Properties()
            {
                Name = model.Name,
                FileName = model.FileName,
                Icon = model.Icon,
                ShortDescription = model.ShortDescription,
                Description = model.Description,
                Link = HelperMethods.UrlFriendly(model.Name),
                Active = model.Active,
                LanguageId = model.LanguageId

            };
            _context.Properties.Add(property);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetPropertiesListViewAsync(property.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<PropertyEditViewModel> GetPropertiesEditViewModelAsync(int propertyId)
        {
            var property = await (from b in _context.Properties
                                  where b.Id == propertyId
                                  select new PropertyEditViewModel()
                                  {
                                      Name = b.Name,
                                      Id = b.Id,
                                      LanguageId = b.LanguageId,
                                      Active = b.Active,
                                      ShortDescription = b.ShortDescription,
                                      Description = b.Description

                                  }).FirstOrDefaultAsync();
            return property;
        }
        public async Task<ServiceCallResult> EditPropertiesAsync(PropertyEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.Properties.AnyAsync(a => a.Name == model.Name && a.LanguageId == model.LanguageId && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu isimde Özellik bulunmaktadır.");
                return callResult;
            }

            var property = await _context.Properties.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (property == null)
            {
                callResult.ErrorMessages.Add("Böyle bir Özellik bulunamadı.");
                return callResult;
            }

            property.FileName = string.IsNullOrWhiteSpace(model.FileName) ? property.FileName : model.FileName;
            property.Icon = string.IsNullOrWhiteSpace(model.Icon) ? property.Icon : model.Icon;
            property.Description = model.Description;
            property.ShortDescription = model.ShortDescription;
            property.Name = model.Name;
            property.Active = model.Active;
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    callResult.Success = true;
                    callResult.Item = await GetPropertiesListViewAsync(property.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<ServiceCallResult> DeletePropertiesAsync(int propertyId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var property = await _context.Properties.FirstOrDefaultAsync(a => a.Id == propertyId).ConfigureAwait(false);
            if (property == null)
            {
                callResult.ErrorMessages.Add("Böyle bir Özellik bulunamadı.");
                return callResult;
            }
            _context.Properties.Remove(property);
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

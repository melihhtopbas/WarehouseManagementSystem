using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Utils.Constants;
using Warehouse.ViewModels.Admin;
using Warehouse.ViewModels.Common; 

namespace Warehouse.Service.Admin
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
                        ImageUrl = s.FileName,
                        Id = s.Id

                    });
        }

        public IQueryable<SliderListViewModel> GetSliderListIQueryable(long languageId)
        {
            var predicate = PredicateBuilder.New<Data.Sliders>(true);/*AND*/
            predicate.And(a => a.LanguageId == languageId);
            return _getSliderListIQueryable(predicate);
        }
        public async Task<SliderListViewModel> GetSliderListViewAsync(long sliderId)
        {

            var predicate = PredicateBuilder.New<Data.Sliders>(true);/*AND*/
            predicate.And(a => a.Id == sliderId);
            var product = await _getSliderListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return product;
        }

        public async Task<ServiceCallResult> AddSliderAsync(SliderCrudViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            var slider = new Sliders()
            {
                LanguageId = model.LanguageId,
                Title = model.Title,
                Active = model.Active,
                Alt = model.Alt,
                ButtonLink1 = model.ButtonLink1,
                ButtonLink2 = model.ButtonLink2,
                ButtonLink3 = model.ButtonLink3,
                ButtonLink4 = model.ButtonLink4,
                ButtonText1 = model.ButtonText1,
                ButtonText2 = model.ButtonText2,
                ButtonText3 = model.ButtonText3,
                ButtonText4 = model.ButtonText4,
                FileName = model.FileName,
                SortOrder = model.SortOrder

            };
            _context.Sliders.Add(slider);

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetSliderListViewAsync(slider.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<SliderCrudViewModel> GetSliderViewModelAsync(int serviceCategoryId)
        {
            var slider = await (from b in _context.Sliders
                                where b.Id == serviceCategoryId
                                select new SliderCrudViewModel()
                                {

                                    Id = b.Id,
                                    LanguageId = b.LanguageId,
                                    Active = b.Active,
                                    SortOrder = b.SortOrder,
                                    Title = b.Title,
                                    FileName = b.FileName,
                                    Alt = b.Alt,
                                    ButtonLink3 = b.ButtonLink3,
                                    ButtonLink2 = b.ButtonLink2,
                                    ButtonLink1 = b.ButtonLink1,
                                    ButtonLink4 = b.ButtonLink4,
                                    ButtonText4 = b.ButtonText4,
                                    ButtonText2 = b.ButtonText2,
                                    ButtonText3 = b.ButtonText3,
                                    ButtonText1 = b.ButtonText1

                                }).FirstOrDefaultAsync();
            return slider;
        }
        public async Task<ServiceCallResult> EditSliderCategoryAsync(SliderCrudViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var slider = await _context.Sliders.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (slider == null)
            {
                callResult.ErrorMessages.Add("Böyle bir slider bulunamadı.");
                return callResult;
            }

            slider.ButtonLink1 = model.ButtonLink1;
            slider.ButtonLink2 = model.ButtonLink2;
            slider.ButtonLink3 = model.ButtonLink3;
            slider.ButtonLink4 = model.ButtonLink4;
            slider.ButtonText1 = model.ButtonText1;
            slider.ButtonText2 = model.ButtonText2;
            slider.ButtonText3 = model.ButtonText3;
            slider.ButtonText4 = model.ButtonText4;
            slider.Title = model.Title;
            slider.FileName = model.FileName;
            slider.SortOrder = model.SortOrder;
            slider.Active = model.Active;
            slider.Alt = model.Alt;

            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();
                    callResult.Success = true;
                    callResult.Item = await GetSliderListViewAsync(slider.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<ServiceCallResult> DeleteSliderAsync(int sliderId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var slider = await _context.Sliders.FirstOrDefaultAsync(a => a.Id == sliderId).ConfigureAwait(false);
            if (slider == null)
            {
                callResult.ErrorMessages.Add("Böyle bir slider bulunamadı.");
                return callResult;
            }


            var deletedImagesList = new List<string>();
            deletedImagesList.Add(slider.FileName);

            _context.Sliders.Remove(slider);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();

                    if (deletedImagesList.Any())
                    {
                        var imagesDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + String.Format(SystemConstants.SliderServiceImagePath));
                        var imagesThumbDirectory = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + String.Format(SystemConstants.SliderServiceImageThumbPath));

                        foreach (var deletedImage in deletedImagesList)
                        {
                            var physicalPath = System.IO.Path.Combine(imagesDirectory, deletedImage);
                            var physicalThumbPath = System.IO.Path.Combine(imagesThumbDirectory, deletedImage);

                            try
                            {
                                if (System.IO.File.Exists(physicalPath)) { System.IO.File.Delete(physicalPath); }
                                if (System.IO.File.Exists(physicalThumbPath)) { System.IO.File.Delete(physicalThumbPath); }
                            }
                            catch (Exception) { }
                        }

                    }

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

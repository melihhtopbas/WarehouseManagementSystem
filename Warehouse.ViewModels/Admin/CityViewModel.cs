using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq; 

namespace Warehouse.ViewModels.Admin
{
    public class CityAddViewModel : CityCrudBaseViwModel
    {
      
    }
    public class CityListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ülke adı")]

        public string Name { get; set; }
        public string CountryName { get; set; }
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

    }
    public class CityEditViewModel : CityCrudBaseViwModel
    {
        public long Id { get; set; }
    }
    public class CityCrudBaseViwModel
    {
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages),
         ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }
        public long LanguageId { get; set; }
        public OrderCountryIdSelectViewModel Country { get; set; }

    }

    public class OrderCityIdSelectViewModel
    {
        [Display(Name = "Şehir Adı")] 
        [Required(ErrorMessage = "Şehir adı gerekli")]
        public long? CityId { get; set; }
    }
    public class CitySearchViewModel
    {
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Language")]
        public long LanguageId { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages),
            ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }
        [StringLength(200)]
        [Display(Name = "Ülke Adı")]
        public string CountryName { get; set; }
    }
}

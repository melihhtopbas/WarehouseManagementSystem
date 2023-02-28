using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class SliderListViewModel
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
    }
    public class SliderSearchViewModel
    {
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "Language")]
        public long LanguageId { get; set; }


    }

    public class SliderCrudViewModel
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }


        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "SortOrder")]
        public int SortOrder { get; set; }

        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "SeoTitle")]
        public string Title { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "Alt")]
        public string Alt { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonText")]
        public string ButtonText1 { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonText")]
        public string ButtonText2 { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonText")]
        public string ButtonText3 { get; set; }
        [StringLength(50, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonText")]
        public string ButtonText4 { get; set; }

        [StringLength(200, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonLink")]
        public string ButtonLink1 { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonLink")]
        public string ButtonLink2 { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonLink")]
        public string ButtonLink3 { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "ButtonLink")]
        public string ButtonLink4 { get; set; }

        public string FileName { get; set; }

    }
}
